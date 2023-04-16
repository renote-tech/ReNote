using Avalonia;
using Avalonia.Controls;
using Client.Layouts;
using Client.Managers;

namespace Client.Windows
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            InitializeClient();
        }

        private void InitializeClient()
        {
            Instance = this;

            Configuration.Load();
            LanguageManager.Initialize();

            SetWindowContent(new SplashLayout());
        }

        public void SetWindowContent(Layout layout)
        {
            Content = layout;
        }
    }
}