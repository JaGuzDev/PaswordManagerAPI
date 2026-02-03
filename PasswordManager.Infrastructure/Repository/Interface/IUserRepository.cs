using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }
}
