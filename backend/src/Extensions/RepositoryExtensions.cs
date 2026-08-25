using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Infrastructure.DataContext;
using LeksanaStudio.Infrastructure.Repositories;

namespace LeksanaStudio.Extensions
{
    public static class RepositoryExtensions
    {
        /// <summary>Auto-registers every concrete repository by convention. A new module's
        /// repository is picked up automatically — no manual registration line needed.</summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Open generic first, so translation and join tables need no file at
            // all. The scan below registers the specific repositories afterwards,
            // and a later registration wins — so a module that declares its own
            // still gets it.
            services.AddScoped(typeof(IBaseRepository<>), typeof(GenericRepository<>));

            services.Scan(scan =>
                scan.FromAssemblyOf<AppDbContext>()
                    .AddClasses(c => c.AssignableTo(typeof(IBaseRepository<>)), publicOnly: false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );

            return services;
        }
    }
}
