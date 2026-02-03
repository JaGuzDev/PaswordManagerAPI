using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public interface IEntryModelBuilder
    {
        EntryViewModel Build(Entry passwordEntry);
        Entry Build(EntryViewModel passwordEntryViewModel);
        ICollection<EntryViewModel> Build(ICollection<Entry> passwordEntries);
    }
}
