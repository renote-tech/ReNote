namespace Client.Windows;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Client.Dialogs;
using Client.Layouts;
using Client.Logging;
using Client.Managers;
using System.Runtime.InteropServices;

internal partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; }

    public MainWindow()
    {
        Instance = this;

        InitializeComponent();
        InitializeWindow();
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

    public void UnbindDialog(Dialog dialog)
    {
        Layout layout = (Layout)Content;
        if (layout == null)
            return;

        Panel mainPanel = (Panel)layout.Content;
        if (!mainPanel.Children.Contains(dialog))
            return;

        mainPanel.Children.Remove(dialog);
    }

    public void BindDialog(Dialog dialog)
    {
        Layout layout = (Layout)Content;
        if (layout == null)
            return;

        Panel mainPanel = (Panel)layout.Content;
        mainPanel.Children.Add(dialog);
    }

    private void InitializeWindow()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        Size screenSize = Screens.Primary.Bounds.Size.ToSize(1);

        Width = screenSize.Width * 2 / 3;
        Height = screenSize.Height * 2 / 3;
    }

    private void InitializeEvents()
    {
        Opened += (sender, e) =>
        {
            Platform.Log("Initialized ReNote Client", LogLevel.INFO);

            LanguageManager.Initialize();

            Content = new SplashLayout();
        };

#if DEBUG
        KeyUp += (sender, e) =>
        {
            if (e.Key == Key.F9 && ThemeColorPicker.IsClosed)
            {
                new ThemeColorPicker().Show(this);
                return;
            }
        };
#endif
    }
}