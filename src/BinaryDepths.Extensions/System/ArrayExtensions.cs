using System.Runtime.CompilerServices;

namespace System
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines the size of the array in bytes using <see cref="Unsafe.SizeOf{T}"/> * Length
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The size in bytes</returns>
        public static uint SizeInBytes<T>(this T[] array) where T : struct
        {
            return (uint)(array.Length * Unsafe.SizeOf<T>());
        }
    }
}