using Avalonia.Controls;
using Avalonia.Input;
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
                if (Session == null)
                    return;

                SessionText.Text = $"{ReNote.Client.Instance.SchoolInformation.GetSchoolName()}\n{Session.RealName}";
            };
        }

        public void SetSession(Session session)
        {
            Session ??= session;
        }

        private void OnLogoutButtonClicked(object sender, PointerReleasedEventArgs e)
        {
            MainWindow.Instance.SetContent(new LogonUI());
        }

        private void OnSwitchMenuVisibility(object sender, PointerReleasedEventArgs e)
        {
            MenuSelector.IsVisible = !MenuSelector.IsVisible;
        }
    }
}