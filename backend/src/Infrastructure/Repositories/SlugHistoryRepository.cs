using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class SlugHistoryRepository : BaseRepository<SlugHistory>, ISlugHistoryRepository
    {
        public SlugHistoryRepository(AppDbContext context)
            : base(context) { }

        public async Task<SlugHistory?> FindAsync(
            string entityType,
            string localeCode,
            string oldSlug
        ) =>
            await _dbSet
                .Where(h =>
                    h.EntityType == entityType
                    && h.LocaleCode == localeCode
                    && h.OldSlug == oldSlug
                )
                .OrderByDescending(h => h.CreatedDate)
                .FirstOrDefaultAsync();
    }
}
