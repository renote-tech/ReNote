using Server.Database.Utilities;
using Server.Common.Utilities;
using Server.ReNote.Utilities;

namespace Server.Database.Commands
{
    internal class Set
    {
        public static string Execute(string[] args)
        {
            if (!CommandUtil.IsExpectedLength(args, 3))
                return CommandMessages.InvalidParamsLength(3);

            DatabasePath path = DatabasePath.Parse(args[0]);
            if (path == null)
                return CommandMessages.InvalidDatabasePath();

            if (!(args[1].ToUpper().Equals("TO") || args[1].Equals("=")))
                return CommandMessages.SetOperatorExpected();

            string data = string.Empty;
            for (int i = 2; i < args.Length; i++)
                data += $"{args[i]}{(i != args.Length ? "" : " ")}";

            if(!JsonUtil.ValiditateJson(data))
                return CommandMessages.InvalidJsonSyntax();

            DatabaseUtil.Set(path.Root, path.Document, data);
            return CommandMessages.Success();
        }
    }
}