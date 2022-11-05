using Server.Web.Api;
using Server.Web.Static;
using Server.Common.Utilities;
using Server.ReNote;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Platform.Initialize();
            Configuration.LoadAllConfigurations();

            StaticInterface.Instance.Initialize();
            ApiInterface.Instance.Initialize();
            ApiRegisterer.Initialize();

            ReNoteApp.Instance.Initialize();

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }
    }
}