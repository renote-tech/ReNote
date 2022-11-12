namespace Server.Web.Interfaces
{
    internal interface IListener
    {
        /// <summary>
        /// True if the <see cref="IListener"/>'s instance is running; otherwise false.
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// True if the <see cref="IListener"/>'s instance is disposed; otherwise false.
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// Starts the <see cref="IListener"/>'s instance.
        /// </summary>
        void Start();
        /// <summary>
        /// Ends the <see cref="IListener"/>'s instance.
        /// </summary>
        void End();
    }
}