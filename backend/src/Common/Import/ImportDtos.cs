namespace LeksanaStudio.Common.Import
{
    // ---------- Validate (dry-run) response ----------

    public sealed class ImportPreviewDTO
    {
        public ImportSummaryDTO Summary { get; set; } = new();
        public List<ImportPreviewRowDTO> Rows { get; set; } = new();

        /// <summary>Distinct master names present in the file that don't exist yet.</summary>
        public List<ImportNewMasterDTO> NewMasters { get; set; } = new();
    }

    public sealed class ImportSummaryDTO
    {
        public int Total { get; set; }
        public int ToInsert { get; set; }
        public int ToUpdate { get; set; }
        public int Invalid { get; set; }
    }

    public sealed class ImportPreviewRowDTO
    {
        public int RowNumber { get; set; }
        public Dictionary<string, string?> Values { get; set; } = new();

        /// <summary>"insert" | "update" | "invalid".</summary>
        public string Action { get; set; } = "insert";

        /// <summary>Id of the record this row would update (when Action == "update").</summary>
        public Guid? ExistingId { get; set; }

        public List<string> Errors { get; set; } = new();
        public List<ImportMasterRefDTO> MasterRefs { get; set; } = new();
    }

    public sealed class ImportMasterRefDTO
    {
        /// <summary>"scheme" | "publicationType" | "venue".</summary>
        public string Kind { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        /// <summary>True when this master name is not in the database yet.</summary>
        public bool IsNew { get; set; }

        /// <summary>True when the row cannot be saved unless this master exists.</summary>
        public bool Required { get; set; }
    }

    public sealed class ImportNewMasterDTO
    {
        public string Kind { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Required { get; set; }
        public int RowCount { get; set; }
    }

    // ---------- Commit request / response ----------

    public sealed class ImportCommitRequest
    {
        public List<ImportCommitRowDTO> Rows { get; set; } = new();

        /// <summary>Master names the user approved for auto-creation before upsert.</summary>
        public ImportApprovedMastersDTO CreateMasters { get; set; } = new();
    }

    public sealed class ImportCommitRowDTO
    {
        public int RowNumber { get; set; }
        public Dictionary<string, string?> Values { get; set; } = new();
    }

    public sealed class ImportApprovedMastersDTO
    {
        public List<string> Schemes { get; set; } = new();
        public List<string> PublicationTypes { get; set; } = new();
        public List<string> Venues { get; set; } = new();
    }

    public sealed class ImportCommitResultDTO
    {
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int Skipped { get; set; }
        public int MastersCreated { get; set; }
        public List<ImportRowErrorDTO> Errors { get; set; } = new();
    }

    public sealed class ImportRowErrorDTO
    {
        public int RowNumber { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
