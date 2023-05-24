namespace Client.Logging;

using System;

internal static class ClientConsole
{
    private static readonly object s_Locker = new object();

    public static void Debug(string message) =>
        Write($"{DateTime.Now} | [<$Green>Debug</>] {message}");

    public static void Info(string message) =>
        Write($"{DateTime.Now} | [<$DarkGreen>Info</>] {message}");

    public static void Warn(string message) =>
        Write($"{DateTime.Now} | [<$Yellow>Warn</>] {message}");

    public static void Error(string message) =>
        Write($"{DateTime.Now} | [<$Red>Error</>] {message}");

    public static void Fatal(string message) =>
        Write($"{DateTime.Now} | [<$DarkRed>Fatal</>] | {message}");

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