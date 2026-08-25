using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Domain.Entities.Base
{
    /// <summary>
    /// Base for a content entry whose text lives in sibling rows, one per language.
    ///
    /// What belongs here: values that read the same in every language — numbers,
    /// ordering, file references, relations. A translated string on this class is
    /// exactly the mistake the split exists to prevent.
    /// </summary>
    public abstract class BaseTranslatableEntity<TTranslation> : BaseEntity,
        ITranslatableEntity<TTranslation>
        where TTranslation : class, ITranslationEntity
    {
        public ICollection<TTranslation> Translations { get; set; } = [];
    }
}
