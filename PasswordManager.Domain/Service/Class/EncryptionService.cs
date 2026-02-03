using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Domain.Service.Class
{
    public static class EncryptionService
    {     
        /// <summary>
        /// Generates a new cryptographically secure initialization vector (IV) suitable for use with AES encryption.
        /// </summary>
        /// <remarks>Use the returned IV when encrypting data with AES to ensure each encryption operation
        /// is unique and secure. The IV should be stored or transmitted alongside the encrypted data to allow for
        /// correct decryption.</remarks>
        /// <returns>A byte array containing the generated initialization vector. The array length matches the block size of the
        /// AES algorithm.</returns>
        public static byte[] GenerateInitializationVector()
        {
            using var aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        /// <summary>
        /// Generates a cryptographically secure random salt of the specified size in bytes.
        /// </summary>
        /// <remarks>This method uses a cryptographically secure random number generator to produce the
        /// salt. The returned salt can be used for password hashing or other security-related operations requiring
        /// random data.</remarks>
        /// <param name="size">The length, in bytes, of the salt to generate. Must be a positive integer. The default is 16.</param>
        /// <returns>A byte array containing the generated random salt of the specified size.</returns>
        public static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Encrypts the specified plain text using AES encryption with the provided initialization vector.
        /// </summary>
        /// <remarks>The encryption key is obtained internally and is not supplied by the caller. The
        /// caller must ensure that the initialization vector is securely generated and managed. This method does not
        /// perform authentication; for secure transmission, consider using authenticated encryption.</remarks>
        /// <param name="plainText">The text to be encrypted. Cannot be null.</param>
        /// <param name="initializationVector">A byte array containing the initialization vector to use for AES encryption. Must be the correct length for
        /// the AES algorithm.</param>
        /// <returns>A Base64-encoded string representing the encrypted data.</returns>
        public static string Encrypt(string plainText, byte[] initializationVector)
        {
            var key = GetKey();
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = initializationVector;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Decrypts the specified cipher text using AES encryption with the provided initialization vector.
        /// </summary>
        /// <param name="cipherText">The base64-encoded string representing the encrypted data to decrypt. Cannot be null or empty.</param>
        /// <param name="initializationVector">A byte array containing the initialization vector (IV) used for AES decryption. Must match the IV used
        /// during encryption.</param>
        /// <returns>A string containing the decrypted plain text. Returns an empty string if the cipher text represents no data.</returns>
        public static string Decrypt(string cipherText, byte[] initializationVector)
        {
            var key = GetKey();
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = initializationVector;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Computes a SHA-256 hash of the specified value using the provided salt and PBKDF2 key derivation.
        /// </summary>
        /// <remarks>This method uses PBKDF2 with 100,000 iterations and SHA-256 to derive a 32-byte hash.
        /// The same salt and input value will always produce the same hash. For best security, use a unique, randomly
        /// generated salt for each value.</remarks>
        /// <param name="value">The input string to be hashed. Typically represents a password or sensitive value.</param>
        /// <param name="salt">A base64-encoded salt used in the hashing process. Must be a valid base64 string.</param>
        /// <returns>A base64-encoded string containing the derived hash of the input value.</returns>
        public static string Hash(string value, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var inputPasswordHashBytes = Rfc2898DeriveBytes.Pbkdf2(
                value,
                saltBytes,
                100_000,
                HashAlgorithmName.SHA256,
                32
            );
            return Convert.ToBase64String(inputPasswordHashBytes);
        }

        /// <summary>
        /// Retrieves the encryption key from the machine-level environment variable 'PMEncryptionKey'.
        /// </summary>
        /// <remarks>This method requires that the 'PMEncryptionKey' environment variable is configured at
        /// the machine level prior to invocation. The returned key is intended for use in cryptographic operations and
        /// should be handled securely.</remarks>
        /// <returns>A string containing the encryption key specified in the environment variable.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the 'PMEncryptionKey' environment variable is not set or is empty.</exception>
        private static string GetKey()
        {
            var keyString = Environment.GetEnvironmentVariable("PMEncryptionKey", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("Encryption key is not set in environment variables.");
            return keyString;
        }
    }
}
