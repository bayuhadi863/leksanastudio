using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>
    /// One language version of a content entry: everything that changes when the
    /// language changes — text, slug, and whether it is live.
    /// </summary>
    public interface ITranslationEntity : IBaseEntity
    {
        /// <summary>Owning entry. The pairing between languages runs through this.</summary>
        Guid ParentId { get; set; }

        string LocaleCode { get; set; }

        /// <summary>
        /// The address, not the identity. Free to change; every change is recorded
        /// so the old address can still answer with a 301.
        /// </summary>
        string? Slug { get; set; }

        ContentStatus Status { get; set; }

        DateTimeOffset? PublishedAt { get; set; }
    }
}
