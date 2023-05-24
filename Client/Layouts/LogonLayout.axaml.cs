namespace Client.Layouts;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;

using Client.Api;
using Client.Api.Requests;
using Client.Builders;
using Client.Dialogs;
using Client.Managers;
using Client.ReNote.Data;
using Client.Windows;

public partial class LogonLayout : Layout
{
    private bool m_IsLoginLocked = false;

    private const string EMPTY_FIELD         = "LogonEmptyField";
    private const string UNEXPECTED_ERROR    = "UnexpectedError";
    private const string CONTACT_ADMIN       = "LogonContactAdmin";
    private const string SERVICE_UNAVAILABLE = "AbnormalStatus";

    public LogonLayout()
    {
        InitializeComponent();

#if DEBUG
        if (Design.IsDesignMode)
            return;
#endif

        InitializeLayout();
        InitializeEvents();
    }

    private async void InitializeLayout()
    {
        ThemeManager.RestoreDefault();
        int index = LanguageManager.RestoreDefault();

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

    private void InitializeEvents()
    {
        User.Delete();

        m_ReNoteIcon.DoubleTapped += (sender, e) =>
        {
            MessageBoxBuilder.Create()
                             .SetTitle("$$AboutReNote$$")
                             .SetMessage($"$$AboutReNoteContent$$")
                             .SetType(MessageBoxType.OK)
                             .SetIcon(MessageBoxIcon.INFO)
                             .Show();
        };

        m_PasswordBox.KeyUp += (sender, e) =>
        {
            if (m_IsLoginLocked)
                return;

            if (e.Key == Key.Enter)
                PerformLogin();
        };

        m_LoginButton.Click += (sender, e) => PerformLogin();

        m_LanguageList.SelectionChanged += (sender, e) =>
        {
            Language language = (Language)m_LanguageList.SelectedItem;
            LanguageManager.SetLanguage(language.LangCode);
            Configuration.Save(Configuration.LOCAL_CONFIG, language.LangCode);
        };
    }

    private async void PerformLogin()
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

            bool hasFailed = await User.GetAsync(response.Data);
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
        
        m_LoadingRing.IsVisible = true;

        Focus();
    }

    private void Unlock(string errorType)
    {
        if (string.IsNullOrWhiteSpace(errorType) || errorType == SERVICE_UNAVAILABLE)
            return;

        if (!PluginManager.Enabled(PluginTypes.EXPERIMENTAL, Plugins.EX_NEW_LOGIN_DIALOG))
        {
            m_LoginButton.IsEnabled = true;
            m_UsernameBox.IsEnabled = true;
            m_PasswordBox.IsEnabled = true;

            m_LoginButton.IsTabStop = true;
            m_UsernameBox.IsTabStop = true;
            m_PasswordBox.IsTabStop = true;

            m_LoadingRing.IsVisible = false;

            m_LoginErrorLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(errorType);
            m_LoginErrorLabel.IsVisible = true;

            m_IsLoginLocked = false;

            return;
        }

        m_LoadingRing.IsVisible = false;

        DialogMessage.Show($"$${errorType}$$", () =>
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