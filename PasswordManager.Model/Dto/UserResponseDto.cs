using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Dto
{
    /// <summary>
    /// Represents a data transfer object containing user information for response operations.
    /// </summary>
    /// <remarks>This class includes key user details such as identification, contact information, and status.
    /// It is typically used to return user data from service or API endpoints. Inherits audit fields from <see
    /// cref="AuditFieldViewModel"/> to provide creation and modification metadata.</remarks>
    public class UserResponseDto : AuditFieldViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
