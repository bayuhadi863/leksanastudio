namespace LeksanaStudio.Common.Constants
{
    /// <summary>Custom HTTP header names used across the API.</summary>
    public static class HeaderNames
    {
        /// <summary>
        /// Selects which of the user's assigned roles is active for the request.
        /// Value is the role id (GUID). Access is scoped to this single role
        /// instead of merging all of the user's roles.
        /// </summary>
        public const string ActiveRole = "X-Role-Active";
    }
}
