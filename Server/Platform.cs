using System.Reflection;
using System.Runtime.CompilerServices;
using Server.Common.Utilities;

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
            Console.WriteLine($"Omega-{ServerEnvironment.ServerVersionName} [Version {ServerEnvironment.ServerVersion}]");
            Console.WriteLine("(c) ReNote NETW. All rights reserved.");
            Console.WriteLine();

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