namespace Client.Windows;

using Avalonia.Controls;
using Avalonia.Input;
using Client.Dialogs;
using Client.Layouts;
using Client.Logging;
using Client.Managers;

internal partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        InitializeClient();
        InitializeEvents();

        Platform.Log("Initialized MainWindow", LogLevel.INFO);
    }

    public void SetLogonUI()
    {
        Content = new LogonLayout();
    }

    public void SetUserUI()
    {
        Content = new UserLayout();
    }

    public void SetMaintenanceMode()
    {
        if (Content is SplashLayout)
            return;

        Content = new SplashLayout();
    }

    public void AddDialog(Dialog control)
    {
        Layout layout = (Layout)Content;
        if (layout == null)
            return;

        Panel mainPanel = (Panel)layout.Content;
        mainPanel.Children.Add(control);
    }

    private void InitializeClient()
    {
        Instance = this;
        LanguageManager.Initialize();
    }

    private void InitializeEvents()
    {
        Opened += (sender, e) =>
        {
            Platform.Log("Initialized ReNote Client", LogLevel.INFO);

            Content = new SplashLayout();
        };

#if DEBUG
        KeyUp += (sender, e) =>
        {
            if (e.Key == Key.F9)
                new ThemeColorPicker().ShowDialog(this);
        };
    }
#endif
}