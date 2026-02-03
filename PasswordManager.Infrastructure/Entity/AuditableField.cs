namespace PasswordManager.Infrastructure.Entity
{
    public class AuditableField
    {
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public User CreatedBy { get; set; } = null!;
        public User? UpdatedBy { get; set; }
    }
}
