using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.Common;
using Server.Web.Interfaces;

namespace Server.Web.Socket
{
    public class SocketInterface : IListener
    {
        /// <summary>
        /// The current existing instance of the <see cref="SocketInterface"/> class; creates a new one if <see cref="m_Instance"/> is null.
        /// </summary>
        public static SocketInterface Instance
        {
            get
            {
                m_Instance ??= new SocketInterface();
                return m_Instance;
            }
        }

        /// <summary>
        /// The <see cref="TcpListener"/>.
        /// </summary>
        public TcpListener Listener { get; private set; }

        /// <summary>
        /// True if the <see cref="SocketInterface"/>'s instance is running; otherwise false.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Thre if the <see cref="SocketInterface"/>'s instance is disposed; otherwise false.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The private field of the <see cref="Instance"/> field.
        /// </summary>
        private static SocketInterface m_Instance;

        /// <summary>
        /// The request handler <see cref="Thread"/>.
        /// </summary>
        private Thread m_RequestHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SocketInterface()
        {
            if (Configuration.GlobalConfig.SocketPort == 0)
                Configuration.GlobalConfig.SocketPort = 7201;

            Listener = new TcpListener(IPAddress.Any, Configuration.GlobalConfig.SocketPort);
            m_RequestHandler = new Thread(SocketHandler.Handle);

            Platform.Log($"Initialized IListener; PORT={Configuration.GlobalConfig.SocketPort}", LogLevel.INFO);
        }

        /// <summary>
        /// Starts the <see cref="Listener"/> and <see cref="m_RequestHandler"/> <see cref="Thread"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="SocketInterface"/>'s instance is disposed.</exception>
        public void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("SocketInterface");

            if (IsRunning)
                return;

            IsRunning = true;

            Listener.Start();
            m_RequestHandler.Start();
        }

        /// <summary>
        /// Stops and disposes the <see cref="SocketInterface"/>'s instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Throws an exception if the <see cref="SocketInterface"/>'s instance is disposed.</exception>
        public void End()
        {
            Platform.Log("Stopping SocketInterface Service", LogLevel.INFO);

            if (IsDisposed)
                throw new ObjectDisposedException("SocketInterface");

            if (!IsRunning)
                return;

            IsRunning = false;
            IsDisposed = true;

            Listener.Stop();
            m_RequestHandler = null;
        }
    }
}