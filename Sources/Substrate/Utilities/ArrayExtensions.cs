using System;

namespace Substrate.Utilities
{
    internal static class ArrayExtensions
    {
        public static byte[] EnsureBigEndian(this byte[] block)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(block);
            }

            return block;
        }
    }
}