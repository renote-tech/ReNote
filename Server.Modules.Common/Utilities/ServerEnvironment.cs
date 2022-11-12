namespace Server.Common.Utilities
{
    public class ServerEnvironment
    {
        /// <summary>
        /// True if the build configuration is debug; otherwise false for release.
        /// </summary>
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

        /// <summary>
        /// The root directory of the application.
        /// </summary>
        public static readonly string ApplicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// The server version.
        /// </summary>
        public static readonly string ServerVersion = $"{(IsDebug ? "Dev" : "Release")}-0.0.0";
        /// <summary>
        /// The server version name.
        /// </summary>
        public static readonly string ServerVersionName = "NightOny";
        /// <summary>
        /// The server API version.
        /// </summary>
        public static readonly string ServerApiVersion = "v1";

        /// <summary>
        /// Detects whether the environment is running Windows or Unix.
        /// </summary>
        /// <returns><see cref="ServerOS"/></returns>
        public static ServerOS DetectOS()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return ServerOS.WINDOWS;

            if (Environment.OSVersion.Platform == PlatformID.Other)
                return ServerOS.OSX;

            return ServerOS.UNIX;
        }

        /// <summary>
        /// Returns the server agent.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string Agent()
        {
            return $"Omega-{ServerVersionName}/{(IsDebug ? "Dev" : "Release")}";
        }
    }

    public enum ServerOS
    {
        UNIX    = 0,
        WINDOWS = 1,
        OSX     = 2
    }
}