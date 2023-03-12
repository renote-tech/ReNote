using System.Text.RegularExpressions;

namespace Server.Database.Utilities
{
    internal class CommandUtil
    {
        public static bool IsExpectedLength(string[] arguments, int amount) => arguments.Length >= amount;
        public static string[] NormalizeParams(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                return Array.Empty<string>();

            parameters = parameters.Trim();
            string[] args = Regex.Matches(parameters, @"[\""].+?[\""]|['].+?[']|[^ ]+").Cast<Match>().Select(x => x.Value.Trim('"'))
                                                                                                     .Select(y => y.Trim('\''))
                                                                                                     .ToArray();
            return args;
        }
        public static string NormalizeName(string command)
        {
            command = command.ToLower();
            char[] commandChars = command.ToCharArray();
            commandChars[0] = char.ToUpper(commandChars[0]);
            return new string(commandChars);
        }
    }
}