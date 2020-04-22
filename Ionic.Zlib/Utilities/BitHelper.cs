using System;
using System.Runtime.CompilerServices;

namespace Ionic.Zlib.Utilities
{
    internal static class BitHelper
    {
        public static byte Reverse(byte value)
        {
            unchecked
            {
                // 0x00802 = 0b0000_0000_‭1000_0000_0010‬
                // 0x02211 = 0b0000_‭0010_0010_0001_0001‬
                // 0x88440 = 0b‭1000_1000_0100_0100_0000‬
                // 0x10101 = 0b‭0001_0000_0001_0000_0001‬
                return (byte)(((value * 0x0802u & 0x22110u) | (value * 0x8020u & 0x88440u)) * 0x10101u >> 16);
            }
        }
    }
}