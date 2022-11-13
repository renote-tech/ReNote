using Server.ReNote.Utilities;

namespace Server.Database.Utilities
{
    internal class DatabasePath
    {
        public string Root { get; set; }
        public string Document { get; set; }

        public static DatabasePath Parse(string path)
        {
            string checkPath = path.Replace("->", "")
                                   .Trim();

            if (DatabaseUtil.ContainsIllegalCharacters(checkPath))
                return null;

            string[] arguments = path.Split("->");
            if(arguments.Length != 2)
                return null;

            if (string.IsNullOrWhiteSpace(arguments[0]) | string.IsNullOrWhiteSpace(arguments[1]))
                return null;

            return new DatabasePath()
            {
                Root = arguments[0],
                Document = arguments[1],
            };
        }
    }
}