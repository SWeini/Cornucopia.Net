﻿namespace System.Numerics
{
    public static class BitOperations
    {
        private static ReadOnlySpan<byte> Log2DeBruijn => new byte[]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        public static int Log2(uint value)
        {
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            return Log2DeBruijn[(int) ((value * 0x07C4ACDDu) >> 27)];
        }

        public static int Log2(ulong value)
        {
            var hi = (uint) (value >> 32);

            if (hi == 0)
            {
                return Log2((uint) value);
            }

            return 32 + Log2(hi);
        }
    }
}