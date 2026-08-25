namespace LeksanaStudio.Infrastructure.Options
{
    public class PostgresqlOptions
    {
        public const string SectionName = "PostgresqlSettings";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ConnectionString =>
            $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
    }
}
