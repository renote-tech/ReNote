using System.Net;
using Newtonsoft.Json;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Client.Api;
using Client.Api.Responses;
using Client.Windows;

namespace Client.Pages
{
    public partial class SplashUI : UserControl
    {
        public SplashUI()
        {
            InitializeComponent();

            Initialized += (sender, e) =>
            {
                Dispatcher.UIThread.Post(async () =>
                {
                    ApiClient.Initialize();

                    HttpResponseMessage schoolDataResponse = await ApiClient.GetSchoolDataAsync();
                    if(schoolDataResponse.StatusCode != HttpStatusCode.OK)
                    {
                        loadingRing.IsVisible = false;
                        errorMessage.Foreground = Brushes.White;
                        return;
                    }

                    string body = await schoolDataResponse.Content.ReadAsStringAsync();
                    ReNote.Client.Instance.SchoolInformation = JsonConvert.DeserializeObject<SchoolResponse>(body).Data;

                    MainWindow.Instance.Title = $"ReNote \u03A3 - {ReNote.Client.Instance.SchoolInformation.SchoolName}";
                    MainWindow.Instance.Content = new LogonUI();
                }, DispatcherPriority.Background);
            };
        }
    }
}