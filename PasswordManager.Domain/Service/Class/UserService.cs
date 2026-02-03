using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PasswordManager.Domain.Service.Class;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Infrastructure.UnitOfWork;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PasswordManager.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IMapper mapper,
            IValidator<User> validator,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="User"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public async Task<User?> GetByIdAsync(long userId)
        {            
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        /// <summary>
        /// Asynchronously retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A <see cref="User"/> object representing the user with the specified username, or <see langword="null"/> if
        /// no such user exists.</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _unitOfWork.UserRepository.GetByUsernameAsync(username);
        }

        /// <summary>
        /// Asynchronously retrieves all users from the data store.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="User"/>
        /// objects representing all users. The list will be empty if no users are found.</returns>
        public async Task<IList<User>> GetManyAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        /// <summary>
        /// Asynchronously creates a new user record after validating the specified user entity.
        /// </summary>
        /// <remarks>The method first validates the provided user entity. If validation fails, the user is
        /// not created and the method returns <see langword="false"/>. On success, the user is associated with the
        /// current authenticated user as the creator and the creation timestamp is set. The operation is performed
        /// within the current unit of work context.</remarks>
        /// <param name="user">The user entity to be validated and created. Cannot be null. The entity's properties should be populated
        /// with the required user information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
        /// successfully created; otherwise, <see langword="false"/> if validation failed.</returns>
        public async Task<bool> CreateAsync(User user)
        {
            // Generate random salt values
            var authSalt = EncryptionService.GenerateSalt();            
            var encSalt = EncryptionService.GenerateSalt();
            
            user.AuthenticationSalt = Convert.ToBase64String(authSalt);
            user.EncryptionSalt = Convert.ToBase64String(encSalt);
            user.PasswordHash = EncryptionService.Hash(user.PasswordHash, user.AuthenticationSalt);

            var userId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            user.CreatedById = userId;
            user.CreatedAt = DateTime.UtcNow;

            user.IsActive = true;

            var validationResult = await _validator.ValidateAsync(user);
            user.ValidationResult = validationResult;
            if (!validationResult.IsValid)
            {
                return false;
            }

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Asynchronously updates the specified user in the data store after validating the user information.
        /// </summary>
        /// <remarks>The update operation will fail if the user data is invalid or if the user does not
        /// exist in the data store. Validation is performed before any changes are made.</remarks>
        /// <param name="user">The user entity containing updated information to be validated and persisted. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update
        /// was successful; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> UpdateAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            user.ValidationResult = validationResult;
            if (!validationResult.IsValid)
            {
                return false;
            }

            await _unitOfWork.BeginTransactionAsync();
            var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }

            _mapper.Map(user, existingUser);

            var userId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            existingUser.UpdatedById = userId;
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UserRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return true;
        }

        /// <summary>
        /// Asynchronously deletes the user with the specified identifier from the data store.
        /// </summary>
        /// <remarks>If the specified user does not exist, no changes are made to the data store and the
        /// method returns <see langword="false"/>. The operation is performed within a transaction to ensure data
        /// consistency.</remarks>
        /// <param name="userId">The unique identifier of the user to delete. Must correspond to an existing user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
        /// successfully deleted; otherwise, <see langword="false"/> if no user with the specified identifier exists.</returns>
        public async Task<bool> DeleteAsync(long userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (existingUser == null)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
            await _unitOfWork.UserRepository.DeleteAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return true;
        }


        /// <summary>
        /// Asynchronously sets the number of consecutive failed password attempts for the specified user.
        /// </summary>
        /// <remarks>If the specified user does not exist, no changes are made. This method updates the
        /// user's bad password count and commits the change to the data store.</remarks>
        /// <param name="userId">The unique identifier of the user whose bad password count will be updated.</param>
        /// <param name="badPwdCount">The new count of consecutive failed password attempts to assign to the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when the bad password count has been
        /// updated.</returns>
        public async Task SetBadPasswordCount(long userId, int badPwdCount)
        {
            var accessUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (accessUser == null)
                return;
            accessUser.BadPwdCount = badPwdCount;
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Authenticates a user asynchronously using the specified username and password.
        /// </summary>
        /// <remarks>This method verifies the provided password against the stored password hash for the
        /// user. If authentication fails, <see langword="null"/> is returned. No exceptions are thrown for invalid
        /// credentials.</remarks>
        /// <param name="username">The username of the user to authenticate. Cannot be null or empty.</param>
        /// <param name="password">The password to verify for the specified user. Cannot be null or empty.</param>
        /// <returns>A <see cref="User"/> object representing the authenticated user if the credentials are valid; otherwise,
        /// <see langword="null"/>.</returns>
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;

            var inputPasswordHash = EncryptionService.Hash(password, user.AuthenticationSalt);

            // Compare hashes
            if (!inputPasswordHash.Equals(user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}
