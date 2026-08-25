using LeksanaStudio.Application.DTOs.PageDocument;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IPageDocumentService
        : ITranslatableCrudService<
            PageDocument,
            PageDocumentDTO,
            PageDocumentParam,
            PageDocumentPaginationDTO,
            PageDocumentPaginationParam
        >
    {
        Task<IEnumerable<PageDocumentPublicDTO>> GetPublicListAsync(string localeCode);

        Task<PageDocumentPublicDTO?> GetPublicBySlugAsync(string localeCode, string slug);

        /// <summary>
        /// One document by the code the site links to. Used where the address is a
        /// content decision but the link in the footer is not.
        /// </summary>
        Task<PageDocumentPublicDTO?> GetPublicByCodeAsync(string localeCode, string pageCode);
    }
}
