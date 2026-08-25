using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Repositories
{
    public interface ISlugHistoryRepository : IBaseRepository<SlugHistory>
    {
        /// <summary>
        /// The current address for a slug that used to point somewhere, or null when
        /// the slug was never used. Drives the 301 the site answers with.
        /// </summary>
        Task<SlugHistory?> FindAsync(string entityType, string localeCode, string oldSlug);
    }
}
