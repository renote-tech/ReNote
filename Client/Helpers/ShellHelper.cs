namespace Client.Helpers;

using Client.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal class ShellHelper
{
    public static void OpenUrl(string url)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(url) { UseShellExecute = true };
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Process.Start(startInfo);
        else
            Platform.Log($"Couldn't open url ({url})", LogLevel.WARN);
    }
}