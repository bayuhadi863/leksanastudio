namespace LeksanaStudio.Common.Models
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; }
        public int AbsoluteRefreshTokenExpirationInDays { get; set; }
    }
}
