using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class UserRefreshTokenRepository
        : BaseRepository<UserRefreshToken>,
            IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(AppDbContext context)
            : base(context) { }
    }
}
