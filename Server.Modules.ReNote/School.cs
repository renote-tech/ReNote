using Newtonsoft.Json;

namespace Server.ReNote
{
    public class School
    {
        /// <summary>
        /// The school name.
        /// </summary>
        [JsonProperty("schoolName")]
        public string SchoolName { get; set; }
        /// <summary>
        /// The school type.
        /// </summary>
        [JsonProperty("schoolType")]
        public SchoolType SchoolType { get; set; }
        /// <summary>
        /// The school geo location.
        /// </summary>
        [JsonProperty("schoolLocation")]
        public string SchoolLocation { get; set; }
    }

    public enum SchoolType
    {
        HIGH_SCHOOL   = 0,
        MIDDLE_SCHOOL = 1
    }
}