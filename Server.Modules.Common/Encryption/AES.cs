using System.Text;
using System.Security.Cryptography;

namespace Server.Common.Encryption
{
    public class AES
    {

        /// <summary>
        /// Encrypts a <see cref="string"/>.
        /// </summary>
        /// <param name="data">The <see cref="string"/> to be encrypted.</param>
        /// <returns><see cref="AESObject"/></returns>
        public static AESObject Encrypt(string data)
        {
            Aes AES = Aes.Create();
            AES.GenerateIV();

            byte[] content = Encoding.ASCII.GetBytes(data);
            byte[] key = SHA256.Create().ComputeHash(content);
            byte[] iv = AES.IV;

            AES.Key = key;
            
            ICryptoTransform cryptor = AES.CreateEncryptor(key, iv);
            byte[] encryptedBytes = cryptor.TransformFinalBlock(content, 0, content.Length);

            cryptor.Dispose();
            return new AESObject(Convert.ToBase64String(encryptedBytes), iv:iv, key:key);
        }

        /// <summary>
        /// Decrypts an <see cref="AESObject"/>.
        /// </summary>
        /// <param name="aesObject">The <see cref="AESObject"/> to be decrypted.</param>
        /// <returns><see cref="string"/></returns>
        public static string Decrypt(AESObject aesObject)
        {
            Aes AES = Aes.Create();
            byte[] decryptedBytes;
            byte[] content = Convert.FromBase64String(aesObject.Data);

            AES.IV = aesObject.IV;
            AES.Key = aesObject.Key;

            ICryptoTransform cryptor = AES.CreateDecryptor(aesObject.Key, aesObject.IV);
            try
            {
                decryptedBytes = cryptor.TransformFinalBlock(content, 0, content.Length);
            } catch(CryptographicException)
            {
                return string.Empty;
            }

            cryptor.Dispose();
            return Encoding.ASCII.GetString(decryptedBytes);
        }
    }

    public class AESObject
    {
        /// <summary>
        /// The cipher <see cref="string"/>.
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// The initialization vector.
        /// </summary>
        public byte[] IV { get; set; }
        /// <summary>
        /// The decryption key; it shall not be stored in a database or file.
        /// </summary>
        public byte[] Key { get; set; }

        public AESObject()
        {

        }

        public AESObject(string data, byte[] iv, byte[] key)
        {
            Data = data;
            IV = iv;
            Key = key;
        }
    }
}