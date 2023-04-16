using Server.Common;
using Server.ReNote.Data;
using Server.ReNote.Management;

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
        /// The server worker timer.
        /// </summary>
        public System.Timers.Timer ServerWorker { get; private set; }

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
            ServerWorker = new System.Timers.Timer(Constants.WORKER_INTERVAL);
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

            dbInstance.FileLocation = Configuration.ReNoteConfig.DBLocation;
            bool isLoaded = dbInstance.Load();
            if (!isLoaded)
                Platform.Log("Coudln't load the database", LogLevel.WARN);

            ServerWorker.Elapsed += OnWorkerServiceInvoked;
            ServerWorker.AutoReset = true;
            ServerWorker.Enabled   = true;

            Platform.Log("Initialized ReNote Server", LogLevel.INFO);

            initialized = true;
        }

        /// <summary>
        /// Occurs when the worker interval has elapsed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnWorkerServiceInvoked(object sender, System.Timers.ElapsedEventArgs e)
        {
            Platform.Log("[Worker] Cleaning and saving data", LogLevel.INFO);

            SessionManager.Clean();
            await dbInstance.SaveAsync(true);
        }
    }
}