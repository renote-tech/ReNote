using System.Net;
using System.Net.Sockets;
using Server.Common.Utilities;
using Server.Web.Interfaces;

namespace Server.Web.Socket
{
    public class SocketInterface : IListener
    {
        public static SocketInterface Instance
        {
            get
            {
                instance ??= new SocketInterface();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public TcpListener Listener { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsDisposed { get; private set; }

        private static SocketInterface instance;
        private Thread requestHandler;

        public SocketInterface()
        {
            if (Configuration.GlobalConfig.SocketPort == 0)
                Configuration.GlobalConfig.SocketPort = 7201;

            Listener = new TcpListener(IPAddress.Any, Configuration.GlobalConfig.SocketPort);
            requestHandler = new Thread(SocketHandler.Handle);

            Platform.Log($"Initialized IListener; PORT={Configuration.GlobalConfig.SocketPort}", LogLevel.INFO);
        }

        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (IsRunning)
                return;

            IsRunning = true;

            Listener.Start();
            requestHandler.Start();
        }

        public void End()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (!IsRunning)
                return;

            IsRunning = false;
            IsDisposed = true;

            Listener.Stop();
            requestHandler = null;
        }
    }
}