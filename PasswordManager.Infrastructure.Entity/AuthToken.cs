using System;

namespace PasswordManager.Infrastructure.Entity
{
    public class AuthToken
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? DeviceInfo { get; set; }

        public User User { get; set; } = null!;
    }
}