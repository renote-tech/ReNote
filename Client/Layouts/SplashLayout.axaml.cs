using System.Net;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Client.Api;
using Client.Api.Responses;
using Client.ReNote;
using Client.Windows;

namespace Client.Layouts
{
    public partial class SplashLayout : Layout
    {
        private bool m_HasRequestFailed = false;

        public SplashLayout()
        {
            InitializeComponent();
            Initialized += (sender, e) => Initialize();
        }

        public override void InitializeLanguage()
        {
            m_SplashDeveloper.Text = Language.GetString("splash_developer_info");
            m_SplashCopyright.Text = Language.GetString("splash_copyright_info");
            m_TryAgainButton.Content = Language.GetString("splash_try_again");
        }

        private async void Initialize()
        {
            ApiService.Initialize();

            await ApiService.GetSchoolDataAsync((HttpStatusCode statusCode, SchoolResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    InvokeErrorState();
                    return;
                }

                School schoolData = response.GetData();
                ReNote.Client.Instance.SchoolInformation = schoolData;
            });

            
            await ApiService.GetConfigurationAsync(null);

            if (m_HasRequestFailed)
                return;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MainWindow.Instance.Title = $"ReNote \u03A3 - {ReNote.Client.Instance.SchoolInformation.SchoolName}";
                MainWindow.Instance.SetWindowContent(new LogonLayout());
            });
        }

        private async void InvokeErrorState()
        {
            m_HasRequestFailed = true;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                m_LoadingRing.IsVisible = false;
                m_TryAgainButton.IsVisible = true;
                m_ErrorMessage.Text = Language.GetString("splash_connection_error");
            });
        }

        private void OnTryAgainButtonClicked(object sender, RoutedEventArgs e)
        {
            m_HasRequestFailed = false;

            m_LoadingRing.IsVisible = true;
            m_TryAgainButton.IsVisible = false;
            m_ErrorMessage.Text = string.Empty;

            Initialize();
        }
    }
}