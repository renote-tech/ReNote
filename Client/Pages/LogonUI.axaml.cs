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

namespace Client.Pages
{
    public partial class LogonUI : UserControl
    {
        public LogonUI()
        {
            InitializeComponent();
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
            if(authData.Status != 200)
            {
                UnlockLogin(authData.Message);
                return;
            }

            Session session = await Session.CreateAsync(authData.Data);
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

            loginErrorText.Foreground = Brushes.Transparent;
        }

        private void UnlockLogin(string error)
        {
            loginButton.IsEnabled = true;
            usernameBox.IsEnabled = true;
            passwordBox.IsEnabled = true;

            loginErrorText.Text = error;
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