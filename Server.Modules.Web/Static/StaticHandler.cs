using System.Net;

namespace Server.Web.Static
{
    internal class StaticHandler
    {
        /// <summary>
        /// Handles an incoming web request.
        /// </summary>
        public static async void Handle()
        {
            while (StaticInterface.Instance.IsRunning)
            {
                if (StaticInterface.Instance.IsDisposed)
                    return;

                HttpListener listener = StaticInterface.Instance.Listener;
                HttpListenerContext webContext = listener.GetContext();

                if (webContext.Request.RawUrl == null)
                    return;

                byte[] webResponse = await StaticData.GetResourceAsync(webContext.Request.RawUrl);

                webContext.Request.InputStream.Close();
                webContext.Response.OutputStream.Write(webResponse, 0, webResponse.Length);
                webContext.Response.KeepAlive = false;
                webContext.Response.OutputStream.Close();
                webContext.Response.Close();
            }
        }
    }
}