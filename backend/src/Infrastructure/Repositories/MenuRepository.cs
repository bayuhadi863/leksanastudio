using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(AppDbContext context)
            : base(context) { }
    }
}
