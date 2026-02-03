using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public interface IAuthTokenModelBuilder
    {
        AuthTokenViewModel Build(AuthToken authToken);
    }
}
