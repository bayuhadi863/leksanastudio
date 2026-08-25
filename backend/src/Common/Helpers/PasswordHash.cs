using BC = BCrypt.Net.BCrypt;

namespace LeksanaStudio.Common.Helpers
{
    /// <summary>
    /// The single place BCrypt is used. Centralised so that no code path can store a
    /// password verbatim, and so verification of a malformed stored value fails as
    /// "wrong credentials" instead of throwing.
    /// </summary>
    public static class PasswordHash
    {
        /// <summary>Length of every BCrypt hash, e.g. <c>$2a$11$…</c>.</summary>
        private const int HashLength = 60;

        /// <summary>Hashes a plaintext password for storage.</summary>
        public static string Hash(string plain) => BC.HashPassword(plain);

        /// <summary>
        /// True when <paramref name="value"/> is already a BCrypt hash rather than a
        /// plaintext password. Used to tell repaired rows from legacy ones.
        /// </summary>
        public static bool IsHashed(string? value) =>
            value is { Length: HashLength } && value.StartsWith("$2", StringComparison.Ordinal);

        /// <summary>
        /// Checks a plaintext password against a stored hash.
        /// </summary>
        /// <remarks>
        /// Returns <c>false</c> — never throws — when the stored value is missing or is
        /// not a BCrypt hash. <see cref="BC.Verify"/> raises
        /// <c>SaltParseException</c> on such input, which would surface as a 500 on the
        /// login endpoint instead of the intended "wrong credentials" response.
        /// </remarks>
        public static bool Verify(string? plain, string? storedHash)
        {
            if (string.IsNullOrEmpty(plain) || !IsHashed(storedHash))
                return false;

            try
            {
                return BC.Verify(plain, storedHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
