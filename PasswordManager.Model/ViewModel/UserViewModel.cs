namespace PasswordManager.Model.ViewModel
{
    public class UserViewModel : AuditFieldViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string AuthenticationSalt { get; set; } = null!;
        public string EncryptionSalt { get; set; } = null!;
        public bool IsActive { get; set; }
        public int BadPwdCount { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
