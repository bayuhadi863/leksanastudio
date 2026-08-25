using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context)
            : base(context) { }

        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(ur => !ur.IsDeleted && ur.UserId == userId)
                .Include(ur => ur.Role)
                .ThenInclude(r => r.DefaultMenu)
                .ToListAsync();
        }
    }
}
