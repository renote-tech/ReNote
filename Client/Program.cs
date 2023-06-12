namespace Client;

using Avalonia;
using Client.Logging;
using System;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Platform.Initialize();
        Configuration.LoadAll();

        InitializeProcessEvents();

        AppBuilder.Configure<App>()
                  .UsePlatformDetect()
                  .StartWithClassicDesktopLifetime(args);
    }

    static void InitializeProcessEvents()
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
            Platform.Log($"{e.Exception.Message}\n{e.Exception.StackTrace}", LogLevel.ERROR);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Exception ex = e.ExceptionObject as Exception;
            Platform.Log($"{ex.Message}\n{ex.StackTrace}\n", LogLevel.FATAL);
        };
    }

#if DEBUG
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace();
    }
#endif
}