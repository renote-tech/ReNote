using Client.ReNote;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class ThemeResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        private Theme[] m_Data;

        public void SetData(Theme[] data)
        {
            m_Data = data;
        }

        public Theme[] GetData()
        {
            return m_Data;
        }
    }
}