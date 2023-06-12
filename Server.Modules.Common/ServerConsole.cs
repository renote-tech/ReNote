using System;

namespace Server.Common
{
    public class ServerConsole
    {
        /// <summary>
        /// Locker object. Used to queue Write() calls.
        /// </summary>
        private static readonly object s_Locker = new object();

        /// <summary>
        /// Outputs a logging message with the format 'DATE | LEVEL | MESSAGE'
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The severity level.</param>
        /// <param name="levelColor">The color of the severity level.</param>
        public static void Log(string message, LogLevel level, string levelColor)
        {
            Write($"{DateTime.Now} | <${levelColor}>{level}</> | {message}");
        }

        /// <summary>
        /// Outputs a colored message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        /// <param name="addLine">If true; adds a line.</param>
        public static void Write(string message, bool addLine = true)
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

    public enum LogLevel
    {
        DEBUG = 0,
        INFO  = 1,
        WARN  = 2,
        ERROR = 3,
        FATAL = 4
    }
}