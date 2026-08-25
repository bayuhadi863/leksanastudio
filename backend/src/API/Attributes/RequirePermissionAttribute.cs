using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Attributes
{
    /// <summary>
    /// Marks an action with the CRUD permission it requires. Combined with the
    /// controller's <see cref="MenuCodeAttribute"/> by the permission filter.
    /// Inherited so annotations on <c>BaseCrudController</c> apply to derived controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RequirePermissionAttribute : Attribute
    {
        public PermissionAction Action { get; }

        /// <summary>
        /// Optional custom-event code that satisfies the action on its own (OR with
        /// <see cref="Action"/>). Null means only the CRUD flag is accepted.
        /// </summary>
        public string? OrCustomEvent { get; init; }

        public RequirePermissionAttribute(PermissionAction action)
        {
            Action = action;
        }
    }
}
