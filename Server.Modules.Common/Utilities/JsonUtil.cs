using System.Text.Json;

namespace Server.Common.Utilities
{
    public class JsonUtil
    {
        public static bool ValiditateJson(string content)
        {
            if (string.IsNullOrEmpty(content))
                return false;

            try
            {
                JsonDocument.Parse(content);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}