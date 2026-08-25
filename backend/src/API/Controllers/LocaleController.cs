using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Attributes;
using LeksanaStudio.Application.DTOs.Locale;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    [ApiController]
    [Route("api/v1/locale")]
    [MenuCode("locale")]
    public class LocaleController
        : BaseCrudController<
            ILocaleService,
            Domain.Entities.Locale,
            LocaleDTO,
            LocaleParam,
            LocalePaginationDTO,
            LocalePaginationParam
        >
    {
        public LocaleController(ILocaleService service)
            : base(service) { }

        /// <summary>
        /// The languages the panel offers when editing content. Behind auth but not
        /// behind the locale permission: every content form needs this list, and an
        /// editor who cannot manage languages still has to pick one.
        /// </summary>
        [HttpGet("active")]
        [JwtAuthorize]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<LocaleDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            var result = await _service.GetActiveAsync();
            return Ok(BaseResponse<IEnumerable<LocaleDTO>>.Ok(result));
        }
    }
}
