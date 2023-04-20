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

            Configuration.LoadAll();
            LanguageManager.Initialize();

            SetLayout(new SplashLayout());
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