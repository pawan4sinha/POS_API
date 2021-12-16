using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace POS.Common.Security
{
    public static class Encryption
    {
        private const string _key = "6D114A94-375E-4FFD-9F48-CF2C7A12620F";
        private const string SALT = "E5A8DB1B-5EAB-4C62-B322-CE24CE274303";

        //Bare minimum iteration is 1000. We should set the iteration to the highest possible value our hardware can support. 10000 iterations appears to be acceptable 
        //compromise and is used by companies like Apple for iTunes passwords. 
        //On development laptop it takes 1.3 seconds to Hash a password. We should only reduce iteration count if our hardware is unable support this.
        private const int ITERATIONS = 10000;
        private const int SALT_SIZE = 16;

        /// <summary>
        /// Uses PBKDF2 to hash the password with Salt. 
        /// 
        /// PBKDBF2 is an acceptable algorithim. Once Microsoft implements scrypt or bcrypt we should change this method accordingly. 
        /// 
        /// Microsoft documentation of Rfc2898DeriveBytes is not clear so I checked StackExchange's OpenId provider to ensure my usage of Rfc2898DeriveBytes is correct.
        /// https://code.google.com/p/stackid/source/browse/OpenIdProvider/Current.cs 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string CalculateHash(string password, string salt)
        {
            int bytesLength = 24;

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), Convert.FromBase64String(salt), ITERATIONS))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(bytesLength));
            }
        }

        /// <summary>
        /// Returns a cryptographically secure random number for salt. Using RNGCryptoServiceProvider because it is much less predictable than Random class and is
        /// security standards compliant. 
        /// </summary>
        /// <returns></returns>
        public static string GetSalt()
        {
            byte[] secureRandom = new byte[SALT_SIZE];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(secureRandom);
            }
            return Convert.ToBase64String(secureRandom);
        }

        /// <summary>
        /// Use this only for reversible Encryption, DO NOT USE THIS FOR PASSWORDS
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            byte[] utfdata = UTF8Encoding.UTF8.GetBytes(text);
            byte[] saltBytes = UTF8Encoding.UTF8.GetBytes(SALT);

            // We're using the PBKDF2 standard for password-based key generation
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(_key, saltBytes, 1000);

            // Our symmetric encryption algorithm
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // Encryption
            ICryptoTransform encryptTransf = aes.CreateEncryptor();

            // Output stream, can be also a FileStream
            MemoryStream encryptStream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(encryptStream, encryptTransf, CryptoStreamMode.Write);
            encryptor.Write(utfdata, 0, utfdata.Length);
            encryptor.Flush();
            encryptor.Close();

            // Showing our encrypted content
            byte[] encryptBytes = encryptStream.ToArray();
            string encryptedString = Convert.ToBase64String(encryptBytes);

            // Close stream
            encryptStream.Close();

            //We have usered Base64. It contain charactor A-Z, a-z, 0-9, +, / and = only.
            //All charactors are supported but only +, / and = are not supported by browser.
            //So it needs to be replaced like + => -, / => _ and = => !
            // Return encrypted text
            return encryptedString.Replace("+", "-").Replace("/", "_").Replace("=", "!");
        }

        public static string Decrypt(string text)
        {
            text = text.Replace("-", "+").Replace("_", "/").Replace("!", "=");

            // Get inputs as bytes
            byte[] encryptBytes = Convert.FromBase64String(text);
            byte[] saltBytes = Encoding.UTF8.GetBytes(SALT);

            // We're using the PBKDF2 standard for password-based key generation
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(_key, saltBytes);

            // Our symmetric encryption algorithm
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // Now, decryption
            ICryptoTransform decryptTrans = aes.CreateDecryptor();

            // Output stream, can be also a FileStream
            MemoryStream decryptStream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(decryptStream, decryptTrans, CryptoStreamMode.Write);
            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Flush();
            decryptor.Close();

            // Showing our decrypted content
            byte[] decryptBytes = decryptStream.ToArray();
            string decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

            // Close Stream
            decryptStream.Close();

            // Return decrypted text
            return decryptedString;
        }

        public static List<string> GeneratePasswordAndSalt()
        {
            var random = new Random();
            string tempPassword = Convert.ToString(Guid.NewGuid());

            //Seems like its prefixing random character from GUID to the temp password. 
            tempPassword = tempPassword.Substring(0, 12);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var singleChar = new string(
                Enumerable.Repeat(chars, 1)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            tempPassword = singleChar + tempPassword;

            List<string> itemList = new List<string>();
            itemList.Add(tempPassword.Replace("-", string.Empty));
            itemList.Add(Encryption.GetSalt());

            return itemList;
        }
    }
}
