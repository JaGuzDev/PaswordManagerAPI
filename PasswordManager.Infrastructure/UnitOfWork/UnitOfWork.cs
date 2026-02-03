using Microsoft.EntityFrameworkCore.Storage;
using PasswordManager.Infrastructure.Data;

namespace PasswordManager.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        public UnitOfWork(
            AppDbContext context, 
            IUserRepository userRepository,
            IAuthTokenRepository authTokenRepository,
            IEntryRepository passwordEntryRepository)
        {
            _context = context;
            UserRepository = userRepository;
            AuthTokenRepository = authTokenRepository;
            EntryRepository = passwordEntryRepository;
        }

        public IUserRepository UserRepository { get; }
        public IAuthTokenRepository AuthTokenRepository { get; }
        public IEntryRepository EntryRepository { get; }


        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<bool> CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                return true;
            }
            return false;
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
