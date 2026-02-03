using FluentValidation;
using PasswordManager.Infrastructure;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(5, 50).WithMessage("Username must be between 5 and 50 characters.")
                .MustAsync(async (user, username, cancellation) =>
                {                    
                    var existingUser = await _userRepository.GetByUsernameAsync(username);
                    return existingUser == null || existingUser.Id == user.Id;
                }).WithMessage("Username already exists.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MustAsync(async (user, email, cancellation) =>
                {
                    var existingUser = await _userRepository.GetByEmailAsync(email);
                    return existingUser == null || existingUser.Id == user.Id;
                }).WithMessage("Email address already exists.");

            RuleFor(u => u.FirstName)
               .NotEmpty().WithMessage("Full Name is required.")
               .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("Password is required.");
            _userRepository = userRepository;
        }
    }
}
