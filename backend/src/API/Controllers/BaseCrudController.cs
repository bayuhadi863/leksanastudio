using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    public abstract class BaseCrudController<
        TService,
        TEntity,
        TDTO,
        TParam,
        TPaginationDTO,
        TFilterParam
    > : ControllerBase
        where TService : IBaseCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, IBaseEntity
        where TDTO : class
        where TParam : class
        where TPaginationDTO : class
        where TFilterParam : BasePaginationParam
    {
        protected readonly TService _service;

        protected BaseCrudController(TService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Create)]
        [ProducesResponseType(typeof(BaseResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Create([FromBody] BaseRequest<TParam> request)
        {
            var id = await _service.CreateAsync(request.Data);
            return CreatedAtAction(nameof(Get), new { id }, BaseResponse<Guid>.Ok(id));
        }

        [HttpPut("update/{id}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Update)]
        [ProducesResponseType(typeof(BaseResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update(
            Guid id,
            [FromBody] BaseRequest<TParam> request
        )
        {
            var updatedId = await _service.UpdateAsync(id, request.Data);
            return Ok(BaseResponse<Guid>.Ok(updatedId));
        }

        [HttpDelete("delete/{id}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Delete)]
        [ProducesResponseType(typeof(BaseResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            var deletedId = await _service.DeleteAsync(id);
            return Ok(BaseResponse<Guid>.Ok(deletedId));
        }

        [HttpGet("get/{id}")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.View)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetAsync(id);
            return Ok(BaseResponse<TDTO>.Ok(result));
        }

        [HttpGet("get/list")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.View)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetList()
        {
            var result = await _service.GetListAsync();
            return Ok(BaseResponse<IEnumerable<TPaginationDTO>>.Ok(result));
        }

        [HttpGet("get/pagination")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.View)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetPagination([FromQuery] TFilterParam param)
        {
            var result = await _service.GetPaginationAsync(param);
            return Ok(BaseResponse<PaginationResponse<TPaginationDTO>>.Ok(result));
        }
    }
}
