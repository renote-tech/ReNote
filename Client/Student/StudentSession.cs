using Client.ReNote;

namespace Client.Student
{
    internal class StudentSession
    {
        public long Id { get; set; }
        public string HighSchoolName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }

        public static StudentSession Create(GlobalSession gSession)
        {
            return new StudentSession(gSession.SessionId);
        }

        public StudentSession()
        {
            
        }

        public StudentSession(long studentId)
        {
            Id = studentId;
        }
    }
}