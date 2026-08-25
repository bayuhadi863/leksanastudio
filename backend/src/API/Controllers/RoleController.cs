using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/role")]
    [MenuCode("role")]
    public class RoleController
        : BaseCrudController<
            IRoleService,
            Role,
            RoleDTO,
            RoleParam,
            RolePaginationDTO,
            RolePaginationParam
        >
    {
        public RoleController(IRoleService roleService)
            : base(roleService) { }

        [HttpGet("menus/{roleId}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.View)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<RoleMenuDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleMenus(Guid roleId)
        {
            var result = await _service.GetRoleMenusAsync(roleId);
            return Ok(BaseResponse<IEnumerable<RoleMenuDTO>>.Ok(result));
        }

        [HttpGet("menus-for-assign/{roleId}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.View)]
        [ProducesResponseType(
            typeof(BaseResponse<PaginationResponse<MenuAssignPickerDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetMenusForAssign(
            Guid roleId,
            [FromQuery] MenuAssignPickerParam param
        )
        {
            var result = await _service.GetMenusForAssignAsync(roleId, param);
            return Ok(BaseResponse<PaginationResponse<MenuAssignPickerDTO>>.Ok(result));
        }

        [HttpPut("assign-menus/{roleId}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Update)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignMenus(
            Guid roleId,
            [FromBody] BaseRequest<AssignMenusParam> request
        )
        {
            await _service.AssignMenusAsync(roleId, request.Data);
            return Ok(BaseResponse<object>.Ok(null!, "Menus assigned successfully"));
        }

        [HttpGet("stats/total")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCount()
        {
            var stat = await _service.GetTotalStatAsync();
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }

        [HttpGet("stats/with-access")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithAccessCount()
        {
            var stat = await _service.GetMenuAccessStatAsync(true);
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }

        [HttpGet("stats/without-access")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<StatCountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithoutAccessCount()
        {
            var stat = await _service.GetMenuAccessStatAsync(false);
            return Ok(BaseResponse<StatCountDTO>.Ok(stat));
        }
    }
}
