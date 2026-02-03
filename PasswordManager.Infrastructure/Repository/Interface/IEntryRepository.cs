using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public interface IEntryRepository : IRepository<Entry>
    {
        Task<Entry?> GetOneByIdAsync(long entryId);
        Task<IList<Entry>?> GetByUserIdAsync(long userId);
    }
}
