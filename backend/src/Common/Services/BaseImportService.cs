using LeksanaStudio.Common.Import;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Common.Services
{
    /// <summary>
    /// Shared engine for Excel imports: reads the workbook, runs the module's row
    /// parser/validator, builds the dry-run preview, and performs the transactional
    /// upsert on commit. Modules supply only their column layout, master lookups,
    /// business key, and entity mapping.
    /// </summary>
    /// <typeparam name="TEntity">The module entity being imported.</typeparam>
    /// <typeparam name="TContext">
    /// Per-module lookup bag (master name→id dictionaries) loaded once per request.
    /// </typeparam>
    public abstract class BaseImportService<TEntity, TContext> : IImportService
        where TEntity : class, IBaseEntity
        where TContext : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly ICurrentUserService _currentUser;
        protected readonly ITableImporter _importer;

        protected BaseImportService(
            IBaseRepository<TEntity> repository,
            ICurrentUserService currentUser,
            ITableImporter importer
        )
        {
            _repository = repository;
            _currentUser = currentUser;
            _importer = importer;
        }

        protected string CurrentUserName => _currentUser.UserName ?? "SYSTEM";

        // ---- Module hooks ----

        protected abstract string TemplateTitle { get; }
        protected abstract string TemplateFileName { get; }
        protected abstract Task<IReadOnlyList<ImportColumn>> BuildTemplateColumnsAsync();
        protected abstract Task<TContext> LoadContextAsync();
        protected abstract ParsedRow ParseRow(ImportRawRow raw, TContext ctx);

        /// <summary>Upsert key for an existing entity; null to exclude it from matching.</summary>
        protected abstract string? ExistingKey(TEntity entity, TContext ctx);

        /// <summary>Create the user-approved new masters, mutating <paramref name="ctx"/>.</summary>
        protected abstract Task<int> CreateApprovedMastersAsync(
            ImportApprovedMastersDTO approved,
            TContext ctx
        );

        protected abstract TEntity CreateEntity(ParsedRow row, TContext ctx);
        protected abstract void UpdateEntity(TEntity entity, ParsedRow row, TContext ctx);

        // ---- Template ----

        public async Task<(byte[] Bytes, string FileName)> BuildTemplateAsync()
        {
            var columns = await BuildTemplateColumnsAsync();
            var bytes = _importer.BuildTemplate(TemplateTitle, columns);
            return (bytes, $"{TemplateFileName}.xlsx");
        }

        // ---- Validate (dry-run) ----

        public async Task<ImportPreviewDTO> ValidateAsync(Stream file)
        {
            var rawRows = _importer.ReadRows(file);
            var ctx = await LoadContextAsync();
            var existing = await BuildExistingKeyMapAsync(ctx);

            var previews = new List<ImportPreviewRowDTO>();
            var newMasters = new Dictionary<string, ImportNewMasterDTO>();
            var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var raw in rawRows)
            {
                var parsed = ParseRow(raw, ctx);
                string action;
                Guid? existingId = null;

                if (parsed.Errors.Count > 0)
                {
                    action = "invalid";
                }
                else if (parsed.BusinessKey != null && !seenKeys.Add(parsed.BusinessKey))
                {
                    parsed.Errors.Add("Baris duplikat di dalam file (judul & tahun sama)");
                    action = "invalid";
                }
                else if (
                    parsed.BusinessKey != null
                    && existing.TryGetValue(parsed.BusinessKey, out var match)
                )
                {
                    action = "update";
                    existingId = match.Id;
                }
                else
                {
                    action = "insert";
                }

                foreach (var m in parsed.MasterRefs.Where(m => m.IsNew))
                {
                    var mapKey = $"{m.Kind}{m.Name}";
                    if (newMasters.TryGetValue(mapKey, out var nm))
                        nm.RowCount++;
                    else
                        newMasters[mapKey] = new ImportNewMasterDTO
                        {
                            Kind = m.Kind,
                            Name = m.Name,
                            Required = m.Required,
                            RowCount = 1,
                        };
                }

                previews.Add(
                    new ImportPreviewRowDTO
                    {
                        RowNumber = raw.RowNumber,
                        Values = parsed.Values,
                        Action = action,
                        ExistingId = existingId,
                        Errors = parsed.Errors,
                        MasterRefs = parsed.MasterRefs,
                    }
                );
            }

            return new ImportPreviewDTO
            {
                Summary = new ImportSummaryDTO
                {
                    Total = previews.Count,
                    ToInsert = previews.Count(p => p.Action == "insert"),
                    ToUpdate = previews.Count(p => p.Action == "update"),
                    Invalid = previews.Count(p => p.Action == "invalid"),
                },
                Rows = previews,
                NewMasters = newMasters
                    .Values.OrderBy(m => m.Kind)
                    .ThenBy(m => m.Name)
                    .ToList(),
            };
        }

        // ---- Commit (upsert) ----

        public async Task<ImportCommitResultDTO> CommitAsync(ImportCommitRequest request)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var ctx = await LoadContextAsync();
                var mastersCreated = await CreateApprovedMastersAsync(
                    request.CreateMasters ?? new ImportApprovedMastersDTO(),
                    ctx
                );
                var existing = await BuildExistingKeyMapAsync(ctx);

                var toInsert = new List<TEntity>();
                var toUpdate = new List<TEntity>();
                var errors = new List<ImportRowErrorDTO>();
                var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var skipped = 0;

                foreach (var row in request.Rows ?? new List<ImportCommitRowDTO>())
                {
                    var raw = new ImportRawRow
                    {
                        RowNumber = row.RowNumber,
                        Values = row.Values ?? new Dictionary<string, string?>(),
                    };
                    var parsed = ParseRow(raw, ctx);

                    if (parsed.Errors.Count > 0)
                    {
                        skipped++;
                        errors.Add(
                            new ImportRowErrorDTO
                            {
                                RowNumber = row.RowNumber,
                                Message = parsed.Errors[0],
                            }
                        );
                        continue;
                    }

                    var unresolved = parsed.MasterRefs.FirstOrDefault(m => m.Required && m.IsNew);
                    if (unresolved != null)
                    {
                        skipped++;
                        errors.Add(
                            new ImportRowErrorDTO
                            {
                                RowNumber = row.RowNumber,
                                Message =
                                    $"{MasterLabel(unresolved.Kind)} \"{unresolved.Name}\" belum dibuat",
                            }
                        );
                        continue;
                    }

                    if (parsed.BusinessKey != null && !seenKeys.Add(parsed.BusinessKey))
                    {
                        skipped++;
                        errors.Add(
                            new ImportRowErrorDTO
                            {
                                RowNumber = row.RowNumber,
                                Message = "Baris duplikat di dalam file",
                            }
                        );
                        continue;
                    }

                    if (
                        parsed.BusinessKey != null
                        && existing.TryGetValue(parsed.BusinessKey, out var match)
                    )
                    {
                        UpdateEntity(match, parsed, ctx);
                        match.UpdatedDate = DateTimeOffset.UtcNow;
                        match.UpdatedBy = CurrentUserName;
                        toUpdate.Add(match);
                    }
                    else
                    {
                        var entity = CreateEntity(parsed, ctx);
                        entity.CreatedDate = DateTimeOffset.UtcNow;
                        entity.CreatedBy = CurrentUserName;
                        toInsert.Add(entity);
                        if (parsed.BusinessKey != null)
                            existing[parsed.BusinessKey] = entity;
                    }
                }

                if (toInsert.Count > 0)
                    await _repository.CreateRangeAsync(toInsert);
                if (toUpdate.Count > 0)
                    await _repository.UpdateRangeAsync(toUpdate);

                await _repository.CommitTransactionAsync();

                return new ImportCommitResultDTO
                {
                    Inserted = toInsert.Count,
                    Updated = toUpdate.Count,
                    Skipped = skipped,
                    MastersCreated = mastersCreated,
                    Errors = errors,
                };
            }
            catch (Exception)
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<Dictionary<string, TEntity>> BuildExistingKeyMapAsync(TContext ctx)
        {
            var all = await _repository.GetListAsync();
            var map = new Dictionary<string, TEntity>(StringComparer.OrdinalIgnoreCase);
            foreach (var entity in all)
            {
                var key = ExistingKey(entity, ctx);
                if (key != null)
                    map[key] = entity;
            }
            return map;
        }

        /// <summary>Reads a trimmed, non-empty cell value by header, or null.</summary>
        protected static string? Val(Dictionary<string, string?> values, string header) =>
            values.TryGetValue(header, out var s) && !string.IsNullOrWhiteSpace(s) ? s.Trim() : null;

        /// <summary>Human-readable label for an unresolved "master" reference, used in
        /// import error messages. Neutral by default — domain import services override
        /// this to localise per <paramref name="kind"/>.</summary>
        protected virtual string MasterLabel(string kind) => "Master";
    }
}
