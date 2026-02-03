using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.Dto;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public interface IUserModelBuilder
    {        
        UserResponseDto Build(User user);
        IList<UserResponseDto> Build(IList<User> users);
        User Build(UserViewModel userViewModel);
        User Build(UserCreateDto userCreateDto);
    }
}
