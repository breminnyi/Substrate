namespace Ionic.Zlib.Utils
{
    internal static class BitHelper
    {
        public static byte ReverseOrder(byte value)
        {
            unchecked
            {
                return (byte) (((value * 0x0802U & 0x22110U) | (value * 0x8020U & 0x88440LU)) * 0x10101U >> 16);
            }
        }

        public static ushort ReverseOrder(ushort value)
        {
            unchecked
            {
                return (ushort) ((ReverseOrder((byte) (value & 0xFF)) << 8) | ReverseOrder((byte) (value >> 8)));
            }
        }

        public static uint ReverseOrder(uint value)
        {
            unchecked
            {
                value = ((value >> 1) & 0x5555_5555) | ((value & 0x5555_5555) << 1);
                value = ((value >> 2) & 0x3333_3333) | ((value & 0x3333_3333) << 2);
                value = ((value >> 4) & 0x0F0F_0F0F) | ((value & 0x0F0F_0F0F) << 4);
                value = ((value >> 8) & 0x00FF_00FF) | ((value & 0x00FF_00FF) << 8);
                value = (value >> 16) | (value << 16);
                return value;
            }
        }

        public static ulong ReverseOrder(ulong value)
        {
            unchecked
            {
                return (ulong) ReverseOrder((uint) (value & 0xFFFF_FFFF)) << 32 | ReverseOrder((uint) (value >> 32));
            }
        }
    }
}