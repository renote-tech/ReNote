using Server.Common;
using Server.Web.Api;
using Server.Web.Static;

namespace Server
{
    internal class Program
    {
        /// <summary>
        /// Entry point of the application; 
        /// loads main components and configurations.
        /// </summary>
        static void Main()
        {
            Platform.Initialize();
            Configuration.LoadAllConfigurations();

            InitializeProcessEvents();  

            StaticInterface.Instance.Initialize();
            ApiInterface.Instance.Initialize();
            ApiRegisterer.Initialize();

            ReNote.Server.Instance.Initialize();

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }

        static void InitializeProcessEvents()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                Platform.Log($"{e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
            };

            AppDomain.CurrentDomain.ProcessExit += async (sender, e) =>
            {
                await ReNote.Server.Database.SaveAsync();
            };
        }
    }
}