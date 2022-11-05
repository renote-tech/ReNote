using System.Threading.Tasks;
using Avalonia.Controls;

using Client.Windows;


namespace Client.Pages
{
    public partial class SplashUI : UserControl
    {
        public SplashUI()
        {
            InitializeComponent();

            Initialized += async (sender, e) =>
            {
                await Task.Delay(2000);
                MainWindow.Instance.Content = new LogonUI();
            };
        }

        
    }
}
