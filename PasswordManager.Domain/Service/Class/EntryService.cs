using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using PasswordManager.Domain.Service.Class;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Infrastructure.UnitOfWork;
using System.Security.Claims;

namespace PasswordManager.Domain.Service
{
    public class EntryService : IEntryService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Entry> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public EntryService(
            IMapper mapper,
            IValidator<Entry> validator,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Asynchronously retrieves an entry by its unique identifier and decrypts it before returning.
        /// </summary>
        /// <remarks>The returned entry is decrypted using its associated initialization vector. If no
        /// entry is found with the specified identifier, the method returns <see langword="null"/>.</remarks>
        /// <param name="entryId">The unique identifier of the entry to retrieve. Must correspond to an existing entry.</param>
        /// <returns>A decrypted <see cref="Entry"/> object if an entry with the specified identifier exists; otherwise, <see
        /// langword="null"/>.</returns>
        public async Task<Entry?> GetByIdAsync(long entryId)
        {
            var entry = await _unitOfWork.EntryRepository.GetOneByIdAsync(entryId);
            if (entry == null)
            {
                return null;
            }            
            entry = Decrypt(entry, Convert.FromBase64String(entry.InitializationVector));
            return entry;
        }

        /// <summary>
        /// Retrieves and decrypts all entries associated with the specified user asynchronously.
        /// </summary>
        /// <remarks>Each entry is decrypted before being returned. If no entries exist for the user, the
        /// method returns null instead of an empty list.</remarks>
        /// <param name="userId">The unique identifier of the user whose entries are to be retrieved.</param>
        /// <returns>A list of decrypted entries for the specified user, or null if no entries are found.</returns>
        public async Task<IList<Entry>?> GetManyByUserAsync(long userId)
        {
            var entries = await _unitOfWork.EntryRepository.GetByUserIdAsync(userId);
            if (entries == null || !entries.Any())
            {
                return null;
            }
            for (int i = 0; i < entries.Count; i++) 
            {
                entries[i] = Decrypt(entries[i], Convert.FromBase64String(entries[i].InitializationVector));
            }
            return entries;
        }

        /// <summary>
        /// Asynchronously creates a new entry after validating and encrypting its data.
        /// </summary>
        /// <remarks>If the entry fails validation, the method returns <see langword="false"/> and does
        /// not persist the entry. The created entry is encrypted before being saved. The method sets the creator's user
        /// ID and the creation timestamp on the entry.</remarks>
        /// <param name="entry">The entry to be created. Must not be null and should contain valid data according to the validator.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the entry
        /// was successfully created; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> CreateAsync(Entry entry)
        {
            var validationResult = await _validator.ValidateAsync(entry);
            entry.ValidationResult = validationResult;
            if (!validationResult.IsValid)
            {
                return false;
            }

            var initializationVector = EncryptionService.GenerateInitializationVector();
            var ivBytes = Convert.ToBase64String(initializationVector);
            entry.InitializationVector = ivBytes;
            entry = Encrypt(entry, initializationVector);          

            var userId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            entry.CreatedById = userId;
            entry.CreatedAt = DateTime.UtcNow;
            
            await _unitOfWork.EntryRepository.AddAsync(entry);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Asynchronously updates the specified entry in the data store after validating and encrypting its contents.
        /// </summary>
        /// <remarks>The method performs validation before updating the entry. If validation fails or the
        /// entry does not exist, the update is not performed and the method returns <see langword="false"/>. The entry
        /// is encrypted prior to being saved. The update operation is executed within a transaction.</remarks>
        /// <param name="entry">The entry to update. Must not be null and must contain valid data as determined by the validator.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update
        /// succeeds; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> UpdateAsync(Entry entry)
        {
            var validationResult = await _validator.ValidateAsync(entry);
            entry.ValidationResult = validationResult;
            if (!validationResult.IsValid)
            {
                return false;
            }

            await _unitOfWork.BeginTransactionAsync();
            var existingEntry = await _unitOfWork.EntryRepository.GetByIdAsync(entry.Id);
            if (existingEntry == null)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }

            _mapper.Map(entry, existingEntry);

            var initializationVector = Convert.FromBase64String(existingEntry.InitializationVector);
            entry = Encrypt(entry, initializationVector);

            var userId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            existingEntry.UpdatedById = userId;
            existingEntry.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.EntryRepository.UpdateAsync(existingEntry);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return true;
        }

        /// <summary>
        /// Attempts to delete the specified entry if it exists and was created by the current user.
        /// </summary>
        /// <remarks>The method only deletes entries that were created by the current user. If the entry
        /// does not exist or was not created by the current user, no action is taken and the method returns <see
        /// langword="false"/>.</remarks>
        /// <param name="entryId">The unique identifier of the entry to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the entry
        /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> DeleteAsync(long entryId)
        {
            var userId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var entry = await _unitOfWork.EntryRepository.GetByIdAsync(entryId);
            if (entry == null || entry.CreatedById != userId)
            {
                return false;
            }
            await _unitOfWork.EntryRepository.DeleteAsync(entry);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Encrypts the sensitive fields of the specified entry using the provided initialization vector.
        /// </summary>
        /// <remarks>This method encrypts only the Username, Password, Url, and Notes fields of the entry.
        /// Other fields are not modified. The same initialization vector is used for all encrypted fields.</remarks>
        /// <param name="entry">The entry whose Username, Password, Url, and Notes fields will be encrypted. Fields with null or empty
        /// values are left unchanged.</param>
        /// <param name="initializationVector">A byte array containing the initialization vector to use for encryption. Must not be null.</param>
        /// <returns>The entry with its sensitive fields encrypted. Fields that were null or empty remain unchanged.</returns>
        private Entry Encrypt(Entry entry, byte[] initializationVector)
        {
            entry.Title = EncryptionService.Encrypt(entry.Title, initializationVector);
            entry.Username = !string.IsNullOrEmpty(entry.Username) ? EncryptionService.Encrypt(entry.Username, initializationVector) : null;
            entry.Password = !string.IsNullOrEmpty(entry.Password) ? EncryptionService.Encrypt(entry.Password, initializationVector) : null;
            entry.Url = !string.IsNullOrEmpty(entry.Url) ? EncryptionService.Encrypt(entry.Url, initializationVector) : null;
            entry.Notes = !string.IsNullOrEmpty(entry.Notes) ? EncryptionService.Encrypt(entry.Notes, initializationVector) : null;
            return entry;
        }

        /// <summary>
        /// Decrypts the sensitive fields of the specified entry using the provided initialization vector.
        /// </summary>
        /// <param name="entry">The entry whose Username, Password, Url, and Notes fields will be decrypted. Fields that are null or empty
        /// will remain null.</param>
        /// <param name="initializationVector">A byte array containing the initialization vector used for decryption. Must not be null.</param>
        /// <returns>The entry with its sensitive fields decrypted. Fields that were null or empty before decryption remain null.</returns>
        private Entry Decrypt(Entry entry, byte[] initializationVector)
        {
            entry.Title = EncryptionService.Decrypt(entry.Title, initializationVector);
            entry.Username = !string.IsNullOrEmpty(entry.Username) ? EncryptionService.Decrypt(entry.Username, initializationVector) : null;
            entry.Password = !string.IsNullOrEmpty(entry.Password) ? EncryptionService.Decrypt(entry.Password, initializationVector) : null;
            entry.Url = !string.IsNullOrEmpty(entry.Url) ? EncryptionService.Decrypt(entry.Url, initializationVector) : null;
            entry.Notes = !string.IsNullOrEmpty(entry.Notes) ? EncryptionService.Decrypt(entry.Notes, initializationVector) : null;
            return entry;
        }
    }
}
