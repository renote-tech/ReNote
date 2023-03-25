using System.Net;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Newtonsoft.Json;
using Client.Api;
using Client.Api.Responses;
using Client.ReNote;
using Client.Windows;
using Client.Builders;

namespace Client.Layout
{
    public partial class LogonUI : UserControl
    {
        public LogonUI()
        {
            InitializeComponent();
            Initialized += OnLayoutInitialized;
        }

        private async void OnLayoutInitialized(object sender, EventArgs e)
        {
            HttpResponseMessage quotationDataResponse = await ApiClient.GetQuotationAsync();
            string body = await quotationDataResponse.Content.ReadAsStringAsync();
            QuotationData quotationData = JsonConvert.DeserializeObject<QuotationResponse>(body).GetData();

            quotationText.Text = $"\"{quotationData.GetContent()}\"";
            quotationAuthor.Text = $"- {quotationData.GetAuthor()}";
        }

        private void OnPasswordBoxKeyUp(object sender, KeyEventArgs e)
        {                             
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

        private async void PerformLogin()
        {
            if (IsFieldsEmpty())
                return;

            HttpResponseMessage authResponse = await ApiClient.AuthenticateAsync(usernameBox.Text, passwordBox.Text);
            if(authResponse.StatusCode == HttpStatusCode.InternalServerError)
            {
                UnlockLogin("An unexpected error occurred");
                return;
            }

            string body = await authResponse.Content.ReadAsStringAsync();
            AuthResponse authData = JsonConvert.DeserializeObject<AuthResponse>(body);
            if(authData.GetStatus() != 200)
            {
                UnlockLogin(authData.GetMessage());
                return;
            }

            Session session = await Session.CreateAsync(authData.GetData());
            if(session == null)
            {
                UnlockLogin("Session is null");
                return;
            }

            UserUI userUI = new UserUI();
            userUI.SetSession(session);

            MainWindow.Instance.Content = userUI;
        }

        private void LockLogin()
        {
            loginButton.IsEnabled = false;
            usernameBox.IsEnabled = false;
            passwordBox.IsEnabled = false;

            loginRing.Foreground = Brushes.White;
            loginErrorText.Foreground = Brushes.Transparent;
        }

        private void UnlockLogin(string error)
        {
            loginButton.IsEnabled = true;
            usernameBox.IsEnabled = true;
            passwordBox.IsEnabled = true;

            loginErrorText.Text = error;
            loginRing.Foreground = Brushes.Transparent;
            loginErrorText.Foreground = Brushes.PaleVioletRed;
        }

        private bool IsFieldsEmpty()
        {
            LockLogin();
            if (string.IsNullOrWhiteSpace(usernameBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text))
            {
                UnlockLogin("Username or password may not be empty");
                return true;
            }

            return false;
        }
    }
}