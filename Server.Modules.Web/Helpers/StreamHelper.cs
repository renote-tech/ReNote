﻿using System.IO;
using System.Threading.Tasks;

namespace Server.Web.Helpers
{
    internal class StreamHelper
    {
        /// <summary>
        /// Retrieves the data from a <see cref="Stream"/> as a string.
        /// </summary>
        /// <typeparam name="T">The new type of the <see cref="Stream"/>'s data.</typeparam>
        /// <param name="stream">The stream to be converted.</param>
        /// <returns><see cref="string"/></returns>
        public static async Task<string> GetStringAsync(Stream stream)
        {
            using StreamReader streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }
    }
}