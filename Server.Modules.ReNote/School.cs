namespace Server.ReNote
{
    public class School
    {
        /// <summary>
        /// The school name.
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// The school type.
        /// </summary>
        public SchoolType SchoolType { get; set; }
        /// <summary>
        /// The school geo location.
        /// </summary>
        public string SchoolLocation { get; set; }
    }

    public enum SchoolType
    {
        HIGH_SCHOOL   = 0,
        MIDDLE_SCHOOL = 1
    }
}