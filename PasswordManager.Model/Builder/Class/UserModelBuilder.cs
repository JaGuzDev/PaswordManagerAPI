using AutoMapper;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.Dto;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public class UserModelBuilder : IUserModelBuilder
    {
        private readonly IMapper _mapper;

        public UserModelBuilder (IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new UserViewModel instance by mapping the specified User object.
        /// </summary>
        /// <param name="user">The User entity to be mapped to a UserViewModel. Cannot be null.</param>
        /// <returns>A UserViewModel representing the mapped data from the specified User. Returns null if the mapping fails or
        /// if user is null.</returns>
        public UserResponseDto Build(User user)
        {
            return _mapper.Map<UserResponseDto>(user);
        }

        /// <summary>
        /// Maps a collection of user domain entities to their corresponding response data transfer objects.
        /// </summary>
        /// <param name="users">The list of user entities to be mapped. Cannot be null.</param>
        /// <returns>A list of <see cref="UserResponseDto"/> objects representing the mapped users. The list will be empty if
        /// <paramref name="users"/> is empty.</returns>
        public IList<UserResponseDto> Build(IList<User> users)
        {
            return _mapper.Map<IList<UserResponseDto>>(users);
        }

        /// <summary>
        /// Creates a new <see cref="User"/> instance by mapping the data from the specified <see
        /// cref="UserViewModel"/>.
        /// </summary>
        /// <param name="userViewModel">The view model containing user data to be mapped. Cannot be null.</param>
        /// <returns>A <see cref="User"/> object populated with the data from <paramref name="userViewModel"/>.</returns>
        public User Build(UserViewModel userViewModel)
        {
            return _mapper.Map<User>(userViewModel);
        }

        /// <summary>
        /// Creates a new User entity from the specified user creation data transfer object.
        /// </summary>
        /// <param name="userCreateDto">The data transfer object containing information required to create a new User. Cannot be null.</param>
        /// <returns>A User entity populated with the values from the provided user creation data transfer object.</returns>
        public User Build(UserCreateDto userCreateDto)
        {
            return _mapper.Map<User>(userCreateDto);
        }
    }
}
