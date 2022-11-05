using System;

namespace Client.Utilities
{
    public class ConsoleUtil
    {
        public static void Debug(string message)
            => CWrite($"{DateTime.Now} | [<$Green>Debug</>]  | {message}");

        public static void Info(string message) =>
            CWrite($"{DateTime.Now} | [<$DarkGreen>Info</>]   | {message}");

        public static void Warn(string message) =>
            CWrite($"{DateTime.Now} | [<$Yellow>Warn</>]   | {message}");

        public static void Error(string message) =>
            CWrite($"{DateTime.Now} | [<$Red>Error</>]  | {message}");

        public static void Fatal(string message) =>
            CWrite($"{DateTime.Now} | [<$DarkRed>Fatal</>]  | {message}");

        public static void CWrite(string message, bool nextLine = true)
        {
            if (!Platform.IsDebug)
                return;

            string[] arguments = message.Split('<', '>');
            ConsoleColor color;
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i].StartsWith("/"))
                    Console.ResetColor();
                else if (arguments[i].StartsWith("$") && Enum.TryParse(arguments[i].Substring(1), out color))
                    Console.ForegroundColor = color;
                else if (arguments[i].StartsWith("#") && Enum.TryParse(arguments[i].Substring(1), out color))
                    Console.BackgroundColor = color;
                else
                    Console.Write(arguments[i]);
            }
            if (nextLine)
                Console.Write("\n");
        }
    }

    public enum LogLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }
}
