using System;
using System.Net;
using System.Threading;
using Server.Common;
using Server.Common.Exceptions;
using Server.Web.Interfaces;
using Server.Web.Helpers;

namespace Server.Web.Static
{
    public class StaticInterface : IHttpListener
    {
        /// <summary>
        /// The current existing instance of the <see cref="StaticInterface"/> class; creates a new one if <see cref="s_Instance"/> is null.
        /// </summary>
        public static StaticInterface Instance
        {
            get
            {
                s_Instance ??= new StaticInterface();
                return s_Instance;
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
        private static StaticInterface s_Instance;
        
        /// <summary>
        /// True if the <see cref="StaticInterface"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool m_Initialized;
        
        /// <summary>
        /// The request handler <see cref="Thread"/>.
        /// </summary>
        private Thread m_RequestHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StaticInterface()
        {
            Listener = new HttpListener();
            m_RequestHandler = new Thread(() => StaticHandler.Handle().Wait());
        }

        /// <summary>
        /// Initializes the <see cref="StaticInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
        public void Initialize()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (m_Initialized)
                return;

            if (Configuration.GlobalConfig.WebPort == 0)
                Configuration.GlobalConfig.WebPort = 7001;

            HttpHelper.RegisterPrefixes(this, Configuration.GlobalConfig.Prefixes, Configuration.GlobalConfig.WebPort);
            m_Initialized = true;
        }

        /// <summary>
        /// Starts the <see cref="Listener"/> and <see cref="m_RequestHandler"/> <see cref="Thread"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is not initialized.</exception>
        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (!m_Initialized)
                throw new UninitializedException("StaticInterface", "Start");

            if (IsRunning)
                return;

            IsRunning = true;

            Listener.Start();
            m_RequestHandler.Start();
        }

        /// <summary>
        /// Stops and disposes the <see cref="StaticInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="StaticInterface"/>'s instance is not initialized.</exception>
        public void End()
        {
            Platform.Log("Stopping StaticInterface Service", LogLevel.INFO);

            if (IsDisposed)
                throw new ObjectDisposedException("StaticInterface");

            if (!m_Initialized)
                throw new UninitializedException("StaticInterface", "Stop");

            if (!IsRunning)
                return;

            m_Initialized = false;
            IsRunning = false;
            IsDisposed = true;

            Listener.Stop();
            Listener.Close();
            m_RequestHandler = null;
        }
    }
}