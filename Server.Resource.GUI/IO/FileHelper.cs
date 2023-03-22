namespace Server.Resource.GUI
{
    internal class FileHelper
    {
        public static ResourceType GetKnownFileType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".txt":
                case ".json":
                case ".xml":
                    return ResourceType.TEXT;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                case ".gif":
                case ".ico":
                    return ResourceType.IMAGE;
                default:
                    return ResourceType.UNKNOWN;
            }
        }
    }
}