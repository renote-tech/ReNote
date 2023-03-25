using Avalonia.Controls;
using Avalonia.Input;
using Client.Api;
using Client.ReNote;
using Client.Windows;

namespace Client.Layout
{
    internal partial class UserUI : UserControl
    {
        public Session Session { get; private set; }

        public UserUI()
        {
            InitializeComponent();
            Initialized += (sender, e) =>
            {
                SessionText.Text = $"{ReNote.Client.Instance.SchoolInformation.GetSchoolName()}\n{Session.RealName}";
            };
        }

        public void SetSession(Session session)
        {
            Session ??= session;
        }

        private async void OnLogoutButtonClicked(object sender, PointerReleasedEventArgs e)
        {
            await ApiClient.SendLogoutAsync(Session.SessionId, Session.AuthToken);
            MainWindow.Instance.SetContent(new LogonUI());
        }

        private void OnSwitchMenuVisibility(object sender, PointerReleasedEventArgs e)
        {
            MenuSelector.IsVisible = !MenuSelector.IsVisible;
        }
    }
}