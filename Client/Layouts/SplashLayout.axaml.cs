namespace Client.Layouts;

using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Client.Api;
using Client.Managers;
using Client.ReNote.Data;
using Client.Windows;
using System.Threading.Tasks;

public partial class SplashLayout : Layout
{
    private bool m_IsErrorState = false;
    private bool m_IsMaintenanceState = false;

    public SplashLayout()
    {
        InitializeComponent();

#if DEBUG
        if (Design.IsDesignMode)
            return;
#endif

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        Initialized += async (sender, e) =>
        {
            await InitializeReNote();
        };

        m_TryAgainButton.Click += async (sender, e) =>
        {
            ChangeErrorState(false);

            m_IsErrorState = false;
            m_IsMaintenanceState = false;

            await InitializeReNote();
        };
    }

    private async Task InitializeReNote()
    {
        ApiService.Initialize();

        await ApiService.GetSchoolDataAsync((requestStatus, response) =>
        {
            if (requestStatus != ResponseStatus.OK)
            {
                m_IsErrorState = true;
                m_IsMaintenanceState = requestStatus == ResponseStatus.SERVICE_UNAVAILABLE;
                return;
            }

            School.Instance = response.Data;
            MainWindow.Instance.Title = $"ReNote \u03a3 - {response.Data.SchoolName}";
        });

        await ApiService.GetConfigurationAsync((requestStatus, response) =>
        {
            if (requestStatus != ResponseStatus.OK)
            {
                m_IsErrorState = true;
                m_IsMaintenanceState = requestStatus == ResponseStatus.SERVICE_UNAVAILABLE;
                return;
            }

            PluginManager.Initialize(response.Data.Features);
            ToolbarManager.Initialize(response.Data.ToolbarsInfo);
            ThemeManager.Initialize(response.Data.Themes);

            UserLayout.SetGlobalMenu(response.Data.MenuInfo);
        });


        if (m_IsErrorState)
        {
            ChangeErrorState(true);
            return;
        }

        MainWindow.Instance.SetLogonUI();
    }

    private void ChangeErrorState(bool isErrorState)
    {
        string messageKey = m_IsMaintenanceState ? "ServiceUnavailable" : "SplashErrorMessage";

        m_LoadingRing.IsVisible = !isErrorState;
        m_TryAgainButton.IsVisible = isErrorState;
        m_ErrorMessage.IsVisible = isErrorState;

        m_ErrorMessage[!TextBlock.TextProperty] = new DynamicResourceExtension(messageKey);
    }
}