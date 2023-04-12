namespace Server.Web.Socket
{
    internal class SocketHandler
    {
        /// <summary>
        /// Handles an incoming socket request.
        /// </summary>
        public static void Handle()
        {
            while (SocketInterface.Instance.IsRunning)
            { }
        }
    }
}