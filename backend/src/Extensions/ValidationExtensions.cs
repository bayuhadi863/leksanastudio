using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using LeksanaStudio.API.Validators;

namespace LeksanaStudio.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            // Register all validators from the assembly
            var assembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(assembly);

            // Dynamically register BaseRequestValidator<T> for every T that has an IValidator<T>
            // This allows BaseRequest<T> to be automatically validated using the T validator.
            var validatorTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(
                    t => t.GetInterfaces(),
                    (t, i) => new { Implementation = t, Interface = i }
                )
                .Where(x =>
                    x.Interface.IsGenericType
                    && x.Interface.GetGenericTypeDefinition() == typeof(IValidator<>)
                );

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.Interface.GetGenericArguments()[0];

                // Skip if the request type is already a BaseRequest (to avoid recursion)
                if (
                    requestType.IsGenericType
                    && requestType.GetGenericTypeDefinition() == typeof(Common.Models.BaseRequest<>)
                )
                    continue;

                var wrappedType = typeof(Common.Models.BaseRequest<>).MakeGenericType(requestType);
                var interfaceType = typeof(IValidator<>).MakeGenericType(wrappedType);
                var implementationType = typeof(BaseRequestValidator<>).MakeGenericType(
                    requestType
                );

                services.AddTransient(interfaceType, implementationType);
            }

            return services;
        }
    }
}
