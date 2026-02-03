using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Domain.Service
{
    public interface IAuthTokenService
    {
        Task<ICollection<AuthToken>> GetManyByUserIdAsync(int userId);
        Task<AuthToken?> GetByTokenAsync(string token);
        Task<AuthTokenViewModel> GenerateJwtTokenAsync(User user, string deviceInfo);
        Task<AuthTokenViewModel?> RefreshAsync(string refreshToken, string deviceInfo);
        Task<bool> RevokeAsync(string token);
    }
}
