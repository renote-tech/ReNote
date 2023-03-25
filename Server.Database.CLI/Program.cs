using System.Reflection;
using Server.Database.Utilities;
using Server.Common;

namespace Server.Database.CLI
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine($"ReNote Database CLI [Version {ServerEnv.Version}]");
            Console.WriteLine("(c) ReNote NETW. All rights reserved.");
            Console.WriteLine();
            while(true)
            {
                Console.Write(">");
                string input = Console.ReadLine()
                                      .Trim();

                string status = ExecuteCommand(input);
                if (status == CommandMessages.NoCommand())
                    continue;

                if (status != CommandMessages.None())
                    Console.WriteLine(status);

                Console.WriteLine();
            }
        }

        public static string ExecuteCommand(string input)
        {
            string[] splitInput = CommandUtil.NormalizeParams(input);

            if (splitInput.Length == 0)
                return CommandMessages.NoCommand();

            string command = splitInput[0];
            string[] arguments = splitInput.Skip(1)
                                           .ToArray();

            if (command.ToLower().Equals("exit"))
                Environment.Exit(0);

            if (command.ToLower().Equals("clear"))
            {
                Console.Clear();
                return CommandMessages.NoCommand();
            }

            if (command.ToLower().Equals("help"))
            {
                EnumerateCommands();
                return CommandMessages.NoCommand();
            }

            string commandName = CommandUtil.NormalizeName(command);
            Type commandType = Type.GetType($"Server.Database.Commands.{commandName}");

            if (commandType == null)
                return CommandMessages.CommandNotFound();

            MethodInfo method = commandType.GetMethod("Execute", BindingFlags.Public | BindingFlags.Static);
            
            return (string)method.Invoke(null, new object[] { arguments });
        }

        private static void EnumerateCommands()
        {
            string message = "COP Copies the content of a document to another\n" +
                   "DEL Deletes a document\n" +
                   "GEN Generates a DB script\n" +
                   "GET Gets the value of a document\n" +
                   "MNT Mounts a database from file\n" +
                   "MOV Moves the content of a document to another\n" +
                   "RUN Runs a DB script\n" +
                   "SAV Writes the database data to a file\n" +
                   "SET Creates a document and sets its value\n";

            Console.WriteLine(message);
        }
    }
}