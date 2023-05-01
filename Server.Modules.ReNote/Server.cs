using System.Threading.Tasks;
using System.Timers;
using Server.Common;
using Server.ReNote.Data;
using Server.ReNote.Management;

namespace Server.ReNote
{
    public class Server
    {
        /// <summary>
        /// The current instance of the <see cref="Server"/> class; creates a new one if <see cref="s_Instance"/> is null.
        /// </summary>
        public static Server Instance
        {
            get
            {
                s_Instance ??= new Server();
                return s_Instance;
            }
        }

        /// <summary>
        /// The current instance of the <see cref="Database"/> class.
        /// </summary>
        public static Database Database
        {
            get
            {
                return s_DatabaseInstance;
            }
        }

        /// <summary>
        /// The school information.
        /// </summary>
        public School SchoolInformation { get; set; }
        /// <summary>
        /// The server worker timer.
        /// </summary>
        public Timer ServerWorker { get; private set; }

        /// <summary>
        /// The private instance of the <see cref="Instance"/> field.
        /// </summary>
        private static Server s_Instance;
        /// <summary>
        /// The private <see cref="Database"/> instance.
        /// </summary>
        private static Database s_DatabaseInstance;
        /// <summary>
        /// True if the <see cref="Server"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool m_Initialized;

        public Server()
        {
            ServerWorker = new Timer(Constants.WORKER_INTERVAL);
        }

        /// <summary>
        /// Initializes the <see cref="Server"/>'s instance.
        /// </summary>
        public void Initialize()
        {
            if (m_Initialized)
                return;

            s_DatabaseInstance = new Database();

            SchoolInformation = new School()
            {
                SchoolLocation = Configuration.ReNoteConfig.SchoolLocation,
                SchoolName     = Configuration.ReNoteConfig.SchoolName,
                SchoolType     = (SchoolType)Configuration.ReNoteConfig.SchoolType
            };

            s_DatabaseInstance.FileLocation = Configuration.ReNoteConfig.DBLocation;
            bool loaded = s_DatabaseInstance.Load();
            if (!loaded)
                Platform.Log("Coudln't load the database", LogLevel.WARN);

            ServerWorker.Elapsed += OnWorkerServiceInvoked;
            ServerWorker.AutoReset = true;
            ServerWorker.Enabled   = true;

            Platform.Log("Initialized ReNote Server", LogLevel.INFO);

            m_Initialized = true;
        }

        /// <summary>
        /// Occurs when the worker interval has elapsed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnWorkerServiceInvoked(object sender, ElapsedEventArgs e)
        {
            Platform.Log("[Worker] Cleaning and saving data", LogLevel.INFO);

            SessionManager.Clean();
            await s_DatabaseInstance.SaveAsync(true);
        }
    }
}