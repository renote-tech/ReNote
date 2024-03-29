namespace Client;

using System.Runtime.InteropServices;

internal class ClientInfo
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

    public const string Version = "0.0.0.0";
    public const string VersionName = "Prototype";
    public const string BuildDate = "NOW";
    public const int BuildNumber = 0;

    public static string GetFullVersion()
    {
        string platformName = GetPlatformName();
        string version = Version.Replace(".", "");

#if BETA
        string buildConfig = (IsDebug ? "DEV" : "BETA");
#else
        string buildConfig = (IsDebug ? "DEV" : "PROD");
#endif

        return $"{buildConfig}_{platformName}_{BuildDate}_{BuildNumber}_{version}";
    }

    private static string GetPlatformName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "WIN";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "OSX";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "UNIX";

        return "OTHER";
    }
}
