using FluentValidation.Results;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Domain.Service
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(long userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<IList<User>> GetManyAsync();
        Task<bool> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(long userId);
        Task SetBadPasswordCount(long userId, int badPwdCount);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}
