using System.Net;

using Server.Common.Utilities;
using Server.Common.Exceptions;
using Server.Web.Interfaces;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    public class ApiInterface : IHttpListener
    {
        public static ApiInterface Instance
        {
            get
            {
                instance ??= new ApiInterface();
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

        private static ApiInterface instance;
        private bool initialized;
        private Thread requestHandler;

        public ApiInterface()
        {
            Listener = new HttpListener();
            requestHandler = new Thread(ApiHandler.Handle);
        }

        public void Initialize()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (initialized)
                return;

            if (Configuration.GlobalConfig.ApiPort == 0)
                Configuration.GlobalConfig.ApiPort = 7101;

            HttpUtil.RegisterPrefixes(this, Configuration.GlobalConfig.Prefixes, Configuration.GlobalConfig.ApiPort);
            initialized = true;
        }

        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (!initialized)
                throw new UninitializedException("ApiInterface", "Start");

            if (IsRunning)
                return;

            IsRunning = true;

            Listener.Start();
            requestHandler.Start();
        }

        public void End()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (!initialized)
                throw new UninitializedException("ApiInterface", "Stop");

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