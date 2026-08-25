using Mapster;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Media;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Constants;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;

namespace LeksanaStudio.Application.Services
{
    public class MediaService
        : BaseCrudService<
            Domain.Entities.Media,
            MediaDTO,
            MediaParam,
            MediaPaginationDTO,
            MediaPaginationParam
        >,
            IMediaService
    {
        private readonly IFileService _fileService;

        public MediaService(
            IBaseRepository<Domain.Entities.Media> repository,
            IFileService fileService,
            ICurrentUserService currentUserService
        )
            : base(repository, currentUserService)
        {
            _fileService = fileService;
        }

        protected override IQueryable<Domain.Entities.Media> ApplyFilter(
            IQueryable<Domain.Entities.Media> query,
            MediaPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                query = query.Where(m =>
                    EF.Functions.ILike(m.OriginalName, $"%{param.Search}%")
                    || (m.Label != null && EF.Functions.ILike(m.Label, $"%{param.Search}%"))
                );
            }

            return query;
        }

        public async Task<MediaDTO> UploadAsync(IFormFile file, MediaUploadParam param)
        {
            // FileService owns the size and content-type rules, so an upload that
            // reaches storage has already been judged acceptable.
            var objectPath = await _fileService.UploadFileAsync(file, "content");

            var media = new Domain.Entities.Media
            {
                ObjectPath = objectPath,
                Mime = file.ContentType,
                SizeBytes = file.Length,
                Width = param.Width,
                Height = param.Height,
                OriginalName = Path.GetFileName(file.FileName),
                Label = string.IsNullOrWhiteSpace(param.Label) ? null : param.Label.Trim(),
                CreatedDate = DateTimeOffset.UtcNow,
                CreatedBy = _currentUserService.UserName ?? AuditConstants.SystemUser,
            };

            var created = await _repository.CreateAsync(media);
            return created.Adapt<MediaDTO>();
        }
    }
}
