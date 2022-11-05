namespace Server.Web.Interfaces
{
    internal interface IListener
    {
        bool IsRunning { get; }
        bool IsDisposed { get; }
        void Start();
        void End();
    }
}