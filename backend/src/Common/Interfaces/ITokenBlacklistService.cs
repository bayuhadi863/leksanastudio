namespace LeksanaStudio.Common.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task BlacklistAsync(string jti, TimeSpan expiry);
        Task<bool> IsBlacklistedAsync(string jti);
    }
}
