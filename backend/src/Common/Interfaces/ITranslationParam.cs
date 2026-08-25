using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Common.Interfaces
{
    /// <summary>The fields every incoming translation carries, whatever the module.</summary>
    public interface ITranslationParam
    {
        string LocaleCode { get; set; }
        string? Slug { get; set; }
        ContentStatus Status { get; set; }
    }

    /// <summary>A write request that carries its translations with it.</summary>
    public interface ITranslatableParam<TTranslationParam>
        where TTranslationParam : ITranslationParam
    {
        List<TTranslationParam> Translations { get; set; }
    }
}
