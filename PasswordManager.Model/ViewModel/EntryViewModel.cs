namespace PasswordManager.Model.ViewModel
{
    /// <summary>
    /// Represents the view model for an entry containing credential and related information for display or editing in
    /// the user interface.
    /// </summary>
    /// <remarks>This class includes properties for storing entry details such as title, username, password,
    /// URL, and notes, along with audit fields inherited from <see cref="AuditFieldViewModel"/>. It is typically used
    /// to transfer entry data between the application and presentation layers.</remarks>
    public class EntryViewModel : AuditFieldViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Notes { get; set; }
    }
}
