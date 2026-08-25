namespace LeksanaStudio.Common.Content
{
    /// <summary>
    /// The complete set of blocks a body may contain.
    ///
    /// Closed on purpose. Every kind here maps to a component that already exists
    /// in the design system, which is what keeps an edited page looking like the
    /// designed one. Adding a kind is a design decision, so it is a code change.
    /// </summary>
    public static class BlockKind
    {
        public const string RichText = "richText";
        public const string Heading = "heading";
        public const string Decision = "decision";
        public const string Figure = "figure";
        public const string Metrics = "metrics";
        public const string Note = "note";
        public const string CodeBlock = "codeBlock";
        public const string Table = "table";

        public static readonly IReadOnlySet<string> All = new HashSet<string>(
            StringComparer.Ordinal
        )
        {
            RichText,
            Heading,
            Decision,
            Figure,
            Metrics,
            Note,
            CodeBlock,
            Table,
        };
    }
}
