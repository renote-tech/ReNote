using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Server.Common.Encryption
{
    public class AES
    {

        /// <summary>
        /// Encrypts a <see cref="string"/>.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be encrypted.</param>
        /// <returns><see cref="AESObject"/></returns>
        public static AESObject Encrypt(string content, byte[] iv = default)
        {
            Aes AES = Aes.Create();
            AES.GenerateIV();

            byte[] data = Encoding.ASCII.GetBytes(content);
            byte[] aesKey = SHA256.HashData(data);
            byte[] aesIV = iv == default ? AES.IV : iv;

            ICryptoTransform cryptor = AES.CreateEncryptor(aesKey, aesIV);
            byte[] encryptedBytes = cryptor.TransformFinalBlock(data, 0, data.Length);
            
            cryptor.Dispose();
            return new AESObject(Convert.ToBase64String(encryptedBytes), iv: aesIV, key: aesKey);
        }

        /// <summary>
        /// Decrypts an <see cref="AESObject"/>.
        /// </summary>
        /// <param name="aesObject">The <see cref="AESObject"/> to be decrypted.</param>
        /// <returns><see cref="string"/></returns>
        public static string Decrypt(AESObject aesObject)
        {
            Aes AES = Aes.Create();
            byte[] content = Convert.FromBase64String(aesObject.Data);
            try
            {
                ICryptoTransform cryptor = AES.CreateDecryptor(aesObject.Key, aesObject.IV);
                byte[] decryptedBytes = cryptor.TransformFinalBlock(content, 0, content.Length);
                
                cryptor.Dispose();
                return Encoding.ASCII.GetString(decryptedBytes);
            } catch(CryptographicException)
            {
                Platform.Log("The key is likely to be invalid", LogLevel.WARN);
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns whether the given key is correct for a specified <see cref="AESObject"/>.
        /// </summary>
        /// <param name="content">The raw key content.</param>
        /// <param name="aesObject">The AES object.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool VerifyKey(string content, AESObject aesObject)
        {
            Aes AES = Aes.Create();
            ICryptoTransform cryptor = AES.CreateEncryptor(aesObject.Key, aesObject.IV);

            byte[] data           = Encoding.ASCII.GetBytes(content);
            byte[] encryptedBytes = cryptor.TransformFinalBlock(data, 0, data.Length);
            byte[] contentKey     = Convert.FromBase64String(aesObject.Data);

            // Comparing using the == operator will return false even for two arrays with identical content.
            // The SequenceEqual method will correctly compare the content of the two arrays.
            return encryptedBytes.SequenceEqual(contentKey);
        }
    }

    public class AESObject
    {
        /// <summary>
        /// The cipher string.
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// The initialization vector.
        /// </summary>
        public byte[] IV { get; set; }
        /// <summary>
        /// The decryption key; it shall not be stored within a database or file.
        /// </summary>
        public byte[] Key { get; set; }

        public AESObject(string data, byte[] iv, byte[] key)
        {
            Data = data;
            IV   = iv;
            Key  = key;
        }
    }
}