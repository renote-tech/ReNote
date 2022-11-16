namespace Server.Common
{
    public class ServerEnv
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
        public static readonly string Version = $"{(IsDebug ? "Dev" : "Release")}-0.7.25";
        /// <summary>
        /// The server version name.
        /// </summary>
        public const string VersionName = "ProtoServer";
        /// <summary>
        /// The server API version.
        /// </summary>
        public const string ApiVersion = "v1";

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
            return $"{VersionName}/{(IsDebug ? "Dev" : "Release")}";
        }
    }

    public enum ServerOS
    {
        UNIX    = 0,
        WINDOWS = 1,
        OSX     = 2
    }
}