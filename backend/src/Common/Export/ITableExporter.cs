namespace LeksanaStudio.Common.Export
{
    public interface ITableExporter
    {
        /// <summary>
        /// Render rows into a downloadable file. Returns the bytes, the HTTP content type,
        /// and a suggested file name (with date + extension).
        /// </summary>
        (byte[] Bytes, string ContentType, string FileName) Generate<T>(
            ExportFormat format,
            IReadOnlyList<T> rows,
            IReadOnlyList<ExportColumn<T>> columns,
            string title,
            string baseFileName
        );
    }
}
