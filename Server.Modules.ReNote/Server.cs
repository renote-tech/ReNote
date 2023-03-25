using Server.Common;
using Server.ReNote.Data;

namespace Server.ReNote
{
    public class Server
    {
        /// <summary>
        /// The current instance of the <see cref="Server"/> class; creates a new one if <see cref="instance"/> is null.
        /// </summary>
        public static Server Instance
        {
            get
            {
                instance ??= new Server();
                return instance;
            }
        }

        /// <summary>
        /// The current instance of the <see cref="Database"/> class.
        /// </summary>
        public static Database Database
        {
            get
            {
                return dbInstance;
            }
        }

        /// <summary>
        /// The school information.
        /// </summary>
        public School SchoolInformation { get; set; }
        /// <summary>
        /// The saving database timer.
        /// </summary>
        public System.Timers.Timer DatabaseTimer { get; private set; }

        /// <summary>
        /// The private instance of the <see cref="Instance"/> field.
        /// </summary>
        private static Server instance;
        /// <summary>
        /// The private <see cref="Database"/> instance.
        /// </summary>
        private static Database dbInstance;
        /// <summary>
        /// True if the <see cref="Server"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool initialized;

        public Server()
        {
            DatabaseTimer = new System.Timers.Timer(Constants.DB_SAVE_INTERVAL);
        }

        /// <summary>
        /// Initializes the <see cref="Server"/>'s instance.
        /// </summary>
        public void Initialize()
        {
            if (initialized)
                return;

            dbInstance = new Database();

            SchoolInformation = new School()
            {
                SchoolLocation = Configuration.ReNoteConfig.SchoolLocation,
                SchoolName     = Configuration.ReNoteConfig.SchoolName,
                SchoolType     = (SchoolType)Configuration.ReNoteConfig.SchoolType
            };

            dbInstance.SaveLocation = Configuration.ReNoteConfig.DBSaveLocation;
            bool isLoaded = dbInstance.Load();
            if (!isLoaded)
                Platform.Log("Coudln't load the database", LogLevel.WARN);

            DatabaseTimer.Elapsed  += async (sender, e) => await dbInstance.SaveAsync(true);
            DatabaseTimer.AutoReset = true;
            DatabaseTimer.Enabled   = true;

            Platform.Log("Initialized ReNote Server", LogLevel.INFO);

            initialized = true;
        }
    }
}