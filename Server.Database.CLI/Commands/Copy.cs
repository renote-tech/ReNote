using Server.Database.Utilities;
using Server.ReNote.Utilities;

namespace Server.Database.Commands
{
    internal class Copy
    {
        private static readonly CommandInfo m_CommandInfo = new CommandInfo()
        {
            Name        = "Copy",
            Description = "Copies a document value to another",
            Usage       = "COPY root->doc TO root->doc2"
        };

        public static string Execute(string[] args)
        {
            if (!CommandUtil.IsExpectedLength(args, 3))
                return CommandMessages.InvalidParamsLength(3);

            DatabasePath inPath = DatabasePath.Parse(args[0]);
            if (inPath == null)
                return CommandMessages.InvalidDatabasePath();

            string value = DatabaseUtil.Get(inPath.Root, inPath.Document).GetRaw();
            if (string.IsNullOrWhiteSpace(value))
                return CommandMessages.InvalidDatabasePath();

            if (!(args[1].ToUpper().Equals("TO") || args[1].Equals("=")))
                return CommandMessages.SetOperatorExpected();

            DatabasePath outPath = DatabasePath.Parse(args[2]);
            if (outPath == null)
                return CommandMessages.InvalidDatabasePath();

            DatabaseUtil.Set(outPath.Root, outPath.Document, value);
            return CommandMessages.Success();
        }

        public static CommandInfo GetCommandInfo()
        {
            return m_CommandInfo;
        }
    }
}