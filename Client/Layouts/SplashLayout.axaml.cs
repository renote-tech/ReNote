using System.Net;
using Avalonia.Interactivity;
using Client.Api;
using Client.Api.Responses;
using Client.ReNote;
using Client.Windows;

namespace Client.Layouts
{
    public partial class SplashLayout : Layout
    {
        public SplashLayout()
        {
            InitializeComponent();
            Initialized += (sender, e) => Initialize();
        }

        private async void Initialize()
        {
            ApiService.Initialize();

            await ApiService.GetSchoolDataAsync((HttpStatusCode statusCode, SchoolResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                School schoolData = response.GetData();
                ReNote.Client.Instance.SchoolInformation = schoolData;
            });

            await ApiService.GetConfigurationAsync(null);

            InvokeLoginState();
        }

        private void InvokeLoginState()
        {
            School schoolInfo = ReNote.Client.Instance.SchoolInformation;
            if (schoolInfo == null)
            {
                InvokeErrorState();
                return;
            }

            MainWindow.Instance.Title = $"ReNote \u03a3 - {ReNote.Client.Instance.SchoolInformation.SchoolName}";
            MainWindow.Instance.SetWindowContent(new LogonLayout());
        }

        private void InvokeErrorState()
        {
            m_LoadingRing.IsVisible = false;
            m_TryAgainButton.IsVisible = true;
            m_ErrorMessage.IsVisible = true;
        }

        private void OnTryAgainButtonClicked(object sender, RoutedEventArgs e)
        {
            m_LoadingRing.IsVisible = true;
            m_TryAgainButton.IsVisible = false;
            m_ErrorMessage.IsVisible = false;

            Initialize();
        }
    }
}