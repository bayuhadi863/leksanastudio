using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId);
    }
}
