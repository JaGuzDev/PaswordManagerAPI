using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.Infrastructure.Entity
{
    public class User : AuditableField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();

        [NotMapped]
        public FluentValidation.Results.ValidationResult ValidationResult { get; set; } = new();

    }
}
