using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class ProfileResponse : Response
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

        [JsonProperty("teamId")]
        private int m_TeamId;

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

        public string GetRealName()
        {
            return m_RealName;
        }

        public void SetTeamId(int teamId)
        {
            m_TeamId = teamId;
        }

        public int GetTeamId()
        {
            return m_TeamId;
        }

        public void SetProfilePicture(string profilePicture)
        {
            m_ProfilePicture = profilePicture;
        }

        public string GetProfilePicture()
        {
            return m_ProfilePicture;
        }

        public void SetEmail(string email)
        {
            m_Email = email;
        }

        public string GetEmail()
        {
            return m_Email;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            m_PhoneNumber = phoneNumber;
        }

        public string GetPhoneNumber()
        {
            return m_PhoneNumber;
        }

        public void SetBirthday(string birthday)
        {
            m_Birthday = birthday;
        }

        public string GetBirthday()
        {
            return m_Birthday;
        }

        public void SetLastConnection(long lastConnection)
        {
            m_LastConnection = lastConnection;
        }

        public long GetLastConnection()
        {
            return m_LastConnection;
        }
    }
}