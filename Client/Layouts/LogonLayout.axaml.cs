using System;
using System.Net;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Api;
using Client.Api.Responses;
using Client.Builders;
using Client.Managers;
using Client.ReNote;
using Client.Windows;

namespace Client.Layouts
{
    public partial class LogonLayout : Layout
    {
        private bool m_IsLoginLocked = false;

        public LogonLayout()
        {
            InitializeComponent();

            m_LanguageSelector.Items = Language.LanguageList;

            ThemeManager.RestoreDefault();

#if DEBUG
            if(!Design.IsDesignMode)
                Initialized += OnLayoutInitialized;
#else
            Initialized += OnLayoutInitialized;
#endif
        }

        public override void InitializeLanguage()
        {
            m_SignInLabel.Text      = Language.GetString("logon_main_title");
            m_UsernameBox.Watermark = Language.GetString("logon_watermark_username");
            m_PasswordBox.Watermark = Language.GetString("logon_watermark_password");
            m_LoginButton.Content   = Language.GetString("logon_sign_in_button");
            m_CopyrightLabel.Text   = Language.GetString("logon_copyright_text");
        }

        private async void PerformLogin()
        {
            LockLogin();

            if (string.IsNullOrWhiteSpace(m_UsernameBox.Text) || string.IsNullOrWhiteSpace(m_PasswordBox.Text))
            {
                UnlockLogin("Username or password may not be empty");
                return;
            }

            await ApiService.AuthenticateAsync(m_UsernameBox.Text, m_PasswordBox.Text, async (HttpStatusCode statusCode, AuthResponse response) =>
            {
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    UnlockLogin("An unexpected error occurred");
                    return;
                }

                if (response.GetStatus() != 200)
                {
                    UnlockLogin(response.GetMessage());
                    return;
                }

                UserSession session = await UserSession.GetAsync(response.GetData());

                if (session == null)
                {
                    UnlockLogin("Session is null");
                    return;
                }

                UserLayout userLayout = new UserLayout();
                userLayout.Session = session;

                MainWindow.Instance.SetWindowContent(userLayout);
            });
        }

        private void LockLogin()
        {
            m_LoginButton.Focus();

            m_LoginButton.IsEnabled = false;
            m_UsernameBox.IsEnabled = false;
            m_PasswordBox.IsEnabled = false;

            m_LoginErrorLabel.IsVisible = false;
            m_LoadingRing.IsVisible = true;

            m_IsLoginLocked = true;
        }

        private void UnlockLogin(string error)
        {
            m_LoginButton.IsEnabled = true;
            m_UsernameBox.IsEnabled = true;
            m_PasswordBox.IsEnabled = true;

            m_LoginErrorLabel.Text = error;

            m_LoginErrorLabel.IsVisible = true;
            m_LoadingRing.IsVisible = false;

            m_IsLoginLocked = false;
        }

        private async void OnLayoutInitialized(object sender, EventArgs e)
        {
            for (int i = 0; i < Language.LanguageList.Count; i++)
            {
                if (Language.GetCurrentLanguage() == Language.LanguageList[i].LangCode)
                {
                    m_LanguageSelector.SelectedIndex = i;
                    break;
                }
            }

            await ApiService.GetQuotationAsync((HttpStatusCode statusCode, QuotationResponse response) =>
            {
                QuotationData quotationData = response.GetData();

                m_QuotationContentLabel.Text = $"\"{quotationData.GetContent()}\"";
                m_QuotationAuthorLabel.Text = $"- {quotationData.GetAuthor()}";
            });
        }

        private void OnPasswordBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (m_IsLoginLocked)
                return;

            if (e.Key == Key.Enter)
                PerformLogin();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void OnLogoClicked(object sender, PointerReleasedEventArgs e)
        {
            MessageBoxBuilder.Create()
                             .SetTitle("About ReNote \u03A3")
                             .SetMessage($"Entirely developed by Alian/DEAD \ud83d\udc7b\nVersion {Platform.VersionName}/{Platform.Version}")
                             .SetType(MessageBoxType.OK)
                             .SetIcon(MessageBoxIcon.INFO)
                             .Show();
        }

        private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            Language selectedLanguage = (Language)m_LanguageSelector.SelectedItem;
            Language.SetLanguage(selectedLanguage.LangCode);

            InitializeLanguage();

            // save setting locally (%APPDATA%\ReNote)
        }
    }
}