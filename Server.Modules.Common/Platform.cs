using Server.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server
{
    public class Platform
    {
        /// <summary>
        /// True if the <see cref="Platform"/> class is initialized; otherwise false.
        /// </summary>
        private static bool initialized;

        /// <summary>
        /// Initializes the <see cref="Platform"/> class.
        /// </summary>
        public static void Initialize()
        {
            if (initialized)
                return;

            Console.Title = "ReNote \u03a9 2023";
            Console.Write($"      ____       _   __      __     \r\n     / __ \\___  / | / /___  / /____ \r\n    / /_/ / _ \\/  |/ / __ \\/ __/ _ \\\r\n   / _, _/  __/ /|  / /_/ / /_/  __/\r\n  /_/ |_|\\___/_/ |_/\\____/\\__/\\___/   Version {ServerEnv.Version}\r\n                                    \r\n");

            Log($"Thanks for choosing ReNote!", LogLevel.INFO);

            initialized = true;
        }

        /// <summary>
        /// Outputs a message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        /// <param name="level">The <see cref="LogLevel"/> of the message.</param>
        public static void Log(string message, LogLevel level = LogLevel.DEBUG)
        {
            if (!ServerEnv.IsDebug && level == LogLevel.DEBUG)
                return;

            switch (level)
            {
                case LogLevel.DEBUG:
                    ServerConsole.Debug(message);
                    break;
                case LogLevel.INFO:
                    ServerConsole.Info(message);
                    break;
                case LogLevel.WARN:
                    ServerConsole.Warn(message);
                    break;
                case LogLevel.ERROR:
                    ServerConsole.Error(message);
                    break;
                case LogLevel.FATAL:
                    ServerConsole.Fatal(message);
                    break;
            }
        }
    }
}