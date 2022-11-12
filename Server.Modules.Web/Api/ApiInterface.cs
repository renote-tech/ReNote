using System.Net;
using Server.Common.Exceptions;
using Server.Common.Utilities;
using Server.Web.Interfaces;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    public class ApiInterface : IHttpListener
    {
        /// <summary>
        /// The current existing instance of the <see cref="ApiInterface"/> class; creates a new one if <see cref="instance"/> is null.
        /// </summary>
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

        /// <summary>
        /// The <see cref="HttpListener"/>.
        /// </summary>
        public HttpListener Listener { get; private set; }
        /// <summary>
        /// True if the <see cref="ApiInterface"/>'s instance is running; otherwise false.
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// True if the <see cref="ApiInterface"/>'s instance is disposed; otherwise false.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The private instance of the <see cref="Instance"/> field.
        /// </summary>
        private static ApiInterface instance;
        /// <summary>
        /// True if the <see cref="ApiInterface"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool initialized;
        /// <summary>
        /// The request handler <see cref="Thread"/>.
        /// </summary>
        private Thread requestHandler;

        public ApiInterface()
        {
            Listener = new HttpListener();
            requestHandler = new Thread(ApiHandler.Handle);
        }

        /// <summary>
        /// Initializes the <see cref="ApiInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
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

        /// <summary>
        /// Starts the <see cref="Listener"/> and <see cref="requestHandler"/> <see cref="Thread"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is not initialized.</exception>
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

        /// <summary>
        /// Stops and disposes the <see cref="ApiInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is not initialized.</exception>
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