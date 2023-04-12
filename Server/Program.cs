using Newtonsoft.Json;
using Server.Common;
using Server.Common.Encryption;
using Server.ReNote;
using Server.ReNote.Data;
using Server.ReNote.Utilities;
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

            AESObject aesObject = AES.Encrypt("password");
            User user = new User()
            {
                RealName = "Alian DEAD",
                Username = "adead99",
                Email = "",
                AccountType = 1,
                TeamId = 16,
                UserId = 256,
                SecurePassword = aesObject.Data,
                IVPassword = aesObject.IV
            };

            DatabaseUtil.Set("users", "256", user, user.Username);
            Task.Run(async () => await ReNote.Server.Database.SaveAsync());

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }

        /// <summary>
        /// Register Application Domain events.
        /// </summary>
        static void InitializeProcessEvents()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                Platform.Log($"{e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
            };

            Console.CancelKeyPress += (sender, e) => EndService();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => EndService();
        }

        static async void EndService()
        {
            Platform.Log("Saving data and closing server", LogLevel.INFO);

            ReNote.Server.Database.ClearContainerContent(Constants.DB_ROOT_SESSIONS);
            await ReNote.Server.Database.SaveAsync();

            Environment.Exit(0);
        }
    }
}