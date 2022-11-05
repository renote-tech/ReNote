using System.Dynamic;
using System.Net;
using System.Net.Http;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

using Client.Api;
using Client.Utilities;

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
            LockLogin();

            if (string.IsNullOrWhiteSpace(usernameBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text))
            {
                UnlockLogin("Username or password may not be empty");
                return;
            }

            HttpResponseMessage response = await ApiClient.Instance.SendAuth(usernameBox.Text, passwordBox.Text);
            if(response.StatusCode == HttpStatusCode.InternalServerError)
            {
                UnlockLogin("An unexpected error occurred");
                return;
            }

            string body = await response.Content.ReadAsStringAsync();
            dynamic deserializedBody = JsonUtil.DeserializeAsDynamic(body);

            if(deserializedBody.status != 200)
            {
                UnlockLogin(deserializedBody.message);
                return;
            }
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
    }
}