namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>
    /// A content entry whose text lives in sibling rows, one per language.
    ///
    /// The entry itself carries only what reads the same in every language:
    /// numbers, ordering, file references, relations. Putting a translated string
    /// here is the mistake this split exists to prevent.
    /// </summary>
    public interface ITranslatableEntity<TTranslation> : IBaseEntity
        where TTranslation : class, ITranslationEntity
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}
