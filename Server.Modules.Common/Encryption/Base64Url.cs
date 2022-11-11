namespace Server.Common.Encryption
{
    public class Base64Url
    {
        public static string Encode(byte[] data)
        {
            if (data == null)
                return string.Empty;

            return Convert.ToBase64String(data)
                          .Replace("=", "")
                          .Replace("/", "_")
                          .Replace("+", "-");
        }

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