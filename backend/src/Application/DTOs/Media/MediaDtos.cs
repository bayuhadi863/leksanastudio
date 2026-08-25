using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Media
{
    public class MediaDTO
    {
        public Guid Id { get; set; }
        public string ObjectPath { get; set; } = string.Empty;
        public string Mime { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string OriginalName { get; set; } = string.Empty;
        public string? Label { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class MediaPaginationDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string ObjectPath { get; set; } = string.Empty;
        public string Mime { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string OriginalName { get; set; } = string.Empty;
        public string? Label { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class MediaPaginationParam : BasePaginationParam { }

    /// <summary>Only the label is editable — the file itself is replaced by uploading a new one.</summary>
    public class MediaParam
    {
        public string? Label { get; set; }
    }

    /// <summary>
    /// Extra facts the browser already knows at upload time.
    ///
    /// Read here rather than by decoding the file on the server: the browser has
    /// the dimensions for free, and adding an image codec to the API to recover
    /// two integers is not a trade worth making.
    /// </summary>
    public class MediaUploadParam
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Label { get; set; }
    }
}
