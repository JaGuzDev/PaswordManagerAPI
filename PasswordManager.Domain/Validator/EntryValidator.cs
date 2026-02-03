using FluentValidation;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Domain.Validator
{
    public class EntryValidator : AbstractValidator<Entry>
    {
        public EntryValidator() 
        {
            RuleFor(entry => entry.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(entry => entry.Username)
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

            RuleFor(entry => entry.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(entry => entry.Url)
                .MaximumLength(500)
                    .WithMessage("URL cannot exceed 500 characters.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("URL must be a valid absolute URL.");

            RuleFor(entry => entry.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
        }
    }
}
