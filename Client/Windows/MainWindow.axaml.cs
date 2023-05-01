using Avalonia.Controls;
using Client.Layouts;
using Client.Logging;
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
            InitializeEvents();

            Platform.Log("Initialized MainWindow", LogLevel.INFO);
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

                SplashLayout splashLayout = new SplashLayout();
                SetLayout(splashLayout);
            };
        }

        public void SetLayout(Layout layout)
        {
            Content = layout;
        }

        public Panel GetMainContent()
        {
            Layout layout = (Layout)Content;
            if (layout == null)
                return null;

            return (Panel)layout.Content;
        }
    }
}