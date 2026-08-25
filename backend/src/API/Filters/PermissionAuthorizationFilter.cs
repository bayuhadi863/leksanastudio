using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.API.Filters
{
    /// <summary>
    /// Data-driven CRUD authorization. For any action carrying a
    /// <see cref="RequirePermissionAttribute"/> whose controller carries a
    /// <see cref="MenuCodeAttribute"/>, it resolves the current user's merged
    /// role-menu flags and throws <see cref="ForbiddenException"/> (403) when the
    /// required action bit is unset. Endpoints without both markers, or marked
    /// <c>[AllowAnonymous]</c>, are ignored. Registered globally.
    /// </summary>
    public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
                return;

            if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
                return;

            var menuAttr = descriptor.ControllerTypeInfo.GetCustomAttribute<MenuCodeAttribute>(
                inherit: true
            );
            var permAttr = descriptor.MethodInfo.GetCustomAttribute<RequirePermissionAttribute>(
                inherit: true
            );
            var customEventAttr =
                descriptor.MethodInfo.GetCustomAttribute<RequireCustomEventAttribute>(
                    inherit: true
                );

            // Not a permission-scoped endpoint — leave to [JwtAuthorize].
            if (menuAttr is null || (permAttr is null && customEventAttr is null))
                return;

            var currentUser =
                context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var userId = currentUser.UserId;

            // Unauthenticated — let the JWT filter produce the 401.
            if (userId is null)
                return;

            var roleMenuRepository =
                context.HttpContext.RequestServices.GetRequiredService<IRoleMenuRepository>();
            var permission = await roleMenuRepository.GetUserPermissionAsync(
                userId.Value,
                menuAttr.Code,
                currentUser.ActiveRoleId
            );

            if (permAttr is not null)
            {
                var allowed = permAttr.Action switch
                {
                    PermissionAction.View => permission.CanView,
                    PermissionAction.Create => permission.CanCreate,
                    PermissionAction.Update => permission.CanUpdate,
                    PermissionAction.Delete => permission.CanDelete,
                    PermissionAction.Verify => permission.CanVerify,
                    _ => false,
                };

                // A declared custom event can stand in for the CRUD flag (e.g. the
                // submission "delete-any" grant substituting for Delete).
                if (
                    !allowed
                    && permAttr.OrCustomEvent is not null
                    && permission.CustomEvents.Contains(
                        permAttr.OrCustomEvent,
                        StringComparer.OrdinalIgnoreCase
                    )
                )
                    allowed = true;

                if (!allowed)
                    throw new ForbiddenException("Anda tidak memiliki izin untuk aksi ini");
            }

            if (
                customEventAttr is not null
                && !permission.CustomEvents.Contains(
                    customEventAttr.Code,
                    StringComparer.OrdinalIgnoreCase
                )
            )
                throw new ForbiddenException("Anda tidak memiliki izin untuk aksi ini");
        }
    }
}
