namespace LeksanaStudio.Common.Content
{
    /// <summary>
    /// The numbers that keep an edited page inside its design.
    ///
    /// Two of these are not arbitrary: <see cref="MetricsCount"/> comes from
    /// blueprint 05 ("exactly three headline numbers") and
    /// <see cref="MaxNotesPerDocument"/> from blueprint 06b ("at most four margin
    /// notes per page"). Both used to be rules a writer had to remember. Here they
    /// are rules the server enforces.
    ///
    /// Served to the panel and the site through <c>GET /api/v1/content/block-schema</c>
    /// so the two ends cannot drift apart.
    /// </summary>
    public static class BlockLimits
    {
        public const int MaxBlocksPerDocument = 200;
        public const int MaxNestedBlocks = 40;

        public const int RichTextMaxChars = 4000;
        public const int HeadingMaxChars = 90;
        public const int NoteMaxChars = 400;
        public const int CodeMaxChars = 6000;

        public const int DecisionTitleMaxChars = 140;
        public const int DecisionClauseMaxChars = 220;

        public const int FigureAltMinChars = 10;
        public const int FigureAltMaxChars = 220;
        public const int FigureCaptionMaxChars = 220;

        /// <summary>Blueprint 05: a metric block is three numbers, never two or four.</summary>
        public const int MetricsCount = 3;
        public const int MetricValueMaxChars = 20;
        public const int MetricLabelMaxChars = 40;

        /// <summary>Blueprint 06b: past four, the margin note stops being a note.</summary>
        public const int MaxNotesPerDocument = 4;

        public const int TableMaxColumns = 8;
        public const int TableMaxRows = 60;
        public const int TableCellMaxChars = 300;

        public static readonly IReadOnlySet<string> FigureVariants = new HashSet<string>(
            StringComparer.Ordinal
        )
        {
            "system",
            "website",
            "catalog",
        };
    }
}
