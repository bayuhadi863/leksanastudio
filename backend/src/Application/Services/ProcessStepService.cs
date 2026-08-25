using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.ProcessStep;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// How a project runs, step by step.
    ///
    /// The least-spoken fear is not price — it is that the process will be a mess
    /// and nobody will know what to do next. Each step answers that before it is
    /// asked, including what the client has to supply, which is where projects
    /// actually slip.
    /// </summary>
    public class ProcessStepService
        : BaseTranslatableCrudService<
            ProcessStep,
            ProcessStepTranslation,
            ProcessStepDTO,
            ProcessStepParam,
            ProcessStepTranslationParam,
            ProcessStepPaginationDTO,
            ProcessStepPaginationParam
        >,
            IProcessStepService
    {
        public ProcessStepService(
            IBaseRepository<ProcessStep> repository,
            IBaseRepository<ProcessStepTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "process-step";

        protected override bool SlugRequired => false;

        protected override string? DisplayName(ProcessStepTranslation translation) =>
            translation.Title;

        protected override IQueryable<ProcessStep> ApplyFilter(
            IQueryable<ProcessStep> query,
            ProcessStepPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(p =>
                    p.Translations.Any(t =>
                        !t.IsDeleted && t.Title != null && EF.Functions.ILike(t.Title, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(ProcessStep entity, ProcessStepDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<ProcessStepTranslationDTO>(entity.Id);
        }

        public async Task<IEnumerable<ProcessStepPublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);

            return entities
                .OrderBy(p => p.Step)
                .ThenBy(p => p.Order)
                .Select(entity => ToPublic(entity, localeCode))
                .OfType<ProcessStepPublicDTO>();
        }

        private static ProcessStepPublicDTO? ToPublic(ProcessStep entity, string localeCode)
        {
            var translation = LiveTranslations(entity)
                .FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new ProcessStepPublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Step = entity.Step,
                Order = entity.Order,
                Title = translation.Title,
                Duration = translation.Duration,
                Summary = translation.Summary,
                Details = translation.Details.Adapt<System.Text.Json.JsonElement?>(),
                ClientInput = translation.ClientInput,
            };
        }
    }
}
