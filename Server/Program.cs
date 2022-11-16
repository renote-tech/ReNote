using Newtonsoft.Json;
using Server.Common;
using Server.Common.Encryption;
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

            StaticInterface.Instance.Initialize();
            ApiInterface.Instance.Initialize();
            ApiRegisterer.Initialize();

            ReNote.Server.Instance.Initialize();

            /* Test Only */
            AESObject aesObject = AES.Encrypt("password");
            User user = new User()
            {
                RealName       = "Alian DEAD",
                Username       = "adead99",
                Email          = "",
                AccountType    = 1,
                UserId         = 256,
                SecurePassword = aesObject.Data,
                IVPassword     = aesObject.IV
            };

            Database.Instance.Clear();
            DatabaseUtil.Set("users", "256", JsonConvert.SerializeObject(user));
            /* End Test */

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }
    }
}