using System.Net;
using Server.Common;
using Server.Common.Exceptions;
using Server.Web.Interfaces;
using Server.Web.Utilities;

namespace Server.Web.Static
{
    public class StaticInterface : IHttpListener
    {
        /// <summary>
        /// The current existing instance of the <see cref="StaticInterface"/> class; creates a new one if <see cref="instance"/> is null.
        /// </summary>
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

        /// <summary>
        /// The <see cref="HttpListener"/>.
        /// </summary>
        public HttpListener Listener { get; private set; }
        /// <summary>
        /// True if the <see cref="StaticInterface"/>'s instance is running; otherwise false.
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// True if the <see cref="StaticInterface"/>'s instance is disposed; otherwise false.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The private instance of the <see cref="Instance"/> field.
        /// </summary>
        private static StaticInterface instance;
        /// <summary>
        /// True if the <see cref="StaticInterface"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool initialized;
        /// <summary>
        /// The request handler <see cref="Thread"/>.
        /// </summary>
        private Thread requestHandler;

        public StaticInterface()
        {
            Listener = new HttpListener();
            requestHandler = new Thread(() => StaticHandler.Handle().Wait());
        }

        /// <summary>
        /// Initializes the <see cref="StaticInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
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

        /// <summary>
        /// Starts the <see cref="Listener"/> and <see cref="requestHandler"/> <see cref="Thread"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is not initialized.</exception>
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

        /// <summary>
        /// Stops and disposes the <see cref="StaticInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is not initialized.</exception>
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