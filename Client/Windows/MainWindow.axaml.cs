using Avalonia.Controls;
using Client.Pages;

namespace Client.Windows
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;

            InitializeComponent();
            Content = new SplashUI();
        }
    }
}