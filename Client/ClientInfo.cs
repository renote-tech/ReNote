using System;

namespace Client
{
    public class ClientInfo
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
        
        public const string Version = $"0.8.27";
        public const string VersionName = "SummerHeat";
        public const string BuildDate = "230511103";
        public const int BuildNumber = 0;

        public static ClientOS DetectOS()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return ClientOS.WINDOWS;

            return ClientOS.UNIX;
        }

        public static string GetPlatformName()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return "Windows NT";

            return "Unix";
        }

        public static string GetFullVersion()
        {
            string platformName = (DetectOS() == ClientOS.WINDOWS ? "WIN" : "UNIX");
            string buildConfig  = (IsDebug ? "DEV" : "PROD");
            string version      = Version.Replace(".", "");

#if METRICS_ANALYSIS
            return $"{buildConfig}_{platformName}_{BuildDate}_{BuildNumber}_{version}_MAB";
#else
            return $"{buildConfig}_{platformName}_{BuildDate}_{BuildNumber}_{version}";
#endif
        }
    }

    public enum ClientOS
    {
        UNIX    = 0,
        WINDOWS = 1
    }
}