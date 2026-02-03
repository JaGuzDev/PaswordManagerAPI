using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure.Data;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public class AuthTokenRepository : Repository<AuthToken>, IAuthTokenRepository
    {
        private readonly AppDbContext _dbContext;
        public AuthTokenRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously retrieves all authentication tokens associated with the specified user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose authentication tokens are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of authentication
        /// tokens for the specified user. If no tokens exist for the user, the collection will be empty.</returns>
        public async Task<ICollection<AuthToken>> GetByUserIdAsync(long userId)
        {
            return await _dbContext.AuthTokens
                .Where(at => at.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves the authentication token entity that matches the specified token value.
        /// </summary>
        /// <param name="token">The token string to search for. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
        /// cref="AuthToken"/> entity if found; otherwise, <see langword="null"/>.</returns>
        public async Task<AuthToken?> GetByTokenAsync(string token)
        {
            return await _dbContext.AuthTokens
                .FirstOrDefaultAsync(at => at.Token == token);
        }

        /// <summary>
        /// Asynchronously retrieves the authentication token associated with the specified refresh token and device
        /// information.
        /// </summary>
        /// <param name="refreshToken">The refresh token used to identify the authentication token. Cannot be null.</param>
        /// <param name="deviceInfo">The device information associated with the authentication token. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
        /// cref="AuthToken"/> if found; otherwise, <see langword="null"/>.</returns>
        public async Task<AuthToken?> GetByRefreshTokenAsync(string refreshToken, string deviceInfo)
        {
            return await _dbContext.AuthTokens
                .FirstOrDefaultAsync(at => at.RefreshToken == refreshToken && at.DeviceInfo == deviceInfo);
        }

        /// <summary>
        /// Asynchronously deletes all authentication tokens that have expired and have not been revoked.
        /// </summary>
        /// <remarks>This method removes tokens from the data store where the expiration date has passed
        /// and the token has not been revoked. The operation is performed asynchronously and commits changes to the
        /// database upon completion.</remarks>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteExpiredTokensByDateAsync()
        {
            var tokensToDelete = await _dbContext.AuthTokens
                .Where(at => at.RevokedAt == null && at.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            _dbContext.AuthTokens.RemoveRange(tokensToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
