using Client.ReNote;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class TeamProfileResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        private Team m_Data;

        public void SetData(Team data)
        {
            m_Data = data;
        }

        public Team GetData()
        {
            return m_Data;
        }
    }
}