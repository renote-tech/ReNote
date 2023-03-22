using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class ProfileResponse : BaseResponse
    {
        [JsonProperty("data", Order = 10)]
        private ProfileData m_Data;

        public void SetData(ProfileData data)
        {
            m_Data = data;
        }

        public ProfileData GetData()
        {
            return m_Data;
        }
    }

    internal class ProfileData
    {
        [JsonProperty("realName")]
        private string m_RealName;

        [JsonProperty("profilePicture")]
        private string m_ProfilePicture;

        [JsonProperty("email")]
        private string m_Email;

        [JsonProperty("phone")]
        private string m_PhoneNumber;

        [JsonProperty("birthday")]
        private string m_Birthday;

        [JsonProperty("lastConnection")]
        private long m_LastConnection;

        public void SetRealName(string realName)
        {
            m_RealName = realName;
        }

        public void SetProfilePicture(string profilePicture)
        {
            m_ProfilePicture = profilePicture;
        }

        public void SetEmail(string email)
        {
            m_Email = email;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            m_PhoneNumber = phoneNumber;
        }

        
        public void SetBirthday(string birthday)
        {
            m_Birthday = birthday;
        }

        public void SetLastConnection(long lastConnection)
        {
            m_LastConnection = lastConnection;
        }

        public string GetRealName()
        {
            return m_RealName;
        }

        public string GetProfilePicture()
        {
            return m_ProfilePicture; 
        }

        public string GetEmail()
        {
            return m_Email;
        }

        public string GetPhoneNumber()
        {
            return m_PhoneNumber;
        }

        public string GetBirthday()
        {
            return m_Birthday;
        }

        public long GetLastConnection()
        {
            return m_LastConnection;
        }
    }
}