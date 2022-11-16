using Server.ReNote.Data;
using Server.ReNote.Encryption;
using Server.ReNote.Management;

namespace Server.ReNote
{
    public class Constants
    {
        /// <summary>
        /// The length of the <see cref="GlobalSession.SessionId"/>.
        /// </summary>
        public const int SID_LENGTH = 9;
        /// <summary>
        /// The interval between each <see cref="Database.SaveAsync(bool)"/> call.
        /// </summary>
        public const int DB_SAVE_INTERVAL = 120000;

        /// <summary>
        /// The name of a <see cref="RootDocument"/> for storing <see cref="User"/>s' data.
        /// </summary>
        public const string DB_ROOT_USERS = "users";
        /// <summary>
        /// The name of a <see cref="RootDocument"/> for storing <see cref="GlobalSession"/>s' data.
        /// </summary>
        public const string DB_ROOT_SESSIONS = "sessions";
        /// <summary>
        /// The <see cref="ReNoteSecureToken"/> pattern.
        /// </summary>
        public const string TOKEN_BASE_PATTERN = "rst";
    }
}