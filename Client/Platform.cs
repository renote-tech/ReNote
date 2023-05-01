using System;
using Client.Logging;

namespace Client
{
    public class Platform
    {
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        private static bool s_Initialized;

        public static void Initialize()
        {
            if (s_Initialized)
                return;

#if !DEBUG
            ConsoleWriter.Initialize("./client.log");
#else
            Console.Title = "ReNote \u03a3 2023";
#endif

            string watermark = @"      ____       _   __      __      " + "\n" +
                               @"     / __ \___  / | / /___  / /____  " + "\n" +
                               @"    / /_/ / _ \/  |/ / __ \/ __/ _ \ " + "\n" +
                               @"   / _, _/  __/ /|  / /_/ / /_/  __/ " + "\n" +
                               @"  /_/ |_|\___/_/ |_/\____/\__/\___/  ";

            Console.WriteLine($"{watermark} Version {ClientInfo.Version} ({ClientInfo.Configuration})\n");

            s_Initialized = true;
        }

        public static void Log(string message, LogLevel level = LogLevel.DEBUG)
        {
            switch (level)
            {
                case LogLevel.DEBUG:
                    ClientConsole.Debug(message);
                    break;
                case LogLevel.INFO:
                    ClientConsole.Info(message);
                    break;
                case LogLevel.WARN:
                    ClientConsole.Warn(message);
                    break;
                case LogLevel.ERROR:
                    ClientConsole.Error(message);
                    break;
                case LogLevel.FATAL:
                    ClientConsole.Fatal(message);
                    break;
            }
        }
    }
}