using Avalonia;

namespace Client
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
           AppBuilder.Configure<App>()
                     .UsePlatformDetect()
                     .LogToTrace()
                     .StartWithClassicDesktopLifetime(args);
        }
    }
}