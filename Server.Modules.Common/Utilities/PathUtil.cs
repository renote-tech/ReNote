namespace Server.Common.Utilities
{
    public class PathUtil
    {
        /// <summary>
        /// Normalizes the <see cref="string"/> to the OS environment path style.
        /// </summary>
        /// <param name="path">The path to be normalized.</param>
        /// <returns><see cref="string"/></returns>
        public static string NormalizeToOS(string path)
        {
            if (ServerEnv.DetectOS() == ServerOS.UNIX)
                return path.Replace(@"\\",  "/")
                           .Replace(@"\",   "/")
                           .Replace(@"\\\", "/")
                           .Replace("//",   "/");

            return path.Replace("//",  @"\")
                       .Replace("/",   @"\")
                       .Replace("///", @"\")
                       .Replace(@"\\", @"\");
        }
    }
}