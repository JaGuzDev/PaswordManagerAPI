using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure.Data;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure
{
    public class EntryRepository : Repository<Entry>, IEntryRepository
    {
        private readonly AppDbContext _dbContext;
        public EntryRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously retrieves a password entry by its unique identifier.
        /// </summary>
        /// <remarks>The returned password entry includes related user information for the creator and
        /// last updater. This method does not throw an exception if the entry is not found.</remarks>
        /// <param name="entryId">The unique identifier of the password entry to retrieve.</param>
        /// <returns>A <see cref="Entry"/> object if an entry with the specified identifier exists; otherwise, <see
        /// langword="null"/>.</returns>
        public async Task<Entry?> GetOneByIdAsync(long entryId)
        {
            return await _dbContext.Entries
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == entryId);
        }

        /// <summary>
        /// Asynchronously retrieves all entries created by the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose created entries are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entries created by the
        /// specified user, or <c>null</c> if no entries are found.</returns>
        public async Task<IList<Entry>?> GetByUserIdAsync(long userId)
        {
            var entries = await _dbContext.Entries
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Where(pe => pe.CreatedById == userId)
                .ToListAsync();

            return entries;
        }
    }
}
