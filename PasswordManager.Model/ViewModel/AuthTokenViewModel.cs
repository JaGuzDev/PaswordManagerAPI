namespace PasswordManager.Model.ViewModel
{
    public class AuthTokenViewModel
    {
        public long Id { get; set; }        
        public string Token { get; set; } = null!;
        public string DeviceInfo { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiresAt { get; set; }

        //public DateTime? RevokedAt { get; set; }
        //public long UserId { get; set; }
    }
}
