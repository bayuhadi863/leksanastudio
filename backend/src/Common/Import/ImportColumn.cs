namespace LeksanaStudio.Common.Import
{
    /// <summary>
    /// Describes one column of an import template: the header the user fills under,
    /// human guidance, and (optionally) a fixed set of allowed values rendered as an
    /// in-cell Excel dropdown (used for master-data columns like Skema / Jenis).
    /// </summary>
    public sealed class ImportColumn
    {
        public required string Header { get; init; }
        public string? Description { get; init; }
        public string? Example { get; init; }
        public bool Required { get; init; }

        /// <summary>When set, the data cells get an Excel list validation (dropdown).</summary>
        public IReadOnlyList<string>? AllowedValues { get; init; }
    }
}
