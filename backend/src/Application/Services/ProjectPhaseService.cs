using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.ProjectPhase;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Stages of system work.
    ///
    /// System work is sold as a sequence rather than as a package list, which is
    /// why the numbering here is real: a client can stop after any phase and still
    /// hold something that runs.
    /// </summary>
    public class ProjectPhaseService
        : BaseTranslatableCrudService<
            ProjectPhase,
            ProjectPhaseTranslation,
            ProjectPhaseDTO,
            ProjectPhaseParam,
            ProjectPhaseTranslationParam,
            ProjectPhasePaginationDTO,
            ProjectPhasePaginationParam
        >,
            IProjectPhaseService
    {
        public ProjectPhaseService(
            IBaseRepository<ProjectPhase> repository,
            IBaseRepository<ProjectPhaseTranslation> translations,
            ISlugHistoryRepository slugHistory,
            IBlockDocumentProcessor blocks,
            ILocaleService locales,
            ICurrentUserService currentUserService
        )
            : base(repository, translations, slugHistory, blocks, locales, currentUserService) { }

        protected override string ContentType => "project-phase";

        protected override bool SlugRequired => false;

        protected override string? DisplayName(ProjectPhaseTranslation translation) =>
            translation.Name;

        protected override IQueryable<ProjectPhase> ApplyFilter(
            IQueryable<ProjectPhase> query,
            ProjectPhasePaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var term = $"%{param.Search}%";
                query = query.Where(p =>
                    p.Translations.Any(t =>
                        !t.IsDeleted && t.Name != null && EF.Functions.ILike(t.Name, term)
                    )
                );
            }

            query = FilterByStatus(query, param.Status);

            return query;
        }

        protected override async Task EnrichSingleAsync(ProjectPhase entity, ProjectPhaseDTO dto)
        {
            dto.Translations = await LoadTranslationDtosAsync<ProjectPhaseTranslationDTO>(entity.Id);
        }

        public async Task<IEnumerable<ProjectPhasePublicDTO>> GetPublicListAsync(string localeCode)
        {
            var entities = await GetPublishedEntitiesAsync(localeCode);

            return entities
                .OrderBy(p => p.Step)
                .ThenBy(p => p.Order)
                .Select(entity => ToPublic(entity, localeCode))
                .OfType<ProjectPhasePublicDTO>();
        }

        private static ProjectPhasePublicDTO? ToPublic(ProjectPhase entity, string localeCode)
        {
            var translation = LiveTranslations(entity)
                .FirstOrDefault(t => t.LocaleCode == localeCode);
            if (translation is null)
                return null;

            return new ProjectPhasePublicDTO
            {
                Id = entity.Id,
                ContentKey = entity.ContentKey,
                LocaleCode = translation.LocaleCode,
                Step = entity.Step,
                Order = entity.Order,
                Name = translation.Name,
                Price = translation.Price,
                Duration = translation.Duration,
                Scope = translation.Scope,
                Note = translation.Note,
            };
        }
    }
}
