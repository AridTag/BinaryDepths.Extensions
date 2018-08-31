using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Int32Extensions
    {
        /// <summary>
        /// Gets the next greatest power of 2.
        /// </summary>
        /// <remarks>
        /// It is recommended to use the uint version of this method
        /// </remarks>
        /// <param name="num"></param>
        /// <returns>The next greatest power of 2</returns>
        public static int GetNextPower2(this int num)
        {
            if (num < 0)
                return 0;

            return (int)GetNextPower2((uint)num);
        }

        /// <summary>
        /// Gets the next greatest power of 2.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The next greatest power of 2</returns>
        public static uint GetNextPower2(this uint num)
        {
            --num;
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return num + 1;
        }
    }
}
