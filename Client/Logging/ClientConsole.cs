namespace Client.Logging;

using System;

internal static class ClientConsole
{
    private static readonly object s_Locker = new object();

    public static void Log(string message, LogLevel level, string levelColor)
    {
        Write($"{DateTime.Now} | <${levelColor}>{level}</> | {message}");
    }

    private static void Write(string message, bool addLine = true)
    {
        lock (s_Locker)
        {
            string[] arguments = message.Split('<', '>');
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i].StartsWith("/"))
                    Console.ResetColor();
                else if (arguments[i].StartsWith("$") && Enum.TryParse(arguments[i].Substring(1), out ConsoleColor color))
                    Console.ForegroundColor = color;
                else if (arguments[i].StartsWith("#") && Enum.TryParse(arguments[i].Substring(1), out color))
                    Console.BackgroundColor = color;
                else
                    Console.Write(arguments[i]);
            }

            if (addLine)
                Console.Write("\n");
        }
    }
}

internal enum LogLevel
{
    DEBUG = 0,
    INFO  = 1,
    WARN  = 2,
    ERROR = 3,
    FATAL = 4
}