using Server.Database.CLI;
using Server.Database.Utilities;

namespace Server.Database.Commands
{
    internal class Run
    {
        public static string Execute(string[] args)
        {
            if (!CommandUtil.HasNumberParams(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            if (!File.Exists(args[0]))
                return CommandMessages.FileNotFound();

            string[] commands = File.ReadAllLines(args[0]);
            for(int i = 0; i < commands.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(commands[i]))
                    continue;

                if (commands[i].StartsWith("#"))
                    continue;

                if (commands[i].ToLower().Contains($"run {args[0].ToLower()}"))
                    return CommandMessages.StackOverflowDetected(commands[i]);

                string cmd = commands[i].Split(' ')[0];

                string result = Program.ExecuteCommand(commands[i]);
                Console.WriteLine($"[{cmd.ToUpper()}] {result}");
            }

            return CommandMessages.None();
        }
    }
}