using Server.Common.Utilities;
using Server.ReNote.Data;

namespace Server.ReNote
{
    public class Server
    {
        /// <summary>
        /// The saving database interval.
        /// </summary>
        public const int SAVE_DB_INTERVAL = 120000;

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
            private set
            {
                instance = value;
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
        /// True if the <see cref="Server"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool initialized;

        public Server()
        {
            DatabaseTimer = new System.Timers.Timer(SAVE_DB_INTERVAL);
        }

        /// <summary>
        /// Initializes the <see cref="Server"/>'s instance.
        /// </summary>
        public void Initialize()
        {
            if (initialized)
                return;

            SchoolInformation = new School()
            {
                SchoolLocation = Configuration.ReNoteConfig.SchoolLocation,
                SchoolName     = Configuration.ReNoteConfig.SchoolName,
                SchoolType     = (SchoolType)Configuration.ReNoteConfig.SchoolType
            };

            Database.Instance.SaveLocation = Configuration.ReNoteConfig.DBSaveLocation;
            Database.Instance.Load();

            DatabaseTimer.Elapsed += async (sender, e) => await Database.Instance.SaveAsync();
            DatabaseTimer.AutoReset = true;
            DatabaseTimer.Enabled = true;

            initialized = true;
        }
    }
}