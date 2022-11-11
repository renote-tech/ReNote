using Avalonia.Controls;
using Client.ReNote;
using Client.Student;

namespace Client.Pages
{
    internal partial class StudentUI : UserControl
    {
        public GlobalSession GlobalSession { get; set; }
        
        private StudentSession Session { get; set; }

        public StudentUI()
        {
            InitializeComponent();
            Initialized += (sender, e) =>
            {
                LoadInformation();
            };
        }

        public void SetSession(GlobalSession gSession)
        {
            GlobalSession = gSession;
            SetStudentSession();
        }

        private void LoadInformation()
        {
            SessionText.Text = $"{Session.HighSchoolName}\n{GlobalSession.UserName}";
        }

        private void SetStudentSession()
        {
            Session = StudentSession.Create(GlobalSession);
        }
    }
}
