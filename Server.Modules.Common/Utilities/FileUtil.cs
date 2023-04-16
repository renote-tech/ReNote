using System.IO;

namespace Server.Common.Utilities
{
    public class FileUtil
    {
        /// <summary>
        /// Returns whether a file is currently in use or not.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsFileBeingUsed(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return false;

            if (!File.Exists(location))
                return false;

            try
            {
                FileInfo file = new FileInfo(location);
                using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Close();
            } catch(IOException ex)
            {
                if(ex is not FileNotFoundException)
                    return true;
            }
            return false;
        }
    }
}