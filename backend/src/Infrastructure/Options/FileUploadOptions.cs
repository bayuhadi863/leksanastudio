namespace LeksanaStudio.Infrastructure.Options
{
    /// <summary>Upload policy (size limits + allowed content types) for <c>FileService</c>.
    /// Bound from the <c>FileUpload</c> config section; sensible defaults let it run unconfigured.</summary>
    public class FileUploadOptions
    {
        public const string SectionName = "FileUpload";

        public long MaxImageSizeBytes { get; set; } = 5 * 1024 * 1024;
        public long MaxDocumentSizeBytes { get; set; } = 20 * 1024 * 1024;

        public string[] AllowedImageContentTypes { get; set; } =
            ["image/jpeg", "image/png", "image/gif", "image/webp"];

        public string[] AllowedDocumentContentTypes { get; set; } = ["application/pdf"];

        public string[] AllowedTemplateExtensions { get; set; } = [".pdf", ".docx"];
    }
}
