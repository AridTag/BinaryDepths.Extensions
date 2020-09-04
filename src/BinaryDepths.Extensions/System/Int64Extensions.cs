namespace BinaryDepths.Extensions
{
    public static class Int64Extensions
    {
        /// <summary>
        /// Gets the next greatest power of 2.
        /// </summary>
        /// <remarks>
        /// It is recommended to use the unsigned version of this method
        /// </remarks>
        /// <param name="num"></param>
        /// <returns>The next greatest power of 2</returns>
        public static long GetNextPower2(this long num)
        {
            if (num < 0)
                return 0;

            return (long)GetNextPower2((ulong)num);
        }

        /// <summary>
        /// Gets the next greatest power of 2.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The next greatest power of 2</returns>
        public static ulong GetNextPower2(this ulong num)
        {
            --num;
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            num |= num >> 32;
            return num + 1;
        }
    }
}