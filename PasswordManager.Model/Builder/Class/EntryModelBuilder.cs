using AutoMapper;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Builder
{
    public class EntryModelBuilder : IEntryModelBuilder
    {
        private readonly IMapper _mapper;

        public EntryModelBuilder (IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntryViewModel"/> by mapping the specified <see
        /// cref="Entry"/> object.
        /// </summary>
        /// <param name="passwordEntry">The <see cref="Entry"/> to be mapped to a view model. Cannot be null.</param>
        /// <returns>A <see cref="EntryViewModel"/> representing the mapped data from the specified <paramref
        /// name="passwordEntry"/>.</returns>
        public EntryViewModel Build(Entry passwordEntry)
        {
            return _mapper.Map<EntryViewModel>(passwordEntry);
        }

        /// <summary>
        /// Creates a new <see cref="Entry"/> instance from the specified view model.
        /// </summary>
        /// <param name="passwordEntryViewModel">The view model containing password entry data to be mapped. Cannot be null.</param>
        /// <returns>A <see cref="Entry"/> object populated with data from <paramref name="passwordEntryViewModel"/>.</returns>
        public Entry Build(EntryViewModel passwordEntryViewModel)
        {
            return _mapper.Map<Entry>(passwordEntryViewModel);
        }

        /// <summary>
        /// Creates a list of view models representing the specified password entries.
        /// </summary>
        /// <param name="passwordEntries">The collection of password entries to convert to view models. Cannot be null.</param>
        /// <returns>A list of <see cref="EntryViewModel"/> objects corresponding to the provided password entries.
        /// Returns an empty list if <paramref name="passwordEntries"/> is empty.</returns>
        public ICollection<EntryViewModel> Build(ICollection<Entry> passwordEntries)
        {
            return _mapper.Map<ICollection<EntryViewModel>>(passwordEntries);
        }
    }
}
