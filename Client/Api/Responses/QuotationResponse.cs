using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class QuotationResponse : BaseResponse
    {
        [JsonProperty("data", Order = 10)]
        private QuotationData m_Data;

        public void SetData(QuotationData data)
        {
            m_Data = data;
        }

        public QuotationData GetData()
        {
            return m_Data;
        }
    }

    internal class QuotationData
    {
        [JsonProperty("author")]
        private string m_Author;

        [JsonProperty("content")]
        private string m_Content;

        public void SetAuthor(string author)
        {
            m_Author = author;
        }

        public void SetContent(string content)
        {
            m_Content = content;
        }

        public string GetAuthor()
        {
            return m_Author;
        }

        public string GetContent()
        {
            return m_Content;
        }
    }
}