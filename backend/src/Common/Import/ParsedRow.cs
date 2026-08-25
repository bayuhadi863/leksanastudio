namespace LeksanaStudio.Common.Import
{
    /// <summary>
    /// The outcome of parsing + validating one raw row against a module's rules.
    /// Produced by each import service and consumed by <c>BaseImportService</c> to
    /// build the preview and to upsert on commit.
    /// </summary>
    public sealed class ParsedRow
    {
        /// <summary>Cleaned input echoed back to the client, keyed by column header.</summary>
        public Dictionary<string, string?> Values { get; set; } = new();

        /// <summary>Hard validation errors. A non-empty list makes the row invalid.</summary>
        public List<string> Errors { get; } = new();

        /// <summary>Master-data references (Skema / Jenis / Venue) this row points at.</summary>
        public List<ImportMasterRefDTO> MasterRefs { get; } = new();

        /// <summary>Upsert match key (e.g. "year|title"); null when the row cannot be keyed.</summary>
        public string? BusinessKey { get; set; }

        /// <summary>Module-specific typed payload used to build/update the entity on commit.</summary>
        public object? Payload { get; set; }
    }
}
