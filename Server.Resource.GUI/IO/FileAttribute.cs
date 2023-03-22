using System;
using System.Buffers.Binary;

namespace Server.Resource.GUI.IO
{
    internal class FileAttribute
    {
        /// <summary>
        /// The chunk size of a file.
        /// </summary>
        private const byte CHUNK_SIZE = 16;
        /// <summary>
        /// The size of a <see cref="long"/>.
        /// </summary>
        private const byte LONG_SIZE = 8;
        /// <summary>
        /// The offset size.
        /// </summary>
        private const byte OFFSET_SIZE = 1;

        /// <summary>
        /// Sets attributes to a file.
        /// </summary>
        /// <param name="data">The resource to be proceeded.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static byte[] SetAttribute(byte[] data, long value)
        {
            /*
            int moduloLength = data.Length % CHUNK_SIZE;
            int missingLength = CHUNK_SIZE - moduloLength;

            byte[] newData = new byte[data.Length + missingLength + CHUNK_SIZE];
            byte[] valueData = BitConverter.GetBytes(value);

            int endOffsetIndex = data.Length + missingLength + OFFSET_SIZE;
            int valueOffsetIndex = data.Length + missingLength + 2 * OFFSET_SIZE;

            Array.Copy(data, newData, data.Length);
            newData[endOffsetIndex] = 0x0b;
            for (int i = 0; i < LONG_SIZE; i++)
                newData[valueOffsetIndex + i] = valueData[i];
            */
            int moduloLength = data.Length % CHUNK_SIZE;
            int paddingLength = moduloLength == 0 ? 0 : CHUNK_SIZE - moduloLength;
            int newDataLength = data.Length + paddingLength + CHUNK_SIZE;

            byte[] newData = new byte[newDataLength];
            byte[] valueBytes = BitConverter.GetBytes(value);

            int endOffsetIndex = newDataLength - CHUNK_SIZE;
            int valueOffsetIndex = endOffsetIndex - CHUNK_SIZE;

            Array.Copy(data, newData, data.Length);
            newData[endOffsetIndex] = 0x0b;

            for (int i = 0; i < CHUNK_SIZE; i++)
                newData[valueOffsetIndex + i] = valueBytes[i];

            return newData;
        }

        /// <summary>
        /// Returns whether the file has attributes.
        /// </summary>
        /// <param name="data">The resource to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasAttribute(byte[] data)
        {
            byte finalBytes = CHUNK_SIZE - OFFSET_SIZE - LONG_SIZE;
            for (int i = data.Length - finalBytes; i < data.Length; i++)
            {
                if (data[i] != 0)
                    return false;
            }

            return true;
        }

        public static long GetAttribute(byte[] data)
        {
            if (!HasAttribute(data))
                return long.MaxValue;

            // The User ID starts at offset 1.
            byte[] arrayOwnerId = new byte[LONG_SIZE];
            byte offset = CHUNK_SIZE - OFFSET_SIZE;
            for (int i = 0; i < arrayOwnerId.Length; i++)
                arrayOwnerId[i] = data[data.Length - offset + i];

            return BinaryPrimitives.ReadInt64BigEndian(arrayOwnerId);
        }
    }
}