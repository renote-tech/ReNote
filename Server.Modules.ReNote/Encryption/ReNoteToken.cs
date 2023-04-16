using System;
using System.Threading.Tasks;
using Server.Common.Encryption;

namespace Server.ReNote.Encryption
{
    public class ReNoteToken
    {
        /// <summary>
        /// Returns a ReNote secure token.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="string"/></returns>
        public static async Task<string> GenerateAsync(long sessionId)
        {
            string tokenContent = $"{sessionId}-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-{RandomTokenSalt(16)}";
            byte[] tokenHash    = await Sha256.ComputeAsync(tokenContent);
            string tokenBase64  = Base64.Encode(tokenHash, false);

            return $"{Constants.TOKEN_BASE_PATTERN}.{tokenBase64}";
        }

        /// <summary>
        /// Returns a randomized salt.
        /// </summary>
        /// <param name="size">The size of the salt.</param>
        /// <returns><see cref="string"/></returns>
        private static string RandomTokenSalt(ushort size = 8)
        {
            if (size < 8)
                size = 8;

            char[] saltChars = @"0123456789ABCDEF&-~#{}[]()|@$*./\!?".ToLower()
                                                                     .ToCharArray();
            string salt = string.Empty;
            for (int i = 0; i < size; i++)
                salt += saltChars[new Random().Next(0, saltChars.Length - 1)];

            return salt;
        }
    }
}