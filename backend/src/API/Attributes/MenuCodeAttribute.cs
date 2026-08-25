namespace LeksanaStudio.API.Attributes
{
    /// <summary>
    /// Declares which menu code a controller's CRUD actions belong to, so the
    /// permission filter can resolve the current user's flags for that menu.
    /// Applied at class level on controllers extending <c>BaseCrudController</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MenuCodeAttribute : Attribute
    {
        public string Code { get; }

        public MenuCodeAttribute(string code)
        {
            Code = code;
        }
    }
}
