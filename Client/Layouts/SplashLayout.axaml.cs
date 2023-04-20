using System.Net;
using Avalonia.Controls;
using Client.Api;
using Client.Api.Responses;
using Client.Managers;
using Client.ReNote;
using Client.Windows;

namespace Client.Layouts
{
    public partial class SplashLayout : Layout
    {
        public SplashLayout()
        {
            InitializeComponent();

#if DEBUG
            if (Design.IsDesignMode)
                return;
#endif

            Initialize();
            InitializeEvents();
        }

        private async void Initialize()
        {
            ApiService.Initialize();

            await ApiService.GetSchoolDataAsync((HttpStatusCode statusCode, SchoolResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                ReNote.Client.Instance.SchoolInformation = response.Data;
            });

            await ApiService.GetConfigurationAsync((HttpStatusCode statusCode, ConfigResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                PluginManager.Initialize(response.Data.Features);
                ToolbarManager.Initialize(response.Data.ToolbarsInfo);
            });

            School schoolInfo = ReNote.Client.Instance.SchoolInformation;
            if (schoolInfo == null)
            {
                m_LoadingRing.IsVisible = false;
                m_TryAgainButton.IsVisible = true;
                m_ErrorMessage.IsVisible = true;
                return;
            }

            ThemeManager.Initialize();

            MainWindow.Instance.Title = $"ReNote \u03a3 - {schoolInfo.SchoolName}";
            MainWindow.Instance.SetLayout(new LogonLayout());
        }

        private void InitializeEvents()
        {
            m_TryAgainButton.Click += (sender, e) =>
            {
                m_LoadingRing.IsVisible = true;
                m_TryAgainButton.IsVisible = false;
                m_ErrorMessage.IsVisible = false;

                Initialize();
            };
        }

    }
}