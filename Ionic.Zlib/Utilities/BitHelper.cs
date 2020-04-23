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
                return (byte) (((value * 0x0802u & 0x22110u) | (value * 0x8020u & 0x88440u)) * 0x10101u >> 16);
            }
        }

        public static ushort Reverse(ushort value)
        {
            unchecked
            {
                value = (ushort) ((value >> 1) & 0x5555 | (value << 1) & 0xAAAA);
                value = (ushort) ((value >> 2) & 0x3333 | (value << 2) & 0xCCCC);
                value = (ushort) ((value >> 4) & 0x0F0F | (value << 4) & 0xF0F0);
                value = (ushort) ((value >> 8) | (value << 8));
                return value;
            }
        }

        public static uint Reverse(uint value)
        {
            unchecked
            {
                value = (value >> 01) & 0x5555_5555 | (value << 01) & 0xAAAA_AAAA;
                value = (value >> 02) & 0x3333_3333 | (value << 02) & 0xCCCC_CCCC;
                value = (value >> 04) & 0x0F0F_0F0F | (value << 04) & 0xF0F0_F0F0;
                value = (value >> 08) & 0x00FF_00FF | (value << 08) & 0xFF00_FF00;
                value = (value >> 16) | (value << 16);
                return value;
            }
        }
    }
}