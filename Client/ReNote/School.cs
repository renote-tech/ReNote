using Newtonsoft.Json;

namespace Client.ReNote
{
    internal class School
    {
        [JsonProperty("schoolName")]
        private string m_SchoolName;
        
        [JsonProperty("schoolType")]
        private SchoolType m_SchoolType;

        [JsonProperty("schoolLocation")]
        private string m_SchoolLocation;

        public void SetSchoolName(string schoolName)
        {
            m_SchoolName = schoolName;
        }

        public void SetSchoolType(SchoolType schoolType)
        {
            m_SchoolType = schoolType;
        }

        public void SetSchoolLocation(string schoolLocation) 
        {
            m_SchoolLocation = schoolLocation;
        }

        public string GetSchoolName()
        {
            return m_SchoolName;
        }

        public SchoolType GetSchoolType()
        {
            return m_SchoolType;
        }

        public string GetSchoolLocation()
        {
            return m_SchoolLocation;
        }
    }

    public enum SchoolType
    {
        HIGH_SCHOOL    = 0,
        MIDDLE_SCHOOL  = 1,
        PRIMARY_SCHOOL = 2
    }
}