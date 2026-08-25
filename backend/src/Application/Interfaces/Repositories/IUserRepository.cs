using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
