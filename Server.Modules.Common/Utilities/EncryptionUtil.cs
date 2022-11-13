using System.Text;
using System.Security.Cryptography;

namespace Server.Common.Utilities
{
    public class EncryptionUtil
    {
        /// <summary>
        /// Hashifies a <see cref="string"/>.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be hashed.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static byte[] ComputeSha256(string content)
        {
            return SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(content));
        }

        /// <summary>
        /// Hashifies a <see cref="string"/> as Base64.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be hashed.</param>
        /// <returns><see cref="string"/></returns>
        public static string ComputeStringSha256(string content)
        {
            return Convert.ToBase64String(ComputeSha256(content));
        }

        /// <summary>
        /// Returns a randomized salt.
        /// </summary>
        /// <param name="size">The size of the salt.</param>
        /// <returns><see cref="string"/></returns>
        public static string RandomTokenSalt(ushort size = 8)
        {
            if (size < 8)
                size = 8;

            char[] saltChars = @"0123456789ABCDEF&-~#{}[]()|@$*./\!?".ToLower()
                                                                     .ToCharArray();
            string salt = string.Empty;
            for(int i = 0; i < size; i ++)
                salt += saltChars[new Random().Next(0, saltChars.Length - 1)];

            return salt;
        }
    }
}