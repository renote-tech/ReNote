using Newtonsoft.Json;

namespace Client.ReNote
{
    internal class School
    {
        [JsonProperty("schoolName")]
        public string SchoolName { get; set; }
        
        [JsonProperty("schoolType")]
        public SchoolType SchoolType { get; set; }

        [JsonProperty("schoolLocation")]
        public string SchoolLocation { get; set; }
    }

    public enum SchoolType
    {
        HIGH_SCHOOL    = 0,
        MIDDLE_SCHOOL  = 1,
        PRIMARY_SCHOOL = 2
    }
}