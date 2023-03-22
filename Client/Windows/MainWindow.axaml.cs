using Avalonia.Controls;
using Client.Layout;

namespace Client.Windows
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;

            InitializeComponent();
            SetContent(new SplashUI());

            Platform.ExecuteArguments();
        }

        public void SetContent(UserControl layout)
        {
            Content = layout;
        }
    }
}