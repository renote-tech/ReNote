using Avalonia.Controls;
using Client.Layouts;

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

            Language.Initialize();
            Configuration.Load();

            SetWindowContent(new SplashLayout());
        }

        public void SetWindowContent(Layout layout)
        {
            Content = layout;
        }
    }
}