using Newtonsoft.Json;
using Server.Common;
using Server.Common.Helpers;
using Server.ReNote.Data;
using Server.ReNote.Helpers;
using Server.ReNote.Management;
using System.Collections.Generic;
using System.Timers;

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
        /// <summary>
        /// The current status of the server.
        /// </summary>
        private int m_CurrentStatus;

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

            ServerWorker = new Timer(Constants.WORKER_INTERVAL);

            ServerWorker.Elapsed += OnWorkerServiceInvoked;
            ServerWorker.AutoReset = true;
            ServerWorker.Enabled   = true;

            m_CurrentStatus = Constants.SERVER_STATUS_OK;

            string pluginsData = DatabaseHelper.Get(Constants.DB_ROOT_CONFIGS, "plugins");
            if (JsonHelper.ValiditateJson(pluginsData))
                PluginManager.Initialize(JsonConvert.DeserializeObject<Dictionary<string, bool>>(pluginsData));
            else
                PluginManager.Initialize(new Dictionary<string, bool>());

            Platform.Log("Initialized ReNote Server", LogLevel.INFO);
            
            m_Initialized = true;
        }

        /// <summary>
        /// Returns the current status of the server.
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public int CheckStatus()
        {
            return m_CurrentStatus;
        }

        /// <summary>
        /// Sets the current status of the server.
        /// </summary>
        /// <param name="statusCode">The new status code.</param>
        public void SetStatus(int statusCode)
        {
            m_CurrentStatus = statusCode;
        }

        /// <summary>
        /// Occurs when the worker interval has elapsed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnWorkerServiceInvoked(object sender, ElapsedEventArgs e)
        {
            Platform.Log("(Worker) Cleaning and saving data", LogLevel.INFO);

            SessionManager.Clean();

            await s_DatabaseInstance.SaveAsync(true);
            await s_DatabaseInstance.SaveAsync();
        }
    }
}