using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Encryption
{
    public class Sha256
    {
        /// <summary>
        /// Hashifies a <see cref="string"/> using the SHA-256 algorithm.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be hashed.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static async Task<byte[]> ComputeAsync(string content)
        {
            byte[] data = Encoding.ASCII.GetBytes(content);
            return await Task.Run(() => SHA256.HashData(data));
        }

        /// <summary>
        /// Hashifies a <see cref="string"/> using the SHA-256 algorithm as Base64.
        /// </summary>
        /// <param name="content">The <see cref="string"/> to be hashed.</param>
        /// <returns><see cref="string"/></returns>
        public static async Task<string> ComputeStringAsync(string content)
        {
            byte[] data = await ComputeAsync(content);
            return Convert.ToBase64String(data);
        }
    }
}