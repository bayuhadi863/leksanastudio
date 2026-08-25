using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LeksanaStudio.Application.DTOs.Auth;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Helpers;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Services
{
    /// <summary>
    /// Email + password authentication against the local database.
    ///
    /// There is no self-registration endpoint: accounts on a content-management
    /// panel are created by an administrator (or the startup seeder), never by a
    /// visitor. See <c>UserService</c> for account creation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IJwtService _jwtService;
        private readonly ITokenBlacklistService _tokenBlacklist;
        private readonly JwtOptions _jwtOptions;

        public AuthService(
            IUserRepository userRepository,
            IUserRefreshTokenRepository refreshTokenRepository,
            IUserRoleRepository userRoleRepository,
            IJwtService jwtService,
            ITokenBlacklistService tokenBlacklist,
            IOptions<JwtOptions> jwtOptions
        )
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _userRoleRepository = userRoleRepository;
            _jwtService = jwtService;
            _tokenBlacklist = tokenBlacklist;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthDTO> LoginAsync(LoginParam param)
        {
            var email = (param.Email ?? string.Empty).Trim();

            var user = await _userRepository.GetByEmailAsync(email);

            // One message for every failure mode — a distinct "unknown email"
            // response would let anyone enumerate the accounts that exist.
            if (
                user == null
                || user.IsDeleted
                || string.IsNullOrEmpty(user.Password)
                || !PasswordHash.Verify(param.Password, user.Password)
            )
            {
                throw new BadRequestException("Email atau kata sandi salah");
            }

            return await GenerateAuthDTO(user);
        }

        public async Task<AuthDTO> RefreshTokenAsync(RefreshTokenParam param)
        {
            await _refreshTokenRepository.BeginTransactionAsync();
            try
            {
                var refreshToken = await _refreshTokenRepository.GetAsync(q =>
                    q.Include(u => u.User)
                        .Where(u => u.Token == param.RefreshToken && u.IsRevoked != true)
                );

                if (
                    refreshToken == null
                    || refreshToken.ExpiresAt == null
                    || refreshToken.ExpiresAt < DateTimeOffset.UtcNow
                    // Absolute cap: even an actively-refreshed session is dead past this.
                    || (
                        refreshToken.AbsoluteExpiresAt != null
                        && refreshToken.AbsoluteExpiresAt < DateTimeOffset.UtcNow
                    )
                )
                {
                    throw new UnauthorizedException("Invalid or expired refresh token");
                }

                refreshToken.IsRevoked = true;
                refreshToken.UpdatedDate = DateTimeOffset.UtcNow;
                await _refreshTokenRepository.UpdateAsync(refreshToken);

                User user =
                    refreshToken.User
                    ?? throw new NotFoundException("User not found in refresh token");
                user.UpdatedDate = DateTimeOffset.UtcNow;
                user.UpdatedBy = user.Email;
                await _userRepository.UpdateAsync(user);

                // Rotate the refresh token: the sliding window resets (active session
                // stays alive), but the ORIGINAL absolute cap is inherited unchanged so
                // total session lifetime stays bounded — it can't be refreshed forever.
                var response = await GenerateAuthDTO(user, refreshToken.AbsoluteExpiresAt);
                await _refreshTokenRepository.CommitTransactionAsync();
                return response;
            }
            catch
            {
                await _refreshTokenRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetAsync(q =>
                q.Where(u => u.Token == token)
            );

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.UpdatedDate = DateTimeOffset.UtcNow;
                await _refreshTokenRepository.UpdateAsync(refreshToken);
            }
        }

        public async Task LogoutAsync(Guid userId, string jti)
        {
            await _refreshTokenRepository.BeginTransactionAsync();
            try
            {
                var tokens = await _refreshTokenRepository.GetListAsync(q =>
                    q.Where(u => u.UserId == userId && u.IsRevoked != true)
                );

                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.UpdatedDate = DateTimeOffset.UtcNow;
                    await _refreshTokenRepository.UpdateAsync(token);
                }

                await _refreshTokenRepository.CommitTransactionAsync();

                // Kills the still-valid access token too, so logout is immediate
                // rather than "until the current token expires".
                if (!string.IsNullOrEmpty(jti))
                {
                    await _tokenBlacklist.BlacklistAsync(
                        jti,
                        TimeSpan.FromMinutes(_jwtOptions.AccessTokenExpirationInMinutes)
                    );
                }
            }
            catch
            {
                await _refreshTokenRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserDTO> GetUserInfoAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(q => q.Where(u => u.Id == userId));

            if (user == null || user.IsDeleted)
            {
                throw new NotFoundException("User not found");
            }

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy,
            };
        }

        public async Task<IEnumerable<UserRoleSummaryDTO>> GetMyRolesAsync(Guid userId)
        {
            var userRoles = await _userRoleRepository.GetByUserIdAsync(userId);

            return userRoles
                .Where(ur => ur.Role != null && !ur.Role.IsDeleted)
                .OrderBy(ur => ur.Role!.Order ?? int.MaxValue)
                .ThenBy(ur => ur.Role!.Name)
                .Select(ur => new UserRoleSummaryDTO
                {
                    RoleId = ur.RoleId,
                    RoleCode = ur.Role!.Code,
                    RoleName = ur.Role.Name,
                    DefaultMenuCode = ur.Role.DefaultMenu?.Code,
                })
                .ToList();
        }

        private async Task<AuthDTO> GenerateAuthDTO(
            User user,
            DateTimeOffset? inheritedAbsoluteExpiresAt = null
        )
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            };

            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Hybrid expiry: absolute cap is set fresh on login, inherited on refresh.
            // The token's expiry is the sliding window clamped to the absolute cap.
            var now = DateTimeOffset.UtcNow;
            var absoluteExpiresAt =
                inheritedAbsoluteExpiresAt
                ?? now.AddDays(_jwtOptions.AbsoluteRefreshTokenExpirationInDays);
            var slidingExpiresAt = now.AddDays(_jwtOptions.RefreshTokenExpirationInDays);
            var expiresAt =
                slidingExpiresAt < absoluteExpiresAt ? slidingExpiresAt : absoluteExpiresAt;

            var userRefreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = expiresAt,
                AbsoluteExpiresAt = absoluteExpiresAt,
                CreatedBy = user.Email ?? string.Empty,
            };

            await _refreshTokenRepository.CreateAsync(userRefreshToken);

            return new AuthDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _jwtOptions.AccessTokenExpirationInMinutes * 60,
            };
        }
    }
}
