using LeksanaStudio.Application.DTOs.Auth;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthDTO> LoginAsync(LoginParam param);
        Task<AuthDTO> RefreshTokenAsync(RefreshTokenParam param);
        Task RevokeTokenAsync(string token);
        Task LogoutAsync(Guid userId, string jti);
        Task<UserDTO> GetUserInfoAsync(Guid userId);

        /// <summary>Roles assigned to the current user, for the active-role switcher.</summary>
        Task<IEnumerable<UserRoleSummaryDTO>> GetMyRolesAsync(Guid userId);
    }
}
