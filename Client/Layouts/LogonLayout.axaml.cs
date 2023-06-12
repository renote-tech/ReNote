namespace Client.Layouts;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;

using Client.Api;
using Client.Api.Requests;
using Client.Dialogs;
using Client.Managers;
using Client.ReNote.Data;
using Client.Windows;
using System.Threading.Tasks;

public partial class LogonLayout : Layout
{
    private bool m_IsLoginLocked = false;

    private const string EMPTY_FIELD = "LogonEmptyField";
    private const string UNEXPECTED_ERROR = "UnexpectedError";
    private const string CONTACT_ADMIN = "LogonContactAdmin";
    private const string SERVICE_UNAVAILABLE = "AbnormalStatus";

    public LogonLayout()
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
        User.Delete();

        Initialized += async (sender, e) =>
        {
            await InitializeLayout();
        };

        m_PasswordBox.KeyUp += (sender, e) =>
        {
            if (m_IsLoginLocked)
                return;

            if (e.Key == Key.Enter)
                m_LoginButton.SimulateClick();
        };

        m_LoginButton.Click += async (sender, e) => await PerformLogin();

        m_LanguageList.SelectionChanged += (sender, e) =>
        {
            Language language = (Language)m_LanguageList.SelectedItem;
            LanguageManager.SetLanguage(language.LangCode);
            Configuration.Save(Configuration.LOCAL_CONFIG, language.LangCode);
        };
    }

    private async Task InitializeLayout()
    {
        ThemeManager.SetThemeByName(ThemeManager.DEFAULT_THEME_NAME);
        int index = LanguageManager.SetLanguage(Configuration.Language);

        m_LanguageList.Items = LanguageManager.Languages;
        m_LanguageList.SelectedIndex = index;

#if DEBUG || BETA
        m_FullVersionLabel.Text = ClientInfo.GetFullVersion();
#endif

        await ApiService.GetQuotationAsync((requestStatus, response) =>
        {
            if (requestStatus != ResponseStatus.OK)
                return;

            m_QuotationContentLabel.Text = $"\"{response.Data.Content}\"";
            m_QuotationAuthorLabel.Text = $"- {response.Data.Author}";
        });
    }

    private async Task PerformLogin()
    {
        Lock();

        if (string.IsNullOrWhiteSpace(m_UsernameBox.Text) || string.IsNullOrWhiteSpace(m_PasswordBox.Text))
        {
            Unlock(EMPTY_FIELD);
            return;
        }

        AuthRequest authRequest = new AuthRequest(m_UsernameBox.Text, m_PasswordBox.Text);
        await ApiService.AuthenticateAsync(authRequest, async (requestStatus, response) =>
        {
            if (requestStatus == ResponseStatus.UNKNOWN)
            {
                Unlock(UNEXPECTED_ERROR);
                return;
            }

            if (response.Status != 200)
            {
                Unlock(response.Message);
                return;
            }

            bool hasFailed = await User.CreateAsync(response.Data);
            if (hasFailed)
            {
                Unlock(CONTACT_ADMIN);
                return;
            }

            MainWindow.Instance.SetUserUI();
        });
    }

    private void Lock()
    {
        m_IsLoginLocked = true;

        m_LoginButton.IsTabStop = false;
        m_UsernameBox.IsTabStop = false;
        m_PasswordBox.IsTabStop = false;

        m_LoginButton.IsEnabled = false;
        m_UsernameBox.IsEnabled = false;
        m_PasswordBox.IsEnabled = false;

        if (!PluginManager.Enabled(PluginTypes.EXPERIMENTAL, Plugins.EX_NEW_LOGIN_DIALOG))
            m_LoginErrorLabel.IsVisible = false;

        Focus();
    }

    private void Unlock(string errorType)
    {
        if (string.IsNullOrWhiteSpace(errorType) || errorType == SERVICE_UNAVAILABLE)
            return;

        m_LoginButton.RestoreContent();

        if (!PluginManager.Enabled(PluginTypes.EXPERIMENTAL, Plugins.EX_NEW_LOGIN_DIALOG))
        {
            m_LoginButton.IsEnabled = true;
            m_UsernameBox.IsEnabled = true;
            m_PasswordBox.IsEnabled = true;

            m_LoginButton.IsTabStop = true;
            m_UsernameBox.IsTabStop = true;
            m_PasswordBox.IsTabStop = true;

            m_LoginErrorLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(errorType);
            m_LoginErrorLabel.IsVisible = true;

            m_IsLoginLocked = false;

            return;
        }

        DialogMessage.Show("$$DialogError$$", $"$${errorType}$$", () =>
        {
            m_LoginButton.IsEnabled = true;
            m_UsernameBox.IsEnabled = true;
            m_PasswordBox.IsEnabled = true;

            m_LoginButton.IsTabStop = true;
            m_UsernameBox.IsTabStop = true;
            m_PasswordBox.IsTabStop = true;

            m_IsLoginLocked = false;
        }, false);
    }
}