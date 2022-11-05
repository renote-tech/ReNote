namespace Server.ReNote
{
    public class School
    {
        public string SchoolName { get; set; }
        public SchoolType SchoolType { get; set; }
        public string SchoolLocation { get; set; }
    }

    public enum SchoolType
    {
        HIGH_SCHOOL   = 0,
        MIDDLE_SCHOOL = 1
    }
}