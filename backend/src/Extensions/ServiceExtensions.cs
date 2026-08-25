using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Application.Seeders;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Export;
using LeksanaStudio.Common.Import;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>Auto-registers application/domain services by the <c>*Service</c> naming
        /// convention (paired with their <c>I*Service</c> interface). New services are picked
        /// up automatically. Services with a non-scoped lifetime are registered explicitly.</summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AsMatchingInterface (FooService -> IFooService) avoids collisions on shared,
            // non-generic base interfaces (e.g. IVerifiableSubmissionService implemented by
            // several submission services). Types without a matching interface are skipped.
            services.Scan(scan =>
                scan.FromAssemblyOf<AppDbContext>()
                    .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")), publicOnly: false)
                    .AsMatchingInterface()
                    .WithScopedLifetime()
            );

            // Stateless helpers — singletons.
            services.AddSingleton<ITableExporter, TableExporter>();
            services.AddSingleton<ITableImporter, TableImporter>();

            // The block gate. Stateless, and it builds an HTML sanitiser once —
            // which is exactly the kind of thing that should not be rebuilt per
            // request.
            services.AddSingleton<IBlockDocumentProcessor, BlockDocumentProcessor>();

            return services;
        }

        /// <summary>Auto-registers all <see cref="ISeeder"/> implementations. Execution order is
        /// controlled by <see cref="ISeeder.Order"/>, not registration order.</summary>
        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblyOf<AppDbContext>()
                    .AddClasses(c => c.AssignableTo<ISeeder>(), publicOnly: false)
                    .As<ISeeder>()
                    .WithScopedLifetime()
            );

            services.AddScoped<DatabaseSeeder>();

            return services;
        }
    }
}
