namespace Server.Common.Encryption
{
    public class Base64Url
    {
        /// <summary>
        /// Encodes a <see cref="byte"/>[].
        /// </summary>
        /// <param name="data">The <see cref="byte"/>[] to be encoded.</param>
        /// <returns><see cref="string"/></returns>
        public static string Encode(byte[] data)
        {
            if (data == null)
                return string.Empty;

            return Convert.ToBase64String(data)
                          .Replace("=", "")
                          .Replace("/", "_")
                          .Replace("+", "-");
        }

        /// <summary>
        /// Decodes a <see cref="Base64Url"/> <see cref="string"/>.
        /// </summary>
        /// <param name="content">The <see cref="Base64Url"/> string to be decoded.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static byte[] Decode(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            content = content.PadRight(content.Length + (4 - content.Length % 4) % 4, '=')
                             .Replace("_", "/")
                             .Replace("-", "+");

            return Convert.FromBase64String(content);
        }
    }
}