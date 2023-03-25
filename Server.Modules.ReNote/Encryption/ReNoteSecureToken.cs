using Server.Common.Encryption;
using Server.Common.Utilities;

namespace Server.ReNote.Encryption
{
    public class ReNoteSecureToken
    {
        /// <summary>
        /// Returns a ReNote secure token.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="string"/></returns>
        public static async Task<string> GenerateAsync(long sessionId)
        {
            string tokenContent = $"{sessionId}-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-{EncryptionUtil.RandomTokenSalt()}";
            byte[] tokenHash    = await EncryptionUtil.ComputeSha256Async(tokenContent);
            string tokenBas64   = Base64Url.Encode(tokenHash, false);

            return $"{Constants.TOKEN_BASE_PATTERN}.{tokenBas64}";
        }
    }
}