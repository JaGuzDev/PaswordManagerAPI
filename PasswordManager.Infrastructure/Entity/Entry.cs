using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.Infrastructure.Entity
{
    public class Entry : AuditableField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }        
        public string Title { get; set; } = null!;
        public string? Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? Url { get; set; }
        public string? Notes { get; set; }
        public string InitializationVector { get; set; } = null!;

        [NotMapped]
        public FluentValidation.Results.ValidationResult ValidationResult { get; set; } = new();
    }
}
