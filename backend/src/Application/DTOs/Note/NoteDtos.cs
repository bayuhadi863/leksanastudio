using System.Text.Json;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Application.DTOs.Note
{
    /* ------------------------------------------------------------------ panel */

    public class NoteDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public NotePillar Pillar { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<NoteTranslationDTO> Translations { get; set; } = [];
    }

    public class NoteTranslationDTO : TranslationDtoBase
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Body { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class NoteParam : Common.Interfaces.ITranslatableParam<NoteTranslationParam>
    {
        public string? ContentKey { get; set; }
        public NotePillar Pillar { get; set; } = NotePillar.Decision;
        public int Order { get; set; }

        public List<NoteTranslationParam> Translations { get; set; } = [];
    }

    public class NoteTranslationParam : TranslationParamBase
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Body { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class NotePaginationDTO : ContentPaginationDtoBase
    {
        public NotePillar Pillar { get; set; }
    }

    public class NotePaginationParam : ContentPaginationParam
    {
        public NotePillar? Pillar { get; set; }
    }

    /* ----------------------------------------------------------- public site */

    /// <summary>One note, flattened to a single language. Published only.</summary>
    public class NotePublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public NotePillar Pillar { get; set; }
        public int Order { get; set; }

        public string? Title { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Body { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        public Dictionary<string, string> Alternates { get; set; } = [];
    }
}
