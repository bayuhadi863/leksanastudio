namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>
    /// An entry whose position on the page is part of the content.
    ///
    /// Ordering is editorial, not incidental: which case study a prospect reads
    /// first is a decision, so it is stored rather than derived from a date.
    /// </summary>
    public interface IHasOrder
    {
        int Order { get; set; }
    }
}
