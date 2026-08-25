using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Auth;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<AuthDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] BaseRequest<LoginParam> request)
        {
            var result = await _authService.LoginAsync(request.Data);
            return Ok(BaseResponse<AuthDTO>.Ok(result));
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(BaseResponse<AuthDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] BaseRequest<RefreshTokenParam> request)
        {
            var result = await _authService.RefreshTokenAsync(request.Data);
            return Ok(BaseResponse<AuthDTO>.Ok(result));
        }

        [HttpPost("revoke")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Revoke([FromBody] BaseRequest<RefreshTokenParam> request)
        {
            await _authService.RevokeTokenAsync(request.Data.RefreshToken);
            return Ok(BaseResponse<object>.Ok(null!, "Token revoked"));
        }

        [HttpPost("logout")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? string.Empty;
            await _authService.LogoutAsync(userId, jti);
            return Ok(BaseResponse<object>.Ok(null!, "Logged out successfully"));
        }

        [HttpGet("user-info")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _authService.GetUserInfoAsync(userId);
            return Ok(BaseResponse<UserDTO>.Ok(result));
        }

        [HttpGet("my-roles")]
        [JwtAuthorize]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<UserRoleSummaryDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetMyRoles()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _authService.GetMyRolesAsync(userId);
            return Ok(BaseResponse<IEnumerable<UserRoleSummaryDTO>>.Ok(result));
        }
    }
}
