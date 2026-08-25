using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    /// <summary>
    /// The panel surface of a translated content module: ordinary CRUD, plus the
    /// one thing every editorial list needs — an order that can be changed.
    ///
    /// Reordering is its own endpoint rather than a series of updates: it is one
    /// decision, and sending it as one write keeps the list from passing through
    /// states where two entries share a position.
    /// </summary>
    public abstract class BaseTranslatableCrudController<
        TService,
        TEntity,
        TDTO,
        TParam,
        TPaginationDTO,
        TFilterParam
    > : BaseCrudController<TService, TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TService : ITranslatableCrudService<TEntity, TDTO, TParam, TPaginationDTO, TFilterParam>
        where TEntity : class, IBaseEntity
        where TDTO : class
        where TParam : class
        where TPaginationDTO : class
        where TFilterParam : BasePaginationParam
    {
        protected BaseTranslatableCrudController(TService service)
            : base(service) { }

        [HttpPut("reorder")]
        [JwtAuthorize]
        [RequirePermission(PermissionAction.Update)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reorder([FromBody] BaseRequest<ReorderParam> request)
        {
            await _service.ReorderAsync(request.Data);
            return Ok(BaseResponse<object>.Ok(null!, "Urutan diperbarui"));
        }
    }
}
