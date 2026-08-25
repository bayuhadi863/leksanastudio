namespace LeksanaStudio.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserName { get; }
        Guid? UserId { get; }
        bool IsAuthenticated { get; }

        /// <summary>
        /// Role id from the <c>X-Role-Active</c> header (raw client intent, not yet
        /// validated against the user's assigned roles). Null when the header is
        /// absent or not a GUID — callers then fall back to the user's primary role.
        /// </summary>
        Guid? ActiveRoleId { get; }
    }
}
