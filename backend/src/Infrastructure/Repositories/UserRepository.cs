using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context)
            : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await GetAsync(q =>
                q.Where(u => u.Email == email)
            );
        }
    }
}
