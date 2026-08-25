namespace LeksanaStudio.API.Attributes
{
    /// <summary>
    /// Requires the current user's active role to be granted a custom event code on
    /// the controller's <see cref="MenuCodeAttribute"/> menu (e.g. "dashboard-system").
    /// Enforced by <c>PermissionAuthorizationFilter</c> alongside CRUD permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RequireCustomEventAttribute : Attribute
    {
        public string Code { get; }

        public RequireCustomEventAttribute(string code)
        {
            Code = code;
        }
    }
}
