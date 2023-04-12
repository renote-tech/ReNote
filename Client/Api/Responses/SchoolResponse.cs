using Client.ReNote;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class SchoolResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        private School m_Data;

        public void SetData(School data)
        {
            m_Data = data;
        }

        public School GetData()
        {
            return m_Data;
        }
    }
}