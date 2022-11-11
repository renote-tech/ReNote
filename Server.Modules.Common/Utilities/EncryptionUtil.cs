using System.Text;
using System.Security.Cryptography;

namespace Server.Common.Utilities
{
    public class EncryptionUtil
    {
        public static byte[] ComputeSha256(string content)
        {
            return SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(content));
        }

        public static string ComputeStringSha256(string content)
        {
            return Convert.ToBase64String(ComputeSha256(content));
        }

        public static string RandomTokenSalt(ushort size = 8)
        {
            if (size > 8)
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