using System;

namespace Server.Common.Encryption
{
    public class Base64
    {
        /// <summary>
        /// Encodes a <see cref="byte"/>[].
        /// </summary>
        /// <param name="data">The <see cref="byte"/>[] to be encoded.</param>
        /// <returns><see cref="string"/></returns>
        public static string Encode(byte[] data, bool usePadding = true)
        {
            if (data == null)
                return string.Empty;

            string base64 = Convert.ToBase64String(data);

            if(usePadding)
                return base64;

            return base64.Replace("=", "")
                         .Replace("/", "_")
                         .Replace("+", "-");
        }

        /// <summary>
        /// Decodes a <see cref="Base64"/> string.
        /// </summary>
        /// <param name="content">The <see cref="Base64"/> string to be decoded.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static byte[] Decode(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            bool hasPadding = content.Length % 4 != 0;

            if (hasPadding)
                return Convert.FromBase64String(content);

            content = content.PadRight(content.Length + (4 - content.Length % 4) % 4, '=')
                             .Replace("_", "/")
                             .Replace("-", "+");

            return Convert.FromBase64String(content);
        }
    }
}