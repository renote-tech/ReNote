using System.Net;
using Server.Common.Exceptions;
using Server.Common.Utilities;
using Server.Web.Interfaces;
using Server.Web.Utilities;

namespace Server.Web.Static
{
    public class StaticInterface : IHttpListener
    {
        public static StaticInterface Instance
        {
            get
            {
                instance ??= new StaticInterface();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public HttpListener Listener { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsDisposed { get; private set; }

        private static StaticInterface instance;
        private bool initialized;
        private Thread requestHandler;

        public StaticInterface()
        {
            Listener = new HttpListener();
            requestHandler = new Thread(StaticHandler.Handle);
        }

        public void Initialize()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (initialized)
                return;

            if (Configuration.GlobalConfig.WebPort == 0)
                Configuration.GlobalConfig.WebPort = 7001;

            HttpUtil.RegisterPrefixes(this, Configuration.GlobalConfig.Prefixes, Configuration.GlobalConfig.WebPort);
            initialized = true;
        }

        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (!initialized)
                throw new UninitializedException("StaticInterface", "Start");

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


            if (!initialized)
                throw new UninitializedException("StaticInterface", "Stop");

            if (!IsRunning)
                return;

            IsRunning = false;
            initialized = false;
            IsDisposed = true;

            Listener.Stop();
            requestHandler = null;
        }
    }
}