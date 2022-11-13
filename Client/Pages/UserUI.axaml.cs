using Avalonia.Controls;
using Client.ReNote;

namespace Client.Pages
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

                SessionText.Text = $"{ReNote.Client.Instance.SchoolInformation.SchoolName}\n{Session.RealName}";
            };
        }

        public void SetSession(Session session)
        {
            Session ??= session;
        }
    }
}