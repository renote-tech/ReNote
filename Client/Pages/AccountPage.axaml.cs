using System;
using System.Globalization;
using System.Linq;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Client.Api;
using Client.Api.Requests;
using Client.Api.Responses;
using Client.Managers;
using Client.Popups;
using Client.ReNote;

namespace Client.Pages
{
    public partial class AccountPage : Page
    {
        private string m_SelectedTheme;
        private string m_SelectedLanguage;
        private readonly Panel[] m_Fragments;

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
                return;
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
                formatter.Text = (string)value;

                double width = formatter.Bounds.Width;
                m_SaveButton.Width = width + 24;
                m_ResetButton.Margin = new Thickness(width + 56, 16);
            });

            User user = User.Current;

            m_UserName.Text = user.RealName;
            m_UserId.Text = $"ID: {user.UserId}";
            m_Version.Text = $"Ver. {Platform.Version}";

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

            Theme[] themes = ThemeManager.GetAllThemes();
            Language[] languages = LanguageManager.Languages;

            m_ThemeList.Items = themes;
            m_LanguageList.Items = languages;

            Theme theme = ThemeManager.GetCurrentTheme();
            if (theme != null)
                m_ThemeList.SelectedIndex = theme.Id;

            Language language = LanguageManager.GetCurrentLanguage();
            if (language != null)
                m_LanguageList.SelectedIndex = language.Id;
        }

        private void InitializeEvents()
        {
            m_ThemeList.SelectionChanged += (sender, e) =>
            {
                Theme theme = (Theme)m_ThemeList.SelectedItem;
                ThemeManager.SetTheme(theme);

                m_SelectedTheme = theme.Name;

                bool isSelected = IsPreferenceSelected();
                m_SaveButton.IsEnabled = isSelected;
                m_ResetButton.IsEnabled = isSelected;
            };

            m_LanguageList.SelectionChanged += (sender, e) =>
            {
                Language language = (Language)m_LanguageList.SelectedItem;
                LanguageManager.SetLanguage(language.LangCode);
                SetLanguageDataFormat();

                m_SelectedLanguage = language.LangCode;

                bool isSelected = IsPreferenceSelected();
                m_SaveButton.IsEnabled = isSelected;
                m_ResetButton.IsEnabled = isSelected;
            };

            m_ProfileButton.Click += (sender, e) => SwitchFragment(m_FragProfile);
            m_SecurityButton.Click += (sender, e) => SwitchFragment(m_FragSecurity);
            m_PreferencesButton.Click += (sender, e) => SwitchFragment(m_FragPreferences);
            m_MobileLoginButton.Click += (sender, e) => SwitchFragment(m_FragMobileLogin);
            m_AboutButton.Click += (sender, e) => SwitchFragment(m_FragAbout);

            m_SaveButton.Click += (sender, e) => SavePreferences();
            m_ResetButton.Click += (sender, e) => ResetPreferences();
        }

        private void SwitchFragment(Panel targetFragment)
        {
            if (!m_Fragments.Contains(targetFragment))
                return;

            for (int i = 0; i < m_Fragments.Length; i++)
                m_Fragments[i].IsVisible = m_Fragments[i] == targetFragment;
        }

        private void SetLanguageDataFormat()
        {
            DateTimeOffset lastConnection = DateTimeOffset.FromUnixTimeMilliseconds(User.Current.LastConnection).ToLocalTime();
            CultureInfo language = new CultureInfo(LanguageManager.GetCurrentLanguage().LangCode);
            m_LastConnection.Text = lastConnection.ToString("F", language);
        }

        private async void SavePreferences()
        {
            if (m_SelectedTheme == null && m_SelectedLanguage == null)
                return;

            if (User.Current.Theme == m_SelectedTheme && User.Current.Language == m_SelectedLanguage)
                return;

            PreferenceRequest request = new PreferenceRequest() { Theme = m_SelectedTheme, Language = m_SelectedLanguage };
            await ApiService.UpdatePreferencesAsync(User.Current.SessionId, User.Current.AuthToken, request, (HttpStatusCode statusCode, Response response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    ResetPreferences();
                    PopupMessage.Show("UnexpectedError", null);

                    return;
                }
                if (m_SelectedTheme != null)
                    User.Current.Theme = m_SelectedTheme;
                
                if (m_SelectedLanguage != null)
                    User.Current.Language = m_SelectedLanguage;

                m_SaveButton.IsEnabled = false;
                m_ResetButton.IsEnabled = false;
            });
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

        private bool IsPreferenceSelected()
        {
            bool isThemeSelected = (m_SelectedTheme != null && m_SelectedTheme != User.Current.Theme);
            bool isLangSelected = (m_SelectedLanguage != null && m_SelectedLanguage != User.Current.Language);

            return isThemeSelected || isLangSelected;
        }

        public override string GetToolbarId()
        {
            return "User";  
        }
    }
}