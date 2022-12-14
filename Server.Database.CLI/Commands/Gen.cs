using Server.Database.Utilities;

namespace Server.Database.Commands
{
    internal class Gen
    {
        public static string Execute(string[] args)
        {
            if (!CommandUtil.HasNumberParams(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            string templateScript = "# Auto-generated - ReNote Database CLI Script\n# Use '#' to add comments\n\nSET root->doc TO {\"Key\":\"Value\"}\nGET root->doc\n\nSAV database.dat";
            try
            { File.WriteAllText(args[0], templateScript); }
            catch(Exception)
            { return CommandMessages.InvalidFilePath(); }

            return CommandMessages.Success();
        }
    }
}