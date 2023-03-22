using System.Net;
using Newtonsoft.Json;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Client.Api;
using Client.Api.Responses;
using Client.Windows;
using Avalonia.Interactivity;

namespace Client.Layout
{
    public partial class SplashUI : UserControl
    {
        public SplashUI()
        {
            InitializeComponent();
            Initialized += (sender, e) => Initialize();
        }

        private void OnTryAgainButtonClicked(object sender, RoutedEventArgs e)
        {
            loadingRing.IsVisible = true;
            tryAgainButton.IsVisible = false;
            errorMessage.Foreground = Brushes.Transparent;

            Initialize();
        }

        public async void Initialize()
        {
            ApiClient.Initialize();

            HttpResponseMessage schoolDataResponse = await ApiClient.GetSchoolDataAsync();
            if (schoolDataResponse.StatusCode != HttpStatusCode.OK)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    loadingRing.IsVisible = false;
                    tryAgainButton.IsVisible = true;
                    errorMessage.Foreground = Brushes.White;
                });
                return;
            }

            string body = await schoolDataResponse.Content.ReadAsStringAsync();
            ReNote.Client.Instance.SchoolInformation = JsonConvert.DeserializeObject<SchoolResponse>(body).GetData();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                MainWindow.Instance.Title = $"ReNote \u03A3 - {ReNote.Client.Instance.SchoolInformation.GetSchoolName()}";
                MainWindow.Instance.Content = new LogonUI();
            });
        }
    }
}