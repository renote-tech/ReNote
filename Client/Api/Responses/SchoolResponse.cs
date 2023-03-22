using Newtonsoft.Json;
using Client.ReNote;

namespace Client.Api.Responses
{
    internal class SchoolResponse : BaseResponse
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