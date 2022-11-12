using Server.ReNote.Data;

namespace Server.ReNote
{
    public class SReNote
    {
        /// <summary>
        /// The saving database interval.
        /// </summary>
        public const int SAVE_DB_INTERVAL = 120000;

        /// <summary>
        /// The current instance of the <see cref="SReNote"/> class; creates a new one if <see cref="instance"/> is null.
        /// </summary>
        public static SReNote Instance
        {
            get
            {
                instance ??= new SReNote();
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
        private static SReNote instance;
        /// <summary>
        /// True if the <see cref="SReNote"/>'s instance is initialized; otherwise false.
        /// </summary>
        private bool initialized;

        public SReNote()
        {
            DatabaseTimer = new System.Timers.Timer(SAVE_DB_INTERVAL);
        }

        /// <summary>
        /// Initializes the <see cref="SReNote"/>'s instance.
        /// </summary>
        public void Initialize()
        {
            if (initialized)
                return;

            Database.Instance.SaveLocation = "school.dat";
            Database.Instance.Load();

            DatabaseTimer.Elapsed += (sender, e) => Database.Instance.Save();
            DatabaseTimer.AutoReset = true;
            DatabaseTimer.Enabled = true;

            initialized = true;
        }
    }
}