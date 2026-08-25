namespace LeksanaStudio.Common.Enums
{
    /// <summary>A CRUD action guarded per role-menu by data-driven permission flags.</summary>
    public enum PermissionAction
    {
        View,
        Create,
        Update,
        Delete,

        /// <summary>Custom event, distinct from Update — e.g. verifying a submission.</summary>
        Verify,
    }
}
