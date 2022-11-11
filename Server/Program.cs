using Newtonsoft.Json;
using Server.Common.Encryption;
using Server.Common.Utilities;
using Server.ReNote;
using Server.ReNote.Data;
using Server.Web.Api;
using Server.Web.Static;

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

            SReNote.Instance.Initialize();

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
            Database.Instance.AddDocument("sessions");
            Database.Instance.AddDocument("users");
            Database.Instance["users"].AddKey("256", new Document(JsonConvert.SerializeObject(user)));
            Database.Instance.Save();

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }
    }
}