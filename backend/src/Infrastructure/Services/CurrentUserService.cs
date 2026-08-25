using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using LeksanaStudio.Common.Constants;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name
            ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public Guid? UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );
                return Guid.TryParse(id, out var guid) ? guid : null;
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public Guid? ActiveRoleId
        {
            get
            {
                var raw = _httpContextAccessor
                    .HttpContext?.Request.Headers[HeaderNames.ActiveRole]
                    .ToString();
                return Guid.TryParse(raw, out var guid) ? guid : null;
            }
        }
    }
}
