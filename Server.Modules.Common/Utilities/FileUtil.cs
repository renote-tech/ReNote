namespace Server.Common.Utilities
{
    public class FileUtil
    {
        public static bool IsFileBeingUsed(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
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