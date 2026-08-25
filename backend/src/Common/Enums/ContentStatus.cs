namespace LeksanaStudio.Common.Enums
{
    /// <summary>
    /// Publication state of one translation — not of the entry it belongs to.
    ///
    /// Per-translation on purpose: an entry may be live in Indonesian while its
    /// English version is still being written. Anything coarser forces a choice
    /// between publishing a half-translated page and publishing nothing.
    /// </summary>
    public enum ContentStatus
    {
        Draft = 0,
        Published = 1,
    }
}
