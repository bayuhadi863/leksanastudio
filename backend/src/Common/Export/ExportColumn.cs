namespace LeksanaStudio.Common.Export
{
    /// <summary>A single export column: a header label and how to read its value from a row.</summary>
    public class ExportColumn<T>
    {
        public required string Header { get; init; }
        public required Func<T, string?> Value { get; init; }
    }
}
