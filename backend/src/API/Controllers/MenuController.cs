using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/menu")]
    public class MenuController
        : BaseCrudController<
            IMenuService,
            Menu,
            MenuDTO,
            MenuParam,
            MenuPaginationDTO,
            MenuPaginationParam
        >
    {
        public MenuController(IMenuService menuService)
            : base(menuService) { }

        [HttpGet("user-access")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<MenuDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAccessibleMenus()
        {
            var result = await _service.GetUserAccessibleMenusAsync();
            return Ok(BaseResponse<IEnumerable<MenuDTO>>.Ok(result));
        }
    }
}
