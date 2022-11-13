using Newtonsoft.Json;
using Server.Common.Encryption;
using Server.Common.Utilities;
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
            User user = new User();
            user.RealName = "Alian DEAD";
            user.Username = "adead99";
            user.Email = "";
            user.AccountType = 1;
            user.UserId = 256;
            user.SecurePassword = aesObject.Data;
            user.IVPassword = aesObject.IV;

            Database.Instance.Clear();
            DatabaseUtil.Set("users", "256", JsonConvert.SerializeObject(user));
            /* End Test */

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }
    }
}