namespace LeksanaStudio.Common.Constants
{
    /// <summary>Well-known audit actor names stamped on entities when no authenticated
    /// user is present.</summary>
    public static class AuditConstants
    {
        /// <summary>Fallback actor for writes made outside a user request context.</summary>
        public const string SystemUser = "SYSTEM";
    }
}
