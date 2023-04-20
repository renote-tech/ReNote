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

        public static readonly string Configuration = $"{(IsDebug ? "Dev" : "Release")}";
        public static readonly string Version = $"0.7.57 ({Configuration})";
        public const string VersionName = "SummerHeat";

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