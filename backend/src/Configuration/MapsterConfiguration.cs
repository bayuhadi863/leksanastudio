using System.Text.Json;
using Mapster;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Configuration
{
    public static class MapsterConfig
    {
        private static readonly JsonSerializerOptions SerializeOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        public static void Configure()
        {
            // Structured content — repeated lists, metric sets, block documents —
            // lives in jsonb columns as text and travels the API as real JSON.
            // Converting in both directions here means no module has to remember to
            // parse or serialise, and none of them can forget.
            TypeAdapterConfig<string?, JsonElement?>.NewConfig().MapWith(src => SafeDeserialize(src));
            TypeAdapterConfig<JsonElement?, string?>.NewConfig().MapWith(src => SafeSerialize(src));

            // A password must never reach the column straight from a request — it has to
            // be hashed first. Excluding it here means the generic CRUD mapping cannot
            // write it at all; UserService sets it explicitly in OnCreating/OnUpdating.
            // On update this also leaves the stored hash in place when no new password
            // was supplied.
            TypeAdapterConfig<UserParam, User>.NewConfig().Ignore(dest => dest.Password!);
        }

        private static JsonElement? SafeDeserialize(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonSerializer.Deserialize<JsonElement>(json);
            }
            catch
            {
                return null;
            }
        }

        private static string? SafeSerialize(JsonElement? value)
        {
            if (value is null || value.Value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
                return null;

            return JsonSerializer.Serialize(value.Value, SerializeOptions);
        }
    }
}
