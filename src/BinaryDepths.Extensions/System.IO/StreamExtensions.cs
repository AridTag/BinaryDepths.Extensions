using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryDepths.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the contents of the stream into a byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fromCurrentPosition">
        /// <para>true to begin reading from the current stream position.</para>
        /// <para>false to begin reading from the start of the stream if the stream supports seeking.</para>
        /// </param>
        /// <returns>
        /// The contents of the stream as a byte[]
        /// </returns>
        public static byte[] ToByteArray(this Stream stream, bool fromCurrentPosition = true)
        {
            long originalPosition = 0;
            if (!fromCurrentPosition && stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                const int bufferSize = 1024;
                var byteList = new List<byte>(bufferSize);
                var readBuffer = new byte[bufferSize];
                var bytesReadThisBuffer = 0;

                var bytesRead = stream.Read(readBuffer, bytesReadThisBuffer, readBuffer.Length);
                while (bytesRead > 0)
                {
                    bytesReadThisBuffer += bytesRead;

                    if (bytesReadThisBuffer == readBuffer.Length)
                    {
                        var nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byteList.AddRange(readBuffer);
                            byteList.Add((byte)nextByte);
                            bytesReadThisBuffer = 0;
                        }
                    }

                    bytesRead = stream.Read(readBuffer, bytesReadThisBuffer, readBuffer.Length - bytesReadThisBuffer);
                }

                // Scoop up any overflow
                if (bytesReadThisBuffer > 0)
                {
                    byteList.AddRange(readBuffer.Take(bytesReadThisBuffer));
                }

                return byteList.ToArray();
            }
            finally
            {
                if (!fromCurrentPosition && stream.CanSeek)
                {
                    stream.Seek(originalPosition, SeekOrigin.Begin);
                }
            }
        }
    }
}
