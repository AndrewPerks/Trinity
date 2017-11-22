using System;
using System.Security;
using System.Security.Cryptography;
using Trinity.Helpers;

namespace Trinity.Authentication
{
    /// <summary>
    /// Manages the Password hashing and Salt generation
    /// </summary>
    public class PasswordHashing
    {
        public static byte[] CalculateHash(byte[] inputBytes)
        {
            SHA256Managed algorithm = new SHA256Managed();
            algorithm.ComputeHash(inputBytes);
            return algorithm.Hash;
        }

        public static string GenerateSalt()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int maxLength = 32;

            // Empty salt array
            byte[] salt = new byte[maxLength];

            // Build the random bytes
            random.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt);
        }

        // Refactor to cut code out from authentication view model
        public static byte[] CalculateHashSalt(SecureString securePassword)
        {
            var salt = GenerateSalt();

            foreach (char c in salt)
            {
                securePassword.AppendChar(c);
            }

            return CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(securePassword));
        }

        //public static string CalculateHashSalt(string clearTextPassword, string salt)
        //{
        //    // Convert the salted password to a byte array
        //    byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
        //    // Use the hash algorithm to calculate the hash
        //    HashAlgorithm algorithm = new SHA256Managed();
        //    byte[] hash = algorithm.ComputeHash(saltedHashBytes);
        //    // Return the hash as a base64 encoded string to be compared to the stored password
        //    return Convert.ToBase64String(hash);
        //}

        public static bool SequenceEquals(byte[] originalByteArray, byte[] newByteArray)
        {
            //If either byte array is null, throw an ArgumentNullException
            if (originalByteArray == null || newByteArray == null)
                throw new ArgumentNullException(originalByteArray == null ? "originalByteArray" : "newByteArray",
                                  "The byte arrays supplied may not be null.");

            //If byte arrays are different lengths, return false
            if (originalByteArray.Length != newByteArray.Length)
                return false;

            //If any elements in corresponding positions are not equal
            //return false
            for (int i = 0; i < originalByteArray.Length; i++)
            {
                if (originalByteArray[i] != newByteArray[i])
                    return false;
            }

            //If we've got this far, the byte arrays are equal.
            return true;
        }
    }
}
