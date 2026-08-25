namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>
    /// Marks a translation whose long-form body is a block document.
    ///
    /// Only long-form writing carries one — case studies, notes, document pages.
    /// Everything else is typed fields, and giving it blocks would be building a
    /// page builder by accident.
    /// </summary>
    public interface IHasBlockBody
    {
        /// <summary>The block document, stored as JSON.</summary>
        string? Body { get; set; }
    }
}
