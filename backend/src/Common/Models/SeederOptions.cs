namespace LeksanaStudio.Common.Models
{
    /// <summary>
    /// Controls database seeding at startup (config section "Seeder").
    /// </summary>
    public class SeederOptions
    {
        /// <summary>Master switch. When false, no seeders run.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Seeder type names to skip (e.g. "GuideSeeder"). Case-insensitive.
        /// </summary>
        public string[] Disabled { get; set; } = [];

        /// <summary>
        /// Initial users to seed (idempotent by email). Empty by default so no
        /// credentials are hardcoded — configure per environment (e.g. a dev admin in
        /// appsettings.Development.json). Production seeds no user unless configured.
        /// </summary>
        public SeedUser[] Users { get; set; } = [];
    }

    /// <summary>A user to seed at startup, with the role codes to grant it.</summary>
    public class SeedUser
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
    }
}
