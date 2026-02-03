using AutoMapper;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public class AuthTokenModelBuilder : IAuthTokenModelBuilder
    {
        private readonly IMapper _mapper;

        public AuthTokenModelBuilder (IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new view model instance that represents the specified authentication token.
        /// </summary>
        /// <param name="authToken">The authentication token to convert to a view model. Cannot be null.</param>
        /// <returns>An <see cref="AuthTokenViewModel"/> that represents the provided authentication token.</returns>
        public AuthTokenViewModel Build(AuthToken authToken)
        {
            return _mapper.Map<AuthTokenViewModel>(authToken);            
        }
    }
}
