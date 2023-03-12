using Server.Database.Utilities;
using Server.ReNote.Utilities;

namespace Server.Database.Commands
{
    internal class Mov
    {
        public static string Execute(string[] args)
        {
            string copyStatus = Cop.Execute(args);
            if (copyStatus != CommandMessages.Success())
                return copyStatus;

            DatabasePath path = DatabasePath.Parse(args[0]);
            if (path == null)
                return CommandMessages.InvalidDatabasePath();

            bool result = DatabaseUtil.Remove(path.Root, path.Document);
            if (!result)
                return CommandMessages.CommandPartFailed("delete");

            return CommandMessages.Success();
        }
    }
}