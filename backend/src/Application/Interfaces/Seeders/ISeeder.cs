namespace LeksanaStudio.Application.Interfaces.Seeders
{
    public interface ISeeder
    {
        /// <summary>Run order (ascending). Lower runs first; seeders with data
        /// dependencies must declare a higher value than what they depend on.</summary>
        int Order => 0;

        Task SeedAsync();
    }
}
