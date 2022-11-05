using System.Net;

namespace Server.Web.Interfaces
{
    internal interface IHttpListener : IListener
    {
        HttpListener Listener { get; }
        void Initialize();
    }
}