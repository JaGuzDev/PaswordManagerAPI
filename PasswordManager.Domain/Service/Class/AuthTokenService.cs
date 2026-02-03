using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Infrastructure.UnitOfWork;
using PasswordManager.Model.Builder;
using PasswordManager.Model.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PasswordManager.Domain.Service
{
    public class AuthTokenService : IAuthTokenService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAuthTokenModelBuilder _authTokenModelBuilder;

        public AuthTokenService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IAuthTokenModelBuilder authTokenModelBuilder)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _authTokenModelBuilder = authTokenModelBuilder;
        }

        /// <summary>
        /// Asynchronously retrieves all authentication tokens associated with the specified user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose authentication tokens are to be retrieved.</param>
        /// <returns>A collection of <see cref="AuthToken"/> objects representing the authentication tokens for the specified
        /// user. The collection will be empty if the user has no tokens.</returns>
        public async Task<ICollection<AuthToken>> GetManyByUserIdAsync(int userId)
        {
            return await _unitOfWork.AuthTokenRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// Asynchronously retrieves an authentication token that matches the specified token value. 
        /// </summary>
        /// <param name="token">The token value to search for. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
        /// cref="AuthToken"/> if found; otherwise, <see langword="null"/>.</returns>
        public async Task<AuthToken?> GetByTokenAsync(string token)
        {
            return await _unitOfWork.AuthTokenRepository.GetByTokenAsync(token);
        }

        /// <summary>
        /// Generates a new JSON Web Token (JWT) for the specified user and device information asynchronously.
        /// </summary>
        /// <remarks>The generated token includes claims for the user's ID, username, and email. The token
        /// is persisted and associated with the provided device information. This method should be called when issuing
        /// a new authentication token for a user session.</remarks>
        /// <param name="user">The user for whom the JWT will be generated. Must not be null.</param>
        /// <param name="deviceInfo">A string containing information about the device requesting the token. Used for associating the token with a
        /// specific device.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthTokenViewModel with the
        /// generated JWT and related token information.</returns>
        public async Task<AuthTokenViewModel> GenerateJwtTokenAsync(User user, string deviceInfo)
        {
            var claims = GetClaims(user);

            var jwtKey = _configuration[Common.Constant.Authentication.Jwt.Key];
            var jwtIssuer = _configuration[Common.Constant.Authentication.Jwt.Issuer];
            var jwtAudience = _configuration[Common.Constant.Authentication.Jwt.Audience];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration[Common.Constant.Authentication.Jwt.Issuer],
                audience: _configuration[Common.Constant.Authentication.Jwt.Audience],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Common.Constant.Authentication.Jwt.ExpireInHours),
                signingCredentials: creds);
            
            var refreshToken = GenerateSecureRefreshToken();
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(Common.Constant.Authentication.Jwt.RefreshExpireInDays);
            
            var authToken = new AuthToken
            {
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresAt = DateTime.UtcNow.AddHours(Common.Constant.Authentication.Jwt.ExpireInHours),
                CreatedAt = DateTime.UtcNow,
                DeviceInfo = deviceInfo,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
            };

            await _unitOfWork.AuthTokenRepository.AddAsync(authToken);

            await _unitOfWork.AuthTokenRepository.DeleteExpiredTokensByDateAsync();

            return _authTokenModelBuilder.Build(authToken);            
        }

        /// <summary>
        /// Asynchronously refreshes the authentication token using the provided refresh token and device information.
        /// </summary>
        /// <remarks>This method validates the refresh token, checks its expiration, and ensures it hasn't been revoked.
        /// If the refresh token is valid, a new access token and refresh token are generated. The old refresh token
        /// can optionally be revoked to make it valid for one-time use only.</remarks>
        /// <param name="refreshToken">The refresh token to validate and use for generating a new access token.</param>
        /// <param name="deviceInfo">A string containing information about the device requesting the token refresh.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthTokenViewModel with the
        /// new JWT and related token information, or null if the refresh token is invalid.</returns>
        public async Task<AuthTokenViewModel?> RefreshAsync(string refreshToken, string deviceInfo)
        {
            var authToken = await _unitOfWork.AuthTokenRepository.GetByRefreshTokenAsync(refreshToken, deviceInfo);
            if (authToken == null || authToken.RefreshTokenExpiresAt < DateTime.UtcNow || authToken.RevokedAt != null)
            {
                // Invalid, expired, or revoked refresh token
                return null;
            }

            // Optionally, revoke the old refresh token (for one-time use)
            authToken.RevokedAt = DateTime.UtcNow;

            // Generate new JWT and refresh token
            var user = authToken.User;
            var claims = GetClaims(user);

            var jwtKey = _configuration[Common.Constant.Authentication.Jwt.Key];
            var jwtIssuer = _configuration[Common.Constant.Authentication.Jwt.Issuer];
            var jwtAudience = _configuration[Common.Constant.Authentication.Jwt.Audience];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Common.Constant.Authentication.Jwt.ExpireInHours),
                signingCredentials: creds);

            var newRefreshToken = GenerateSecureRefreshToken();
            var newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(Common.Constant.Authentication.Jwt.RefreshExpireInDays);

            // Create new AuthToken entry
            var newAuthToken = new AuthToken
            {
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresAt = DateTime.UtcNow.AddHours(Common.Constant.Authentication.Jwt.ExpireInHours),
                CreatedAt = DateTime.UtcNow,
                DeviceInfo = deviceInfo,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = newRefreshTokenExpiresAt
            };

            await _unitOfWork.AuthTokenRepository.AddAsync(newAuthToken);

            return _authTokenModelBuilder.Build(newAuthToken);
        }

        /// <summary>
        /// Attempts to revoke the specified authentication token asynchronously.
        /// </summary>
        /// <param name="token">The authentication token to revoke. Cannot be null or empty.</param>
        /// <returns>true if the token was successfully revoked; otherwise, false. Returns false if the token does not exist or
        /// has already been revoked.</returns>
        public async Task<bool> RevokeAsync(string token)
        {
            var authToken = await _unitOfWork.AuthTokenRepository.GetByTokenAsync(token);
            if (authToken == null || authToken.RevokedAt != null)
                return false;

            authToken.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.AuthTokenRepository.UpdateAsync(authToken);
            return true;
        }

        private static string GenerateSecureRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private Claim[] GetClaims(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };
            return claims;
        }
    }
}
