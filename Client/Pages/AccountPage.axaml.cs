using System;
using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Api.Responses;
using Client.Api;
using Client.ReNote;
using Client.Managers;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.Globalization;

namespace Client.Pages
{
    public partial class AccountPage : Page
    {
        public static AccountPage Instance { get; set; }
        public UserSession Session { get; set; }

        public AccountPage()
        {
            InitializeComponent();
            ToolBar = new ToolBar("My Profile");
            ToolBar.Buttons.Add("Account", null);
            ToolBar.Buttons.Add("Documents", null);

#if DEBUG
            if(!Design.IsDesignMode)
                Initialized += OnLayoutInitialized;
#else
            Initialized += OnLayoutInitialized;
#endif
        }

        public void SetSession(UserSession session)
        {
            if (session == null)
                return;

            Session = session;

            m_UserName.Text = Session.RealName;
            if (!string.IsNullOrWhiteSpace(Session.Team.TeamName))
                m_UserTeam.Text = $"({Session.Team.TeamName})";

            if (!string.IsNullOrWhiteSpace(Session.Email))
                m_Email.Text = Session.Email;

            if (!string.IsNullOrWhiteSpace(Session.PhoneNumber))
                m_PhoneNumber.Text = Session.PhoneNumber;

            m_UserId.Text = $"ID: {Session.UserId}";

            if (Session.LastConnection == 0)
            {
                m_LastConnection[!TextBlock.TextProperty] = new DynamicResourceExtension("AccountWelcome");
                return;
            }

            DateTimeOffset lastConnection = DateTimeOffset.FromUnixTimeMilliseconds(Session.LastConnection).ToLocalTime();
            CultureInfo cultureInfo = new CultureInfo(LanguageManager.GetCurrentLanguage());
            m_LastConnection.Text = lastConnection.ToString("F", cultureInfo);
        }

        private async void OnLayoutInitialized(object sender, EventArgs e)
        {
            m_LanguageSelector.Items = LanguageManager.LanguageList;
            m_Version.Text = $"Ver. {Platform.Version}";

            await ApiService.GetThemeList((HttpStatusCode statusCode, ThemeResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                m_ThemeList.Items = response.GetData();
            });
        }

        private void OnProfileButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_FragProfile.IsVisible)
                return;

            m_FragProfile.IsVisible = true;
            m_FragSecurity.IsVisible = false;
            m_FragPreferences.IsVisible = false;
            m_FragMobileLogin.IsVisible = false;
            m_FragAbout.IsVisible = false;
        }

        private void OnSecurityButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_FragSecurity.IsVisible)
                return;

            m_FragSecurity.IsVisible = true;
            m_FragProfile.IsVisible = false;
            m_FragPreferences.IsVisible = false;
            m_FragMobileLogin.IsVisible = false;
            m_FragAbout.IsVisible = false;
        }

        private void OnPreferencesButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_FragPreferences.IsVisible)
                return;

            m_FragPreferences.IsVisible = true;
            m_FragProfile.IsVisible = false;
            m_FragSecurity.IsVisible = false;
            m_FragMobileLogin.IsVisible = false;
            m_FragAbout.IsVisible = false;
        }

        private void OnMobileLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_FragMobileLogin.IsVisible)
                return;

            m_FragMobileLogin.IsVisible = true;
            m_FragProfile.IsVisible = false;
            m_FragSecurity.IsVisible = false;
            m_FragPreferences.IsVisible = false;
            m_FragAbout.IsVisible = false;
        }

        private void OnAboutButtonClicked(object sender, RoutedEventArgs e)
        {
            if (m_FragAbout.IsVisible)
                return;

            m_FragAbout.IsVisible = true;
            m_FragProfile.IsVisible = false;
            m_FragSecurity.IsVisible = false;
            m_FragPreferences.IsVisible = false;
            m_FragMobileLogin.IsVisible = false;
        }

        private void OnThemeListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Theme theme = (Theme)m_ThemeList.SelectedItem;
            ThemeManager.SetTheme(theme);

            m_ThemeList.SelectedItems.Clear();
        }

        private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            Language selectedLanguage = (Language)m_LanguageSelector.SelectedItem;
            LanguageManager.SetLanguage(selectedLanguage.LangCode);

            DateTimeOffset lastConnection = DateTimeOffset.FromUnixTimeMilliseconds(Session.LastConnection).ToLocalTime();
            CultureInfo cultureInfo = new CultureInfo(LanguageManager.GetCurrentLanguage());
            m_LastConnection.Text = lastConnection.ToString("F", cultureInfo);

            // save setting via API
        }
    }
}