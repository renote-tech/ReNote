namespace Server.Common.Utilities
{
    public class ServerEnvironment
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

        public static readonly string ApplicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string ServerVersion = $"{(IsDebug ? "Dev" : "Release")}-0.0.0";
        public static readonly string ServerVersionName = "NightOny";

        public static readonly string ServerAgent = $"ReNote-Sv/{(IsDebug ? "Dev" : "Release")}";
        public static readonly string ServerApiVersion = "v1";

        public static ServerOS DetectOS()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return ServerOS.WINDOWS;

            return ServerOS.UNIX;
        }

        public static string Agent()
        {
            return $"Omega-{ServerVersionName}/{(IsDebug ? "Dev" : "Release")}";
        }
    }

    public enum ServerOS
    {
        UNIX    = 0,
        WINDOWS = 1
    }
}