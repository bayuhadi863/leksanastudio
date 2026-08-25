using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LeksanaStudio.Common.Helpers
{
    /// <summary>
    /// Turns a title into an address.
    ///
    /// Deliberately narrow output — lowercase ASCII letters, digits, and single
    /// hyphens. A slug with an accent or a space works until it is pasted into a
    /// chat app, an email, or a shell, and then it does not.
    /// </summary>
    public static partial class SlugHelper
    {
        [GeneratedRegex(@"[^a-z0-9]+")]
        private static partial Regex NonSlugChars();

        public static string Slugify(string? value, int maxLength = 200)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            // Decompose accents, then drop the marks: "é" becomes "e", not "".
            var normalised = value.Normalize(NormalizationForm.FormD);
            var stripped = new StringBuilder(normalised.Length);
            foreach (var character in normalised)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                    stripped.Append(character);
            }

            var slug = NonSlugChars()
                .Replace(stripped.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant(), "-")
                .Trim('-');

            return slug.Length > maxLength ? slug[..maxLength].Trim('-') : slug;
        }

        /// <summary>True when the value is already a well-formed slug.</summary>
        public static bool IsValid(string? value) =>
            !string.IsNullOrWhiteSpace(value) && Slugify(value) == value;
    }
}
