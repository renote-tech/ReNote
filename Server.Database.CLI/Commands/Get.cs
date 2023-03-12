using Server.Database.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Utilities;

namespace Server.Database.Commands
{
    internal class Get
    {
        public static string Execute(string[] args)
        {
            if(!CommandUtil.IsExpectedLength(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            DatabasePath path = DatabasePath.Parse(args[0]);
            if (path == null)
                return CommandMessages.InvalidDatabasePath();

            Document document = DatabaseUtil.Get(path.Root, path.Document);

            if (document == null)
                return CommandMessages.DocumentNotFound();

            return document.GetRaw();
        }
    }
}