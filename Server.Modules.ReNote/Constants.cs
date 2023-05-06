using Server.ReNote.Data;
using Server.ReNote.Encryption;
using Server.ReNote.Management;

namespace Server.ReNote
{
    public class Constants
    {
        /// <summary>
        /// The length of the <see cref="Session.SessionId"/>.
        /// </summary>
        public const int SID_LENGTH = 9;
        /// <summary>
        /// The interval between each Worker Service call.
        /// </summary>
        public const int WORKER_INTERVAL = 300000;
        /// <summary>
        /// The shared ID for static file authorization.
        /// </summary>
        public const int SHARED_AUTH_ID = 128;
        /// <summary>
        /// The public ID for static file authorization.
        /// </summary>
        public const int PUBLIC_AUTH_ID = 0;

        /// <summary>
        /// The name of the <see cref="Container"/> for storing the <see cref="User"/>s' data.
        /// </summary>
        public const string DB_ROOT_USERS = "users";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing the <see cref="Session"/>s' data.
        /// </summary>
        public const string DB_ROOT_SESSIONS = "sessions";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing a list of quotations.
        /// </summary>
        public const string DB_ROOT_QUOTATIONS = "quotations";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing a list of themes.
        /// </summary>
        public const string DB_ROOT_COLOR_THEMES = "themes";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing the teams' data.
        /// </summary>
        public const string DB_ROOT_TEAMS = "teams";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing configurations.
        /// </summary>
        public const string DB_ROOT_CONFIGS = "configurations";
        /// <summary>
        /// The name of the <see cref="Container"/> for storing users' preferences.
        /// </summary>
        public const string DB_ROOT_PREFERENCES = "preferences";

        /// <summary>
        /// The <see cref="ReNoteToken"/> base token pattern.
        /// </summary>
        public const string TOKEN_BASE_PATTERN = "rst";
    }
}