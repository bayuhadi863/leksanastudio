namespace LeksanaStudio.Common.Models
{
    /// <summary>
    /// A user's merged (OR across roles) CRUD permission for a single menu code.
    /// </summary>
    public record MenuPermission(
        bool CanView,
        bool CanCreate,
        bool CanUpdate,
        bool CanDelete,
        bool CanVerify,
        IReadOnlyList<string> CustomEvents
    )
    {
        public static readonly MenuPermission None = new(
            false,
            false,
            false,
            false,
            false,
            []
        );
    }
}
