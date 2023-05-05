using System;

namespace Server.Common
{
    public class ServerInfo
    {
        /// <summary>
        /// Returns true if the build configuration is debug; otherwise false for release.
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
        /// The configuration. Returns Release if IsDebug is false, otherwise Dev.
        /// </summary>
        public static readonly string Configuration = $"{(IsDebug ? "Dev" : "Release")}";
        /// <summary>
        /// The server agent.
        /// </summary>
        public static readonly string Agent = $"{VersionName}/{Configuration}";
        /// <summary>
        /// The server version.
        /// </summary>
        public static readonly string Version = "0.0.0.0";
        /// <summary>
        /// The server version name.
        /// </summary>
        public const string VersionName = "SpringBloom";
        /// <summary>
        /// The server build date.
        /// </summary>
        public const string BuildDate = "NOW";
        /// <summary>
        /// The server build number.
        /// </summary>
        public const int BuildNumber = 0;

        /// <summary>
        /// Returns whether the environment is running Windows or Unix.
        /// </summary>
        /// <returns><see cref="ServerOS"/></returns>
        public static ServerOS DetectOS()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return ServerOS.WINDOWS;

            return ServerOS.UNIX;
        }

        /// <summary>
        /// Returns the name of the platform ReNote is running on.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string GetPlatformName()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return "Windows NT";

            return "Unix";
        }
    }

    public enum ServerOS
    {
        UNIX    = 0,
        WINDOWS = 1
    }
}
