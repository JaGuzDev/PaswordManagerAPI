using FluentValidation.Results;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Domain.Service
{
    public interface IEntryService
    {
        Task<Entry?> GetByIdAsync(long entryId);
        Task<IList<Entry>?> GetManyByUserAsync(long userId);
        Task<bool> CreateAsync(Entry entry);
        Task<bool> UpdateAsync(Entry entry);
        Task<bool> DeleteAsync(long entryId);
    }
}
