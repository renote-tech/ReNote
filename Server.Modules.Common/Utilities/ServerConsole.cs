namespace Server.Common.Utilities
{
    public class ServerConsole
    {
        /// <summary>
        /// Outputs a debugging message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void Debug(string message) =>
            Write($"{DateTime.Now} | [<$Green>Debug</>]  | {message}");

        /// <summary>
        /// Outputs an information message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void Info(string message) =>
            Write($"{DateTime.Now} | [<$DarkGreen>Info</>]   | {message}");

        /// <summary>
        /// Outputs a warning message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void Warn(string message) =>
            Write($"{DateTime.Now} | [<$Yellow>Warn</>]   | {message}");

        /// <summary>
        /// Outputs an error message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void Error(string message) =>
            Write($"{DateTime.Now} | [<$Red>Error</>]  | {message}");

        /// <summary>
        /// Outputs a fatal error message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void Fatal(string message) =>
            Write($"{DateTime.Now} | [<$DarkRed>Fatal</>]  | {message}");

        /// <summary>
        /// Outputs a colored message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        /// <param name="addLine">If true; adds a line.</param>
        public static void Write(string message, bool addLine = true)
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

    public enum LogLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }
}