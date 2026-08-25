namespace LeksanaStudio.Common.Content
{
    /// <summary>Outcome of checking and cleaning one body document.</summary>
    /// <param name="IsValid">False when the document breaks a rule the panel should have caught.</param>
    /// <param name="Error">Reader-facing reason, in Indonesian. Null when valid.</param>
    /// <param name="Json">
    /// The document as it should be stored: sanitised, normalised, and with block
    /// ids filled in. Null when invalid.
    /// </param>
    public sealed record BlockDocumentResult(bool IsValid, string? Error, string? Json);

    /// <summary>
    /// The gate every body passes through before it is stored.
    ///
    /// It runs on the server, not in the editor, because an editor can be
    /// bypassed and a request cannot. Anything it rejects is a bug in the panel;
    /// anything it strips is something the panel should not have offered.
    /// </summary>
    public interface IBlockDocumentProcessor
    {
        /// <summary>Checks, sanitises, and normalises a body document.</summary>
        BlockDocumentResult Process(string? json);

        /// <summary>Plain text of a body, for search indexes and meta descriptions.</summary>
        string ToPlainText(string? json);
    }
}
