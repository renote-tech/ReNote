using Server.Common.Utilities;
using System.Reflection;

namespace Server
{
    public class Platform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;

            Console.Title = "ReNote \u03a9 2023";
            Console.Write("      ____       _   __      __     \r\n     / __ \\___  / | / /___  / /____ \r\n    / /_/ / _ \\/  |/ / __ \\/ __/ _ \\\r\n   / _, _/  __/ /|  / /_/ / /_/  __/\r\n  /_/ |_|\\___/_/ |_/\\____/\\__/\\___/ \r\n                                    \r\n");

            for(int i = 0; i < AppDomain.CurrentDomain.GetAssemblies().Length; i++)
            {
                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()[i];
                Log($"Loaded {assembly.GetName().Name}");
            }

            Log("Initializing server components");

            initialized = true;
        }

        public static void Log(string message, LogLevel level = LogLevel.DEBUG)
        {
            if (!ServerEnvironment.IsDebug && level == LogLevel.DEBUG)
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