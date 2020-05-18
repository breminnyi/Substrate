using System;

namespace Ionic.Zlib.Checksums
{
    public class Crc16
    {
        private readonly ushort[] _lookupTable;

        public Crc16(ushort generator)
        {
            _lookupTable = new ushort[256];
            for (var value = 0; value < 256; value++)
            {
                var crc = (ushort) (value << 8);
                for (byte bit = 0; bit < 8; bit++)
                {
                    var msbSet = (crc & 0x8000) == 0x8000;
                    crc <<= 1;
                    if (msbSet)
                    {
                        crc ^= generator;
                    }
                }

                _lookupTable[value] = crc;
            }
        }
        
        public byte[] Compute(byte[] block)
        {
            return Compute(block, 0, block.Length);
        }

        public byte[] Compute(byte[] block, int offset, int count)
        {
            ushort crc = 0;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = offset; index < offset + count; index++)
            {
                var data = block[index] ^ (crc >> 8);
                crc = (ushort) (((crc & 0xFF) << 8) ^ _lookupTable[data]);
            }

            return BitConverter.GetBytes(crc);
        }
    }
}