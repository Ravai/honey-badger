/* 
 * Hashing class to hash and verify passwords.
 */

using System;
using System.Text;
using System.Security.Cryptography;

namespace PasswordHash
{
	// PBKDF2 stands for Password-Based Key Derivation 2
    public class PasswordHash
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        //public const int PBKDF2_ITERATIONS = 1000;

        //public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 0;
        public const int PBKDF2_INDEX = 1;
              
        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The salt/hash of the password.</returns>
        public static string CreateHash(string password)
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, HASH_BYTE_SIZE);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            // Extract the parameters from the hash
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, hash.Length);
            return isEquals(hash, testHash);
        }
        /*
        /// <summary>
        /// Splits the string password, extracts salt and password.
        /// </summary>
        /// <param name="password">salt/password combo to split.</param>
        /// <returns>Array with salt in the first position and pw in the second.</returns>
        public static string[] splitHash(string password)
        {
            string[] split;
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
        }*/

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool isEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            return pbkdf2.GetBytes(outputBytes);
        }
    }
} 
