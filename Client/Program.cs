using Avalonia;
using Client.Utilities;

namespace Client
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
           InitializeProcessEvents();

           AppBuilder.Configure<App>()
                     .UsePlatformDetect()
                     .LogToTrace()
                     .StartWithClassicDesktopLifetime(args);
        }

        static void InitializeProcessEvents()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                Platform.Log($"{e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
            };
        }

        // DEVELOPMENT ONLY
        public AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                     .UsePlatformDetect()
                     .LogToTrace();
        }
    }
}