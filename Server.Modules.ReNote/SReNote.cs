using Server.ReNote.Data;

namespace Server.ReNote
{
    public class SReNote
    {
        public const int SAVE_DB_INTERVAL = 120000;

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
        public School SchoolInformation { get; set; }
        public System.Timers.Timer DatabaseTimer { get; private set; }

        private static SReNote instance;

        public SReNote()
        {
            DatabaseTimer = new System.Timers.Timer(SAVE_DB_INTERVAL);
        }

        public void Initialize()
        {
            Database.Instance.SaveLocation = "school.dat";
            Database.Instance.Load();

            DatabaseTimer.Elapsed += (sender, e) => Database.Instance.Save();
            DatabaseTimer.AutoReset = true;
            DatabaseTimer.Enabled = true;
        }
    }
}