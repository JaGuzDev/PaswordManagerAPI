namespace PasswordManager.Model.Dto
{
    /// <summary>
    /// DTO for creating a new user. Only includes fields that should be provided by the client.
    /// Sensitive fields like PasswordHash and salts are generated server-side.
    /// </summary>
    public class UserCreateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
