using System.Text.Json;

namespace Server.Common.Helpers
{
    public class JsonHelper
    {
        /// <summary>
        /// Returns whether the <see cref="string"/> is a valid json value.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ValiditateJson(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
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