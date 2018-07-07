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
    }
}
