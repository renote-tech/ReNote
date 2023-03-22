using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class BaseResponse
    {
        [JsonProperty("status")]
        private int m_Status;

        [JsonProperty("message")]
        private string m_Message;

        public void SetStatus(int status)
        {
            m_Status = status;
        }

        public int GetStatus() 
        {
            return m_Status; 
        }

        public void SetMessage(string message)
        {
            m_Message = message;
        }

        public string GetMessage()
        {
            return m_Message;
        }
    }
}