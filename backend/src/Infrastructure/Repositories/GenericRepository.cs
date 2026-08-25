using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    /// <summary>
    /// A repository for entities that need nothing beyond the base contract —
    /// translation rows, join rows, and the like.
    ///
    /// Registered as an open generic so a new translation table costs no file at
    /// all. Modules that need their own queries still declare their own
    /// repository, and that registration wins.
    /// </summary>
    public class GenericRepository<T> : BaseRepository<T>
        where T : class, IBaseEntity
    {
        public GenericRepository(AppDbContext context)
            : base(context) { }
    }
}
