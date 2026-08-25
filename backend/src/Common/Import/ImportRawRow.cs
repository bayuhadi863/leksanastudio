namespace LeksanaStudio.Common.Import
{
    /// <summary>
    /// A single data row read from the uploaded workbook, keyed by column header
    /// (trimmed). Empty cells are stored as null. <see cref="RowNumber"/> is the
    /// 1-based Excel row so validation messages can point the user at the cell.
    /// </summary>
    public sealed class ImportRawRow
    {
        public int RowNumber { get; init; }
        public Dictionary<string, string?> Values { get; init; } = new();
    }
}
