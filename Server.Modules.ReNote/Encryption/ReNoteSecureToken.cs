using Server.Common.Encryption;
using Server.Common.Utilities;

namespace Server.ReNote.Encryption
{
    public class ReNoteSecureToken
    {
        /// <summary>
        /// The token pattern.
        /// </summary>
        public const string BASE_PATTERN = "rst";

        /// <summary>
        /// Returns a ReNote secure token.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="string"/></returns>
        public static string Generate(long sessionId)
        {
            string tokenContent = $"{sessionId}-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-{EncryptionUtil.RandomTokenSalt()}";
            byte[] tokenHash = EncryptionUtil.ComputeSha256(tokenContent);
            string tokenBas64 = Base64Url.Encode(tokenHash);

            return $"{BASE_PATTERN}.{tokenBas64}";
        }
    }
}