using System;
using System.IO;
using System.Security.Cryptography;

namespace SaveMyDataServer.Database.Static
{
    /// <summary>
    /// A static class for hashing and checking passwords
    /// </summary>
    public static class HashHelpers
    {

        #region Properties
        /// <summary>
        /// The itreation on the hash function (100 --> 1000)
        /// </summary>
        public static int HashIteration { get; private set; } = 500;
        #endregion

        #region AES
        /// <summary>
        /// Encrypts a plain text using AES algorithm 
        /// </summary>
        /// <param name="plainText">The plain text to encrypt</param>
        /// <param name="key">The key for encryption</param>
        /// <param name="IV">The initializing vector</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_AES(string plainText, byte[] key, byte[] IV)
        {
            //The encrypted bytes representing the plain text
            byte[] encryptedBytes;
            //Create the instance to encrypt
            using (Aes aes = Aes.Create())
            {
                //Assign the key and the IV
                aes.Key = key;
                aes.IV = IV;

                //Create the encryptor to preform the stream transform
                ICryptoTransform encryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memeoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memeoryStream, encryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encryptedBytes = memeoryStream.ToArray();
                    }
                }
            }

            return encryptedBytes;
        }
        /// <summary>
        /// Decryptes a byte array that was encrypted by AES algorithm
        /// </summary>
        /// <param name="bytes">The encryped byte array</param>
        /// <param name="key">The key that was used in encryption</param>
        /// <param name="IV">The initialize vectore that was used in encryption</param>
        /// <returns></returns>
        public static string DecryptBytesToString_AES(byte[] bytes, byte[] key, byte[] IV)
        {
            //The plain text from the encrypted bytes
            string plainText = "";
            using (Aes aes = Aes.Create())
            {
                //assign the key and IV
                aes.Key = key;
                aes.IV = IV;
                //Create the transfrom to decrypt the data
                ICryptoTransform decryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                //Open a memory stram
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    //Assign a cryptoStram to read from the memoryStream
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            //Read the decrypted text
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
        #endregion

        /// <summary>
        /// Hashes the password with a salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            //Create the salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //Create the Rfc2898DeriveBytes
            var rfc = new Rfc2898DeriveBytes(password, salt, HashIteration);
            //Get the hash password
            byte[] hash = rfc.GetBytes(20);
            //combain the password with its salt
            var hashPassword = new byte[36];
            Array.Copy(salt, 0, hashPassword, 0, 16);
            Array.Copy(hash, 0, hashPassword, 16, 20);
            //return the string password
            return Convert.ToBase64String(hashPassword);
        }
        /// <summary>
        /// Vertifies if the user provided the right passord
        ///     by hashing the  user password and comparing it to the password in the database
        /// </summary>
        /// <param name="dbPassword">The password that is saved in the database</param>
        /// <param name="userPassword">The user password that is provided one logging in</param>
        /// <returns></returns>
        public static bool VertifyPassword(string dbPassword, string userPassword)
        {
            //Get the byte array 
            var dbBytePassword = Convert.FromBase64String(dbPassword);
            //Get the salt
            var salt = new byte[16];
            //Copy to the salt array
            Array.Copy(dbBytePassword, 0, salt, 0, 16);
            //Create the Rfc2898DeriveBytes
            var rfc = new Rfc2898DeriveBytes(userPassword, salt, HashIteration);
            //Get the hash password
            byte[] hash = rfc.GetBytes(20);
            //Compare values
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != dbBytePassword[salt.Length + i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
