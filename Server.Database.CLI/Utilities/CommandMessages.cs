namespace Server.Database.Utilities
{
    internal class CommandMessages
    {
        public static string NoCommand()
        {
            return "Cannot execute empty command.";
        }

        public static string CommandNotFound()
        {
            return "Couldn't find the specified command.";
        }

        public static string InvalidParamsLength(int numberRequired)
        {
            return $"The command should have at least {numberRequired} parameter{(numberRequired > 1 ? "s" : "")}.";
        }

        public static string InvalidDatabasePath()
        {
            return "The given database path is invalid.";
        }

        public static string SetOperatorExpected()
        {
            return "The SET operator (TO or =) is expected.";
        }

        public static string InvalidJsonSyntax()
        {
            return "JSON syntax is invalid.";
        }

        public static string Success()
        {
            return "Success.";
        }

        public static string DatabaseLoadFailed()
        {
            return "Failed to load database data from a file.";
        }

        public static string DatabaseSaveFailed()
        {
            return "Failed to save database date to a file.";
        }

        public static string FileNotFound()
        {
            return "The specified file couldn't be found.";
        }

        public static string RemoveKeyFailed()
        {
            return "Couldn't remove the specified key.";
        }

        public static string CommandPartFailed(string part)
        {
            return $"Failed to execute the part of the command '{part}'.";
        }

        public static string DocumentNotFound()
        {
            return "The specified document couldn't be found.";
        }

        public static string StackOverflowDetected(string cmd)
        {
            return $"The system found that a StackOverflow Exception will happen if the command '{cmd}' is executed.";
        }

        public static string InvalidFilePath()
        {
            return "The specified file path is invalid.";
        }

        public static string None()
        {
            return string.Empty;
        }
    }
}