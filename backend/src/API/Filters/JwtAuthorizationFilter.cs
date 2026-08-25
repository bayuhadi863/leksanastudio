using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.API.Filters
{
    public class JwtAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ITokenBlacklistService _blacklist;

        public JwtAuthorizationFilter(ITokenBlacklistService blacklist)
        {
            _blacklist = blacklist;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Skip auth check for actions explicitly marked as public
            if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
                return;

            var authHeader = context.HttpContext.Request.Headers.Authorization.ToString();

            if (
                string.IsNullOrEmpty(authHeader)
                || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            )
            {
                throw new UnauthorizedException("Missing or invalid Authorization header");
            }

            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedException("Unauthorized access");
            }

            var jti = context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (!string.IsNullOrEmpty(jti) && await _blacklist.IsBlacklistedAsync(jti))
            {
                throw new UnauthorizedException("Token has been revoked");
            }
        }
    }
}
