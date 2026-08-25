using System.Text.Json;
using LeksanaStudio.Common.Helpers;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// Rules every content module repeats, written once.
    ///
    /// Not a convenience: a slug rule that exists in eleven copies is a slug rule
    /// that will disagree with itself the first time one of them is tightened.
    /// </summary>
    public static class ContentRules
    {
        public const string SlugMessage =
            "Alamat halaman hanya boleh huruf kecil, angka, dan tanda hubung — "
            + "supaya tetap utuh saat ditempel di WhatsApp.";

        public static bool BeAWellFormedSlug(string? slug) =>
            slug is null || SlugHelper.IsValid(slug);

        /// <summary>One language may appear once. Two rows for `id` is a data bug, not a choice.</summary>
        public static bool OneTranslationPerLocale<T>(List<T> translations)
            where T : ITranslationParam =>
            translations
                .Select(t => t.LocaleCode?.Trim().ToLowerInvariant())
                .Distinct()
                .Count() == translations.Count;

        /// <summary>True when the value is absent, or is a JSON array.</summary>
        public static bool BeAnArrayOrEmpty(JsonElement? value) =>
            value is null
            || value.Value.ValueKind == JsonValueKind.Null
            || value.Value.ValueKind == JsonValueKind.Array;

        /// <summary>True when the value is absent, or is a JSON array of at most <paramref name="max"/> items.</summary>
        public static bool BeAnArrayOfAtMost(JsonElement? value, int max) =>
            BeAnArrayOrEmpty(value)
            && (value is null || value.Value.ValueKind != JsonValueKind.Array || value.Value.GetArrayLength() <= max);

        /// <summary>
        /// True when every item in the array carries all of the named string fields,
        /// non-empty. Used for the small repeaters — FAQ entries, package features —
        /// where a half-filled row would render as a blank line on the page.
        /// </summary>
        public static bool EveryItemHas(JsonElement? value, params string[] fields)
        {
            if (value is null || value.Value.ValueKind != JsonValueKind.Array)
                return true;

            foreach (var item in value.Value.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                    return false;

                foreach (var field in fields)
                {
                    if (
                        !item.TryGetProperty(field, out var property)
                        || property.ValueKind != JsonValueKind.String
                        || string.IsNullOrWhiteSpace(property.GetString())
                    )
                        return false;
                }
            }

            return true;
        }

        /// <summary>True when every item is a non-empty string.</summary>
        public static bool EveryItemIsText(JsonElement? value)
        {
            if (value is null || value.Value.ValueKind != JsonValueKind.Array)
                return true;

            foreach (var item in value.Value.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(item.GetString()))
                    return false;
            }

            return true;
        }
    }
}
