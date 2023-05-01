using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Client.Api;
using Client.Api.Requests;
using Client.Api.Responses;
using Client.Builders;
using Client.Managers;
using Client.ReNote.Data;
using Client.Windows;

namespace Client.Layouts
{
    public partial class LogonLayout : Layout
    {
        private bool m_IsLoginLocked = false;

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

            m_FullVersionLabel.Text = ClientInfo.GetFullVersion();

            await ApiService.GetQuotationAsync((HttpStatusCode statusCode, QuotationResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                m_QuotationContentLabel.Text = $"\"{response.Data.Content}\"";
                m_QuotationAuthorLabel.Text = $"- {response.Data.Author}";
            });
        }

        private void InitializeEvents()
        {
            m_ReNoteIcon.DoubleTapped += (sender, e) =>
            {
                MessageBoxBuilder.Create()
                             .SetTitle("About ReNote \u03A3")
                             .SetMessage($"Entirely developed by Alian/DEAD \ud83d\udc7b\n" +
                                         $"Ver. {ClientInfo.Version} ({ClientInfo.Configuration})\n" +
                                         $"Code name \"{ClientInfo.VersionName}\"")
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

            m_LoginButton.Click += (sender, e) =>
            {
                PerformLogin();
            };

            m_LanguageList.SelectionChanged += (sender, e) =>
            {
                Language language = (Language)m_LanguageList.SelectedItem;
                LanguageManager.SetLanguage(language.LangCode);
                Configuration.Save("Local", language.LangCode);
            };
        }

        private async void PerformLogin()
        {
            LockLogin();

            if (string.IsNullOrWhiteSpace(m_UsernameBox.Text) || string.IsNullOrWhiteSpace(m_PasswordBox.Text))
            {
                UnlockLogin("LogonEmptyField");
                return;
            }

            AuthRequest authRequest = new AuthRequest(m_UsernameBox.Text, m_PasswordBox.Text);
            await ApiService.AuthenticateAsync(authRequest, async (HttpStatusCode statusCode, AuthResponse response) =>
            {
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    UnlockLogin("UnexpectedError");
                    return;
                }

                if (response.Status != 200)
                {
                    UnlockLogin(response.Message);
                    return;
                }

                await User.GetAsync(response.Data);
                if (User.Current == null)
                {
                    UnlockLogin("LogonContactAdmin");
                    return;
                }

                MainWindow.Instance.SetLayout(new UserLayout());
            });
        }

        private void LockLogin()
        {
            m_IsLoginLocked = true;

            m_LoginButton.IsEnabled = false;
            m_UsernameBox.IsEnabled = false;
            m_PasswordBox.IsEnabled = false;

            m_LoginErrorLabel.IsVisible = false;
            m_LoadingRing.IsVisible = true;

            m_LoginButton.Focus();
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
    }
}