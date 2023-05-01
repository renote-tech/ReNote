using System.Net;
using Avalonia.Controls;
using Client.Api;
using Client.Api.Responses;
using Client.Managers;
using Client.Windows;

namespace Client.Layouts
{
    public partial class SplashLayout : Layout
    {
        private bool m_IsErrorState = false;

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
                {
                    m_IsErrorState = true;
                    return;
                }

                ReNote.Client.Instance.SchoolInformation = response.Data;
                MainWindow.Instance.Title = $"ReNote \u03a3 - {response.Data.SchoolName}";
            });

            await ApiService.GetConfigurationAsync((HttpStatusCode statusCode, ConfigResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    m_IsErrorState = true;
                    return;
                }

                PluginManager.Initialize(response.Data.Features);
                ToolbarManager.Initialize(response.Data.ToolbarsInfo);
                ThemeManager.Initialize(response.Data.Themes);
            });

            if (m_IsErrorState)
            {
                ChangeErrorState(true);
                return;
            }

            MainWindow.Instance.SetLayout(new LogonLayout());
        }

        private void ChangeErrorState(bool isErrorState)
        {
            m_LoadingRing.IsVisible    = !isErrorState;
            m_TryAgainButton.IsVisible = isErrorState;
            m_ErrorMessage.IsVisible   = isErrorState;

            m_IsErrorState = isErrorState;
        }

        private void InitializeEvents()
        {
            m_TryAgainButton.Click += (sender, e) =>
            {
                ChangeErrorState(false);
                Initialize();
            };
        }

    }
}