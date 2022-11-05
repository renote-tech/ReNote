using Server.Common.Utilities;
using Timer = System.Timers.Timer;

namespace Server.ReNote
{
    public class ReNoteApp
    {
        public static ReNoteApp Instance
        {
            get
            {
                instance ??= new ReNoteApp();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        public School School { get; set; }
        public Database SchoolDB { get; private set; }
        public Timer SaveDBTimer { get; private set; }
        public string SaveDBLocation { get; set; }

        private static ReNoteApp instance;

        public ReNoteApp()
        {
            SchoolDB = new Database();
            SaveDBTimer = new Timer(15000);
            SaveDBLocation = "databases/school.dat";
        }

        public void Initialize()
        {
            SchoolDB.Load(SaveDBLocation);

            SaveDBTimer.Elapsed += (sender, e) =>
            {
                if (!FileUtil.IsFileBeingUsed(SaveDBLocation))
                    SchoolDB.Save(SaveDBLocation);
            };

            SaveDBTimer.AutoReset = true;
            SaveDBTimer.Enabled = true;
        }

        public void SetDBSaveInterval(double milliseconds)
        {
            if(SaveDBTimer != null)
                SaveDBTimer.Interval = milliseconds;
        }
    }
}