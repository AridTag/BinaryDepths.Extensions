using System;
using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream, int minimumSize = 1024)
        {
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                var byteList = new List<byte>(minimumSize);

                var readBuffer = new byte[4096];
                var bytesReadThisBuffer = 0;

                var bytesRead = stream.Read(readBuffer, bytesReadThisBuffer, readBuffer.Length - bytesReadThisBuffer);
                while (bytesRead > 0)
                {
                    bytesReadThisBuffer += bytesRead;

                    if (bytesReadThisBuffer == readBuffer.Length)
                    {
                        var nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byteList.AddRange(readBuffer);
                            bytesReadThisBuffer = 0;
                        }
                    }

                    bytesRead = stream.Read(readBuffer, bytesReadThisBuffer, readBuffer.Length - bytesReadThisBuffer);
                }

                // Scoop up any overflow
                if (bytesReadThisBuffer > 0)
                    byteList.AddRange(readBuffer.Take(bytesReadThisBuffer));

                return byteList.ToArray();
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
