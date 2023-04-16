using System;
using Avalonia;

namespace Client
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
           InitializeProcessEvents();

            Platform.Log("Starting client", LogLevel.INFO);

            AppBuilder.Configure<App>()
                      .UsePlatformDetect()
                      .StartWithClassicDesktopLifetime(args);
        }

        static void InitializeProcessEvents()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                Platform.Log($"Caused by {e.Exception.Source} | {e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Exception exception = (Exception)e.ExceptionObject;
                Platform.Log($"{exception.Message}\n{exception.StackTrace}\n", LogLevel.FATAL);
            };

            Platform.Log("Initialized Process Events", LogLevel.INFO);
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
}