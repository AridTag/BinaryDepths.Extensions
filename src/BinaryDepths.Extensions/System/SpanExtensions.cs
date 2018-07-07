using System;
using System.Runtime.InteropServices;

namespace BinaryDepths.Extensions.System
{
#if NETCORE2_1
    public static class SpanExtensions
    {
        public static T ToStruct<T>(this Span<byte> bytes) where T : struct
        {
            return ToStruct<T>((ReadOnlySpan<byte>)bytes);
        }

        public static T[] ToStructArray<T>(this Span<byte> bytes, uint numEntries) where T : struct
        {
            return ToStructArray<T>((ReadOnlySpan<byte>)bytes, numEntries);
        }

        public static T ToStruct<T>(this ReadOnlySpan<byte> bytes) where T : struct
        {
            return MemoryMarshal.Read<T>(bytes);
        }

        public static T[] ToStructArray<T>(this ReadOnlySpan<byte> bytes, uint numEntries) where T : struct
        {
            var sizeOfT = Marshal.SizeOf<T>();
            var retVal = new T[numEntries];
            for (int i = 0; i < numEntries; i++)
            {
                retVal[i] = MemoryMarshal.Read<T>(bytes.Slice(0, sizeOfT));
                bytes = bytes.Slice(sizeOfT);
            }

            return retVal;
        }

        /// <summary>
        /// Returns a slice of length starting at 0
        /// </summary>
        /// <param name="span"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Span<T> Take<T>(this Span<T> span, int length)
        {
            return span.Slice(0, length);
        }
    }
#endif
}
