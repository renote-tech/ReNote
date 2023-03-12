using Server.Database.Utilities;
using Server.ReNote.Utilities;

namespace Server.Database.Commands
{
    internal class Del
    {
        public static string Execute(string[] args)
        {
            if (!CommandUtil.IsExpectedLength(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            DatabasePath path = DatabasePath.Parse(args[0]);
            if (path == null)
                return CommandMessages.InvalidDatabasePath();

            bool result = DatabaseUtil.Remove(path.Root, path.Document);
            if (!result)
                return CommandMessages.RemoveKeyFailed();

            return CommandMessages.Success();
        }
    }
}