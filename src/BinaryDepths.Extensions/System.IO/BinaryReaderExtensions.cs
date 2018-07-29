using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.IO
{
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a 3 byte integer from the stream in Big Endian order
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static int ReadInt24BigEndian(this BinaryReader reader)
        {
            return (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte();
        }

        /// <summary>
        /// Reads a 4 byte integer from the stream in Big Endian order
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            return (reader.ReadByte() << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte();
        }

        /// <summary>
        /// Reads an unsigned 2 byte integer from the stream in Big Endian order
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ushort ReadUInt16BigEndian(this BinaryReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) | reader.ReadByte());
        }

        /// <summary>
        /// Reads a null terminated string from the stream
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadZString(this BinaryReader reader)
        {
            var next = reader.Read();
            if (next == -1)
                return string.Empty;

            var stringBuilder = new StringBuilder();
            do
            {
                var c = (char)next;
                if (c == '\0')
                    break;

                stringBuilder.Append(c);
                next = reader.Read();
            } while (next != -1);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the remaining number of bytes if the underlying stream supports it
        /// </summary>
        /// <param name="reader"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <returns></returns>
        public static long Remaining(this BinaryReader reader)
        {
            return reader.BaseStream.Length - reader.BaseStream.Position;
        }
    }
}
