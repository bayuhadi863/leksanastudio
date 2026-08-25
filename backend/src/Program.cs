using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Middlewares;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Configuration;
using LeksanaStudio.Extensions;

MapsterConfig.Configure();

// QuestPDF Community license (free for organisations under USD 1M annual revenue).
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
Configuration.AddEnvironmentVariables();

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorsPolicy(Configuration);
builder.Services.AddSwaggerDocumentation();
builder
    .Services.AddControllers(options =>
        options.Filters.Add<LeksanaStudio.API.Filters.PermissionAuthorizationFilter>()
    )
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context
                .ModelState.Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = BaseResponse<object>.Fail(
                "Validation failed",
                "VALIDATION_ERROR",
                errors
            );
            return new BadRequestObjectResult(response);
        };
    })
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        )
    );

builder.Services.AddPostgresql(Configuration);
builder.Services.AddRepositories();

builder.Services.AddJwtAuthentication(Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddSeeders();
builder.Services.AddRedis(Configuration);
builder.Services.AddMinioStorage(Configuration);
builder.Services.AddValidation();
builder.Services.AddHealthChecksServices(Configuration);

var app = builder.Build();

app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

await app.MigrateDatabaseAsync();
await app.RunStartupChecksAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

// Behind a production reverse proxy TLS is terminated at the edge and the backend
// is reached over plain HTTP on the internal network. Forcing an HTTPS redirect
// there would 307-loop, so only redirect during local development.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.MapControllers();
app.UseHealthChecksEndpoint();

app.Run();
