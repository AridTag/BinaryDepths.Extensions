using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System
{
    public static class ByteArrayExtensions
    {
        public static unsafe T ToStructUnsafe<T>(this byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static unsafe T[] ToStructArrayUnsafe<T>(this byte[] bytes, uint numEntries) where T : struct
        {
            var sizeOfT = Marshal.SizeOf<T>();
            var arrayTotalSize = (int)(sizeOfT * numEntries);
            var array = new T[numEntries];
            using (var memStream = new MemoryStream(bytes, 0, arrayTotalSize, false))
            {
                using (var tableReader = new BinaryReader(memStream))
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = tableReader.ReadBytes(sizeOfT).ToStructUnsafe<T>();
                    }
                }
            }

            return array;
        }

#if NETCORE2_1
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
#endif
    }
}
