using System.Net;

namespace Server.Web.Interfaces
{
    internal interface IHttpListener : IListener
    {   
        /// <summary>
        /// The <see cref="HttpListener"/>.
        /// </summary>
        HttpListener Listener { get; }
        /// <summary>
        /// Initializes the <see cref="IHttpListener"/>'s instance.
        /// </summary>
        void Initialize();
    }
}