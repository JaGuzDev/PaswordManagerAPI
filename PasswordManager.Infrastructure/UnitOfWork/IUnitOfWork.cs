namespace PasswordManager.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IAuthTokenRepository AuthTokenRepository { get; }
        IEntryRepository EntryRepository { get; }

        Task<int> SaveChangesAsync();
        Task RollbackAsync();
        Task BeginTransactionAsync();
        Task<bool> CommitAsync();
    }
}
