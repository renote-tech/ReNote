namespace Server.Common.Utilities
{
    public class PathUtil
    {
        public static string NormalizeToOS(string path)
        {
            if (ServerEnvironment.DetectOS() == ServerOS.UNIX)
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