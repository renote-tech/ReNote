using System;
using Client.Utilities;

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
        public static readonly string ClientVersion = $"{(IsDebug ? "Dev" : "Release")}-0.0.207";
        public static readonly string ClientVersionName = "IronAssembly";

        public static void Log(string message, LogLevel level = LogLevel.DEBUG)
        {
            switch (level)
            {
                case LogLevel.DEBUG:
                    ConsoleUtil.Debug(message);
                    break;
                case LogLevel.INFO:
                    ConsoleUtil.Info(message);
                    break;
                case LogLevel.WARN:
                    ConsoleUtil.Warn(message);
                    break;
                case LogLevel.ERROR:
                    ConsoleUtil.Error(message);
                    break;
                case LogLevel.FATAL:
                    ConsoleUtil.Fatal(message);
                    break;
            }
        }
    }
}
