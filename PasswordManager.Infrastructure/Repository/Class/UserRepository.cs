using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure.Data;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously retrieves a user entity that matches the specified username.
        /// </summary>
        /// <param name="username">The username to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user entity if found;
        /// otherwise, null.</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Asynchronously retrieves a user entity that matches the specified email address.
        /// </summary>
        /// <remarks>The comparison is case-insensitive. If multiple users share the same email address,
        /// only the first match is returned.</remarks>
        /// <param name="email">The email address to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user entity if found;
        /// otherwise, null.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
