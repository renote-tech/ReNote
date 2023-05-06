using System;
using System.Net;
using System.Threading;
using Server.Common;
using Server.Common.Exceptions;
using Server.Web.Interfaces;
using Server.Web.Helpers;

namespace Server.Web.Api
{
    public class ApiInterface : IHttpListener
    {
        /// <summary>
        /// The current existing instance of the <see cref="ApiInterface"/> class; creates a new one if <see cref="m_Instance"/> is null.
        /// </summary>
        public static ApiInterface Instance
        {
            get
            {
                m_Instance ??= new ApiInterface();
                return m_Instance;
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
        private static ApiInterface m_Instance;

        /// <summary>
        /// True if the <see cref="ApiInterface"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool m_Initialized;

        /// <summary>
        /// The request handler <see cref="Thread"/>.
        /// </summary>
        private Thread m_RequestHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiInterface()
        {
            Listener = new HttpListener();
            m_RequestHandler = new Thread(() => ApiHandler.Handle().Wait());
        }

        /// <summary>
        /// Initializes the <see cref="ApiInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
        public void Initialize()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (m_Initialized)
                return;

            if (Configuration.GlobalConfig.ApiPort == 0)
                Configuration.GlobalConfig.ApiPort = 7101;

            HttpHelper.RegisterPrefixes(this, Configuration.GlobalConfig.Prefixes, Configuration.GlobalConfig.ApiPort);
            m_Initialized = true;
        }

        /// <summary>
        /// Starts the <see cref="Listener"/> and <see cref="m_RequestHandler"/> <see cref="Thread"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is not initialized.</exception>
        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (!m_Initialized)
                throw new UninitializedException("ApiInterface", "Start");

            if (IsRunning)
                return;

            IsRunning = true;

            Listener.Start();
            m_RequestHandler.Start();
        }

        /// <summary>
        /// Stops and disposes the <see cref="ApiInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is disposed.</exception>
        /// <exception cref="UninitializedException">Throws an exception if the <see cref="ApiInterface"/>'s instance is not initialized.</exception>
        public void End()
        {
            Platform.Log("Stopping ApiInterface Service", LogLevel.INFO);

            if (IsDisposed)
                throw new ObjectDisposedException("ApiInterface");

            if (!m_Initialized)
                throw new UninitializedException("ApiInterface", "Stop");

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