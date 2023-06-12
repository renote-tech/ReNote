namespace Client.Pages;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Client.Api;
using Client.Api.Requests;
using Client.Dialogs;
using Client.Helpers;
using Client.Managers;
using Client.ReNote.Data;
using Client.Windows;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

public partial class AccountPage : Page
{
    private string m_SelectedTheme;
    private string m_SelectedLanguage;
    private readonly Panel[] m_Fragments;

    private DialogPassword s_DialogPassword;

    public AccountPage()
    {
        InitializeComponent();

        m_Fragments = new Panel[]
        {
                m_FragProfile,
                m_FragSecurity,
                m_FragPreferences,
                m_FragMobileLogin,
                m_FragAbout
        };

#if DEBUG
        if (Design.IsDesignMode)
        {
            m_ProfileButton.Click += (sender, e) => SwitchFragment(m_FragProfile);
            m_SecurityButton.Click += (sender, e) => SwitchFragment(m_FragSecurity);
            m_PreferencesButton.Click += (sender, e) => SwitchFragment(m_FragPreferences);
            m_MobileLoginButton.Click += (sender, e) => SwitchFragment(m_FragMobileLogin);
            m_AboutButton.Click += (sender, e) => SwitchFragment(m_FragAbout);

            return;
        }
#endif

        InitializeLayout();
        InitializeEvents();
    }

    private void InitializeLayout()
    {
        FormattedText formatter = new FormattedText()
        {
            Typeface = new Typeface(m_SaveButton.FontFamily, m_SaveButton.FontStyle, m_SaveButton.FontWeight),
            FontSize = m_SaveButton.FontSize
        };

        m_SaveButton.GetObservable(ContentProperty).Subscribe(value =>
        {
            if (value is not string)
                return;

            formatter.Text = (string)value;

            double width = formatter.Bounds.Width;
            m_SaveButton.Width = width + 32;
            m_ResetButton.Margin = new Thickness(width + 64, 16);
        });


        m_UserId.IsVisible = PluginManager.Enabled(PluginTypes.USER, Plugins.KN_SHOW_ID);

        if (PluginManager.Enabled(PluginTypes.AUTO, Plugins.KN_CHANGE_PASSWORD))
        {
            m_ChangePasswordBtn.IsEnabled = true;
            s_DialogPassword = new DialogPassword();
        }
        else
        {
            m_CannotChangePasswordLabel.IsVisible = true;
            m_CannotChangePasswordLearnLabel.IsVisible = true;
        }

        User user = User.Current;

        m_UserName.Text = user.RealName;
        m_UserId.Text = $"ID: {user.UserId}";
        m_Version.Text = $"Ver. {ClientInfo.Version} ({ClientInfo.Configuration})";

        if (!string.IsNullOrWhiteSpace(user.Team.TeamName))
            m_UserTeam.Text = $"({user.Team.TeamName})";

        if (!string.IsNullOrWhiteSpace(user.Email))
            m_Email.Text = user.Email;

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            m_PhoneNumber.Text = user.PhoneNumber;

        if (user.LastConnection == 0)
            m_LastConnection[!TextBlock.TextProperty] = new DynamicResourceExtension("AccountWelcome");
        else
            SetLanguageDataFormat();

        Theme[] themes = ThemeManager.Themes;
        Language[] languages = LanguageManager.Languages;

        m_ThemeList.Items = themes;
        m_LanguageList.Items = languages;

        Theme theme = ThemeManager.CurrentTheme;
        if (theme != null)
            m_ThemeList.SelectedIndex = theme.Id;
        else
            m_ThemeList.SelectedItem = 0;

        Language language = LanguageManager.CurrentLanguage;
        if (language != null)
            m_LanguageList.SelectedIndex = language.Id;

#if DEBUG || BETA
        m_FullVersionLabel.Text = ClientInfo.GetFullVersion();
#endif
    }

    private void InitializeEvents()
    {
        m_ProfileButton.Click += (sender, e) => SwitchFragment(m_FragProfile);
        m_SecurityButton.Click += (sender, e) => SwitchFragment(m_FragSecurity);
        m_PreferencesButton.Click += (sender, e) => SwitchFragment(m_FragPreferences);
        m_MobileLoginButton.Click += (sender, e) => SwitchFragment(m_FragMobileLogin);
        m_AboutButton.Click += (sender, e) => SwitchFragment(m_FragAbout);

        m_ChangePasswordBtn.Click += (sender, e) => s_DialogPassword.Show();

        m_CannotChangePasswordLearnLabel.PointerReleased += (sender, e) => ShellHelper.OpenUrl("https://example.com/?linkid=288238");

        m_ThemeList.SelectionChanged += (sender, e) =>
        {
            Theme theme = (Theme)m_ThemeList.SelectedItem;
            ThemeManager.SetTheme(theme);

            m_SelectedTheme = theme.Name;

            bool isSelected = IsSaveAllowed();
            m_SaveButton.IsEnabled = isSelected;
            m_ResetButton.IsEnabled = isSelected;
        };

        m_LanguageList.SelectionChanged += (sender, e) =>
        {
            Language language = (Language)m_LanguageList.SelectedItem;
            LanguageManager.SetLanguage(language.LangCode);
            SetLanguageDataFormat();

            m_SelectedLanguage = language.LangCode;

            bool isSelected = IsSaveAllowed();
            m_SaveButton.IsEnabled = isSelected;
            m_ResetButton.IsEnabled = isSelected;
        };

        m_SaveButton.Click += async (sender, e) => await SavePreferencesAsync();
        m_ResetButton.Click += (sender, e) => ResetPreferences();
    }

    private void SwitchFragment(Panel targetFragment)
    {
        if (!m_Fragments.Contains(targetFragment))
            return;

        if (IsSaveAllowed())
            ResetPreferences();

        for (int i = 0; i < m_Fragments.Length; i++)
            m_Fragments[i].IsVisible = m_Fragments[i] == targetFragment;
    }

    private void SetLanguageDataFormat()
    {
        DateTimeOffset lastConnection = DateTimeOffset.FromUnixTimeMilliseconds(User.Current.LastConnection).ToLocalTime();
        CultureInfo language = new CultureInfo(LanguageManager.CurrentLanguage.LangCode);
        m_LastConnection.Text = lastConnection.ToString("F", language);
    }

    private async Task SavePreferencesAsync()
    {
        if (m_SelectedTheme == null && m_SelectedLanguage == null)
            return;

        if (User.Current.Theme == m_SelectedTheme && User.Current.Language == m_SelectedLanguage)
            return;

        m_SaveButton.IsEnabled = false;
        m_ResetButton.IsEnabled = false;

        PreferenceRequest request = new PreferenceRequest(m_SelectedLanguage, m_SelectedTheme);
        await ApiService.SetPreferencesAsync(request, (requestStatus, response) =>
        {
            if (requestStatus != ResponseStatus.OK)
            {
                ResetPreferences();

                if (requestStatus == ResponseStatus.EXPIRED)
                    DialogMessage.Show("$$DialogWarning$$", "$$SessionExpired$$", () => MainWindow.Instance.SetLogonUI());
                else if (requestStatus != ResponseStatus.SERVICE_UNAVAILABLE)
                    DialogMessage.Show("$$DialogError$$", "$$UnexpectedError$$", null, false);

                m_SaveButton.IsEnabled = true;
                m_SaveButton.IsEnabled = true;

                return;
            }

            if (m_SelectedTheme != null)
                User.Current.Theme = m_SelectedTheme;

            if (m_SelectedLanguage != null)
                User.Current.Language = m_SelectedLanguage;

            m_SaveButton.IsEnabled = false;
            m_ResetButton.IsEnabled = false;
        });

        m_SaveButton.RestoreContent();
    }

    private void ResetPreferences()
    {
        Theme theme = ThemeManager.GetThemeByName(User.Current.Theme);
        Language language = LanguageManager.GetLanguageByName(User.Current.Language);

        m_ThemeList.SelectedIndex = theme.Id;
        m_LanguageList.SelectedIndex = language.Id;

        m_SelectedTheme = theme.Name;
        m_SelectedLanguage = language.LangCode;

        m_SaveButton.IsEnabled = false;
        m_ResetButton.IsEnabled = false;
    }

    private bool IsSaveAllowed()
    {
        bool isThemeSelected = m_SelectedTheme != null && m_SelectedTheme != User.Current.Theme;
        bool isLangSelected = m_SelectedLanguage != null && m_SelectedLanguage != User.Current.Language;

        return isThemeSelected || isLangSelected;
    }

    public override void Destroy()
    {
        if (IsSaveAllowed())
            ResetPreferences();
    }

    public override string GetToolbarId()
    {
        return "User";
    }
}