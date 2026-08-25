using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Locale;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Services
{
    public class LocaleService
        : BaseCrudService<Locale, LocaleDTO, LocaleParam, LocalePaginationDTO, LocalePaginationParam>,
            ILocaleService
    {
        private string? _defaultCode;

        public LocaleService(IBaseRepository<Locale> repository, ICurrentUserService currentUserService)
            : base(repository, currentUserService) { }

        protected override IQueryable<Locale> ApplyFilter(
            IQueryable<Locale> query,
            LocalePaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                query = query.Where(l =>
                    EF.Functions.ILike(l.Code, $"%{param.Search}%")
                    || EF.Functions.ILike(l.Name, $"%{param.Search}%")
                );
            }

            return query.OrderBy(l => l.Order);
        }

        public async Task<IEnumerable<LocaleDTO>> GetActiveAsync()
        {
            var locales = await _repository.GetListAsync(q =>
                q.Where(l => l.IsActive).OrderBy(l => l.Order)
            );
            return locales.Adapt<IEnumerable<LocaleDTO>>();
        }

        public async Task<string> GetDefaultCodeAsync()
        {
            if (_defaultCode is not null)
                return _defaultCode;

            var fallback = await _repository.GetAsync(q =>
                q.Where(l => l.IsDefault).OrderBy(l => l.Order)
            );

            // A site with no default language cannot decide what to serve at the
            // root, so this is a configuration error worth failing loudly on rather
            // than guessing past.
            _defaultCode =
                fallback?.Code
                ?? throw new InternalServerErrorException(
                    "Tidak ada bahasa bawaan yang ditetapkan. Tandai satu bahasa sebagai bawaan."
                );

            return _defaultCode;
        }

        /// <summary>
        /// Exactly one default, always. Promoting a language demotes the previous
        /// one in the same transaction — two defaults is a state the rest of the
        /// system has no sensible answer for.
        /// </summary>
        protected override void OnCreating(Locale entity, LocaleParam param)
        {
            entity.Code = entity.Code.Trim().ToLowerInvariant();
        }

        protected override void OnUpdating(Locale entity, LocaleParam param)
        {
            entity.Code = entity.Code.Trim().ToLowerInvariant();
        }

        public override async Task<Guid> CreateAsync(LocaleParam param)
        {
            var id = await base.CreateAsync(param);
            if (param.IsDefault)
                await DemoteOtherDefaultsAsync(id);
            return id;
        }

        public override async Task<Guid> UpdateAsync(Guid id, LocaleParam param)
        {
            var updated = await base.UpdateAsync(id, param);
            if (param.IsDefault)
                await DemoteOtherDefaultsAsync(id);
            return updated;
        }

        public override async Task<Guid> DeleteAsync(Guid id)
        {
            var locale = await _repository.GetAsync(q => q.Where(l => l.Id == id));
            if (locale is { IsDefault: true })
            {
                throw new BadRequestException(
                    "Bahasa bawaan tidak bisa dihapus. Tetapkan bahasa lain sebagai bawaan lebih dulu."
                );
            }

            return await base.DeleteAsync(id);
        }

        private async Task DemoteOtherDefaultsAsync(Guid keepId)
        {
            var others = (await _repository.GetListAsync(q => q.Where(l => l.IsDefault && l.Id != keepId)))
                .ToList();

            if (others.Count == 0)
                return;

            foreach (var locale in others)
            {
                locale.IsDefault = false;
                locale.UpdatedDate = DateTimeOffset.UtcNow;
                locale.UpdatedBy = _currentUserService.UserName;
            }

            await _repository.UpdateRangeAsync(others);
            _defaultCode = null;
        }
    }
}
