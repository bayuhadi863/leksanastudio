namespace LeksanaStudio.Infrastructure.Options
{
    /// <summary>CORS policy config. Bound from the <c>Cors</c> section. When
    /// <see cref="AllowedOrigins"/> is empty the policy allows any origin (dev default).</summary>
    public class CorsOptions
    {
        public const string SectionName = "Cors";

        public string[] AllowedOrigins { get; set; } = [];
    }
}
