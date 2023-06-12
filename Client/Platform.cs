namespace Client;

using Client.Logging;
using System;

internal class Platform
{
    public static void Initialize()
    {
#if !DEBUG
        ConsoleWriter.Initialize("./client.log");
#endif

        try
        {
            Console.Title = "ReNote \u03a3 2023";
        }
        catch { }

        string watermark = @"      ____       _   __      __      " + "\n" +
                           @"     / __ \___  / | / /___  / /____  " + "\n" +
                           @"    / /_/ / _ \/  |/ / __ \/ __/ _ \ " + "\n" +
                           @"   / _, _/  __/ /|  / /_/ / /_/  __/ " + "\n" +
                           @"  /_/ |_|\___/_/ |_/\____/\__/\___/  ";

        Console.WriteLine($"{watermark} Version {ClientInfo.Version} ({ClientInfo.Configuration})\n");
    }

    public static void Log(string message, LogLevel level = LogLevel.DEBUG)
    {
        switch (level)
        {
            case LogLevel.DEBUG:
                ClientConsole.Log(message, level, "Green");
                break;
            case LogLevel.INFO:
                ClientConsole.Log(message, level, "DarkGreen");
                break;
            case LogLevel.WARN:
                ClientConsole.Log(message, level, "Yellow");
                break;
            case LogLevel.ERROR:
                ClientConsole.Log(message, level, "Red");
                break;
            case LogLevel.FATAL:
                ClientConsole.Log(message, level, "DarkRed");
                break;
        }
    }
}