namespace Server.Common.Utilities
{
    public class FileUtil
    {
        public static bool IsFileBeingUsed(string location)
        {
            try
            {
                FileInfo file = new FileInfo(location);
                using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Close();
            } catch(IOException)
            {
                return true;
            }
            return false;
        }
    }
}
