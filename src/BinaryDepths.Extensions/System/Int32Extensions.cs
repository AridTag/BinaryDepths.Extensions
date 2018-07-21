using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Int32Extensions
    {
        public static int GetNearestPower2Ceiling(this int num)
        {
            num = --num | num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return 1 + num;
        }

        public static uint GetNearestPower2Ceiling(this uint num)
        {
            num = --num | num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return 1 + num;
        }
    }
}
