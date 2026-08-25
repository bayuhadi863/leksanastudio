using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.Application.DTOs.AddOn;
using LeksanaStudio.Application.DTOs.CaseStudy;
using LeksanaStudio.Application.DTOs.Locale;
using LeksanaStudio.Application.DTOs.Note;
using LeksanaStudio.Application.DTOs.PageDocument;
using LeksanaStudio.Application.DTOs.PaymentTerm;
using LeksanaStudio.Application.DTOs.ProcessStep;
using LeksanaStudio.Application.DTOs.ProjectPhase;
using LeksanaStudio.Application.DTOs.Service;
using LeksanaStudio.Application.DTOs.ServicePackage;
using LeksanaStudio.Application.DTOs.Vertical;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Controllers
{
    /// <summary>
    /// What the public site reads — and nothing else.
    ///
    /// A separate controller rather than a flag on the panel one, deliberately.
    /// Everything here is anonymous, so everything here must be published: an
    /// anonymous endpoint that can return a draft is a leak that stays invisible
    /// until it has already happened. Keeping the two apart makes that a property
    /// of the file, not of remembering a filter.
    ///
    /// Read at build time, not per visit: the site pre-renders from these
    /// responses, so a visitor never waits on this API and the site survives the
    /// API being down.
    /// </summary>
    [ApiController]
    [Route("api/v1/public")]
    [AllowAnonymous]
    public class PublicContentController : ControllerBase
    {
        private readonly ICaseStudyService _caseStudies;
        private readonly INoteService _notes;
        private readonly IServiceService _services;
        private readonly IVerticalService _verticals;
        private readonly IServicePackageService _packages;
        private readonly IProjectPhaseService _phases;
        private readonly IAddOnService _addOns;
        private readonly IPaymentTermService _paymentTerms;
        private readonly IProcessStepService _processSteps;
        private readonly IPageDocumentService _documents;
        private readonly ILocaleService _locales;

        public PublicContentController(
            ICaseStudyService caseStudies,
            INoteService notes,
            IServiceService services,
            IVerticalService verticals,
            IServicePackageService packages,
            IProjectPhaseService phases,
            IAddOnService addOns,
            IPaymentTermService paymentTerms,
            IProcessStepService processSteps,
            IPageDocumentService documents,
            ILocaleService locales
        )
        {
            _caseStudies = caseStudies;
            _notes = notes;
            _services = services;
            _verticals = verticals;
            _packages = packages;
            _phases = phases;
            _addOns = addOns;
            _paymentTerms = paymentTerms;
            _processSteps = processSteps;
            _documents = documents;
            _locales = locales;
        }

        /// <summary>The languages the site may be served in.</summary>
        [HttpGet("locale")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<LocaleDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocales()
        {
            var result = await _locales.GetActiveAsync();
            return Ok(BaseResponse<IEnumerable<LocaleDTO>>.Ok(result));
        }

        /// <summary>
        /// The block contract: which kinds exist and what limits apply.
        ///
        /// Served rather than duplicated so the editor, the renderer, and the
        /// server cannot drift apart — the one failure mode a hand-written second
        /// copy of these numbers guarantees.
        /// </summary>
        [HttpGet("block-schema")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public IActionResult GetBlockSchema()
        {
            var schema = new
            {
                kinds = BlockKind.All,
                limits = new
                {
                    maxBlocksPerDocument = BlockLimits.MaxBlocksPerDocument,
                    maxNestedBlocks = BlockLimits.MaxNestedBlocks,
                    richTextMaxChars = BlockLimits.RichTextMaxChars,
                    headingMaxChars = BlockLimits.HeadingMaxChars,
                    noteMaxChars = BlockLimits.NoteMaxChars,
                    codeMaxChars = BlockLimits.CodeMaxChars,
                    decisionTitleMaxChars = BlockLimits.DecisionTitleMaxChars,
                    decisionClauseMaxChars = BlockLimits.DecisionClauseMaxChars,
                    figureAltMinChars = BlockLimits.FigureAltMinChars,
                    figureAltMaxChars = BlockLimits.FigureAltMaxChars,
                    figureCaptionMaxChars = BlockLimits.FigureCaptionMaxChars,
                    metricsCount = BlockLimits.MetricsCount,
                    metricValueMaxChars = BlockLimits.MetricValueMaxChars,
                    metricLabelMaxChars = BlockLimits.MetricLabelMaxChars,
                    maxNotesPerDocument = BlockLimits.MaxNotesPerDocument,
                    tableMaxColumns = BlockLimits.TableMaxColumns,
                    tableMaxRows = BlockLimits.TableMaxRows,
                    tableCellMaxChars = BlockLimits.TableCellMaxChars,
                },
                figureVariants = BlockLimits.FigureVariants,
            };

            return Ok(BaseResponse<object>.Ok(schema));
        }

        /* -------------------------------------------------------- case studies */

        [HttpGet("case-study")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<CaseStudyPublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetCaseStudies([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _caseStudies.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<CaseStudyPublicDTO>>.Ok(result));
        }

        [HttpGet("case-study/{slug}")]
        [ProducesResponseType(typeof(BaseResponse<CaseStudyPublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCaseStudy(string slug, [FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _caseStudies.GetPublicBySlugAsync(localeCode, slug);

            return await RespondAsync(
                result,
                () => _caseStudies.ResolveMovedSlugAsync(localeCode, slug),
                "Studi kasus tidak ditemukan"
            );
        }

        /* --------------------------------------------------------------- notes */

        [HttpGet("note")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<NotePublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetNotes([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _notes.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<NotePublicDTO>>.Ok(result));
        }

        [HttpGet("note/{slug}")]
        [ProducesResponseType(typeof(BaseResponse<NotePublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNote(string slug, [FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _notes.GetPublicBySlugAsync(localeCode, slug);

            return await RespondAsync(
                result,
                () => _notes.ResolveMovedSlugAsync(localeCode, slug),
                "Catatan tidak ditemukan"
            );
        }

        /* ------------------------------------------------------------ services */

        [HttpGet("service")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<ServicePublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetServices([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _services.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<ServicePublicDTO>>.Ok(result));
        }

        [HttpGet("service/{slug}")]
        [ProducesResponseType(typeof(BaseResponse<ServicePublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetService(string slug, [FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _services.GetPublicBySlugAsync(localeCode, slug);

            return await RespondAsync(
                result,
                () => _services.ResolveMovedSlugAsync(localeCode, slug),
                "Layanan tidak ditemukan"
            );
        }

        /* ----------------------------------------------------------- verticals */

        [HttpGet("vertical")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<VerticalPublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetVerticals([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _verticals.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<VerticalPublicDTO>>.Ok(result));
        }

        [HttpGet("vertical/{slug}")]
        [ProducesResponseType(typeof(BaseResponse<VerticalPublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVertical(string slug, [FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _verticals.GetPublicBySlugAsync(localeCode, slug);

            return await RespondAsync(
                result,
                () => _verticals.ResolveMovedSlugAsync(localeCode, slug),
                "Halaman industri tidak ditemukan"
            );
        }

        /* ------------------------------------------------------------- pricing */

        /// <summary>
        /// Everything the pricing page renders, in one response.
        ///
        /// One call rather than five: these tables are read together, and a build
        /// that assembles a page from five requests fails in five ways.
        /// </summary>
        [HttpGet("pricing")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPricing([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);

            var payload = new
            {
                packages = await _packages.GetPublicListAsync(localeCode),
                phases = await _phases.GetPublicListAsync(localeCode),
                addOns = await _addOns.GetPublicListAsync(localeCode),
                paymentTerms = await _paymentTerms.GetPublicListAsync(localeCode),
            };

            return Ok(BaseResponse<object>.Ok(payload));
        }

        [HttpGet("process")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<ProcessStepPublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetProcess([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _processSteps.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<ProcessStepPublicDTO>>.Ok(result));
        }

        /* ----------------------------------------------------------- documents */

        [HttpGet("page-document")]
        [ProducesResponseType(
            typeof(BaseResponse<IEnumerable<PageDocumentPublicDTO>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetPageDocuments([FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _documents.GetPublicListAsync(localeCode);
            return Ok(BaseResponse<IEnumerable<PageDocumentPublicDTO>>.Ok(result));
        }

        /// <summary>By page code — how the footer links to these pages.</summary>
        [HttpGet("page-document/code/{pageCode}")]
        [ProducesResponseType(typeof(BaseResponse<PageDocumentPublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPageDocumentByCode(
            string pageCode,
            [FromQuery] string? locale
        )
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _documents.GetPublicByCodeAsync(localeCode, pageCode);

            return result is null
                ? NotFound(BaseResponse<object>.Fail("Halaman tidak ditemukan", "NOT_FOUND"))
                : Ok(BaseResponse<PageDocumentPublicDTO>.Ok(result));
        }

        [HttpGet("page-document/{slug}")]
        [ProducesResponseType(typeof(BaseResponse<PageDocumentPublicDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPageDocument(string slug, [FromQuery] string? locale)
        {
            var localeCode = await ResolveLocaleAsync(locale);
            var result = await _documents.GetPublicBySlugAsync(localeCode, slug);

            return await RespondAsync(
                result,
                () => _documents.ResolveMovedSlugAsync(localeCode, slug),
                "Halaman tidak ditemukan"
            );
        }

        /* ------------------------------------------------------------- helpers */

        /// <summary>
        /// Found, moved, or genuinely gone — in that order.
        ///
        /// The middle case is the one worth the code: an address that changed still
        /// answers, so the site can emit a 301 rather than lose whatever ranking the
        /// old URL had earned.
        /// </summary>
        private async Task<IActionResult> RespondAsync<T>(
            T? result,
            Func<Task<string?>> resolveMoved,
            string notFoundMessage
        )
            where T : class
        {
            if (result is not null)
                return Ok(BaseResponse<T>.Ok(result));

            var moved = await resolveMoved();
            if (moved is not null)
            {
                return Ok(
                    BaseResponse<object>.Ok(
                        new { movedTo = moved },
                        "Alamat halaman telah berpindah",
                        "MOVED_PERMANENTLY"
                    )
                );
            }

            return NotFound(BaseResponse<object>.Fail(notFoundMessage, "NOT_FOUND"));
        }

        private async Task<string> ResolveLocaleAsync(string? requested) =>
            string.IsNullOrWhiteSpace(requested)
                ? await _locales.GetDefaultCodeAsync()
                : requested.Trim().ToLowerInvariant();
    }
}
