using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.Extensions
{
    public static class JwtExtensions
    {
        /// <summary>Binds <see cref="JwtOptions"/> and wires JWT bearer authentication.</summary>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var section = configuration.GetSection(JwtOptions.SectionName);
            services.Configure<JwtOptions>(section);
            var jwtOptions = section.Get<JwtOptions>() ?? new JwtOptions();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Secret)
                        ),
                    };
                });

            return services;
        }
    }
}
