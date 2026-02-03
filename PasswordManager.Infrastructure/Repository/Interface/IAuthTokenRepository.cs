using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public interface IAuthTokenRepository : IRepository<AuthToken>
    {
        Task<ICollection<AuthToken>> GetByUserIdAsync(long userId);
        Task<AuthToken?> GetByTokenAsync(string token);
        Task<AuthToken?> GetByRefreshTokenAsync(string refreshToken, string deviceInfo);
        Task DeleteExpiredTokensByDateAsync();
    }
}
