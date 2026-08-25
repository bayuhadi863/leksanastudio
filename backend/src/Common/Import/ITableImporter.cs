namespace LeksanaStudio.Common.Import
{
    /// <summary>
    /// Low-level Excel helper shared by all module imports: reads uploaded rows and
    /// builds styled `.xlsx` templates (mirror of <c>ITableExporter</c>).
    /// </summary>
    public interface ITableImporter
    {
        IReadOnlyList<ImportRawRow> ReadRows(Stream stream);

        byte[] BuildTemplate(string sheetTitle, IReadOnlyList<ImportColumn> columns);
    }

    /// <summary>Marker contract every per-module import service exposes.</summary>
    public interface IImportService
    {
        Task<(byte[] Bytes, string FileName)> BuildTemplateAsync();
        Task<ImportPreviewDTO> ValidateAsync(Stream file);
        Task<ImportCommitResultDTO> CommitAsync(ImportCommitRequest request);
    }
}
