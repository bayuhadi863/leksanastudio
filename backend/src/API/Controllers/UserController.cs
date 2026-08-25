using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    [MenuCode("user")]
    public class UserController
        : BaseCrudController<
            IUserService,
            User,
            UserDTO,
            UserParam,
            UserPaginationDTO,
            UserPaginationParam
        >
    {
        public UserController(IUserService userService)
            : base(userService) { }

        [HttpGet("roles/{userId}")]
        [JwtAuthorize]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<UserRoleDTO>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var result = await _service.GetUserRolesAsync(userId);
            return Ok(BaseResponse<IEnumerable<UserRoleDTO>>.Ok(result));
        }

        [HttpGet("roles-for-assign/{userId}")]
        [JwtAuthorize]
        [ProducesResponseType(
            typeof(BaseResponse<PaginationResponse<RoleAssignPickerDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetRolesForAssign(
            Guid userId,
            [FromQuery] RoleAssignPickerParam param
        )
        {
            var result = await _service.GetRolesForAssignAsync(userId, param);
            return Ok(BaseResponse<PaginationResponse<RoleAssignPickerDTO>>.Ok(result));
        }

        [HttpPut("assign-roles/{userId}")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRoles(
            Guid userId,
            [FromBody] BaseRequest<AssignRolesParam> request
        )
        {
            await _service.AssignRolesAsync(userId, request.Data);
            return Ok(BaseResponse<object>.Ok(null!, "Roles assigned successfully"));
        }

        [HttpGet("stats/total")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCount()
        {
            var stat = await _service.GetTotalStatAsync();
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }

        [HttpGet("stats/with-role")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithRoleCount()
        {
            var stat = await _service.GetRoleAssignmentStatAsync(true);
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }

        [HttpGet("stats/without-role")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithoutRoleCount()
        {
            var stat = await _service.GetRoleAssignmentStatAsync(false);
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }
    }
}
