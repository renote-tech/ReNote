using System;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;
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

            m_LanguageSelector.Items = LanguageManager.LanguageList;

            ThemeManager.RestoreDefault();

#if DEBUG
            if(!Design.IsDesignMode)
                Initialized += OnLayoutInitialized;
#else
            Initialized += OnLayoutInitialized;
#endif
        }

        private async void PerformLogin()
        {
            LockLogin();

            if (string.IsNullOrWhiteSpace(m_UsernameBox.Text) || string.IsNullOrWhiteSpace(m_PasswordBox.Text))
            {
                UnlockLogin("LogonEmptyField");
                return;
            }

            await ApiService.AuthenticateAsync(m_UsernameBox.Text, m_PasswordBox.Text, async (HttpStatusCode statusCode, AuthResponse response) =>
            {
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    UnlockLogin("LogonUnexpectedError");
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
                    UnlockLogin("LogonContactAdmin");
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

        private void UnlockLogin(string errorType)
        {
            m_LoginButton.IsEnabled = true;
            m_UsernameBox.IsEnabled = true;
            m_PasswordBox.IsEnabled = true;

            m_LoginErrorLabel[!TextBlock.TextProperty] = new DynamicResourceExtension(errorType);

            m_LoginErrorLabel.IsVisible = true;
            m_LoadingRing.IsVisible = false;

            m_IsLoginLocked = false;
        }

        private async void OnLayoutInitialized(object sender, EventArgs e)
        {
            for (int i = 0; i < LanguageManager.LanguageList.Count; i++)
            {
                if (LanguageManager.GetCurrentLanguage() == LanguageManager.LanguageList[i].LangCode)
                {
                    m_LanguageSelector.SelectedIndex = i;
                    break;
                }
            }

            await ApiService.GetQuotationAsync((HttpStatusCode statusCode, QuotationResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

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
            LanguageManager.SetLanguage(selectedLanguage.LangCode);

            // save setting locally (%APPDATA%\ReNote)
        }
    }
}