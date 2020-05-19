using System;

namespace Ionic.Zlib.Checksums
{
    public class Crc16Calculator : ICrcCalculator
    {
        private readonly ushort[] _lookupTable;
        private ushort _result;

        public Crc16Calculator(ushort generator)
        {
            _lookupTable = GenerateLookup(generator);
        }

        public byte[] Result => BitConverter.GetBytes(_result);

        public long BytesRead { get; private set; }

        private static ushort[] GenerateLookup(ushort generator)
        {
            var lookup = new ushort[256];
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

                lookup[value] = crc;
            }

            return lookup;
        }

        public void Advance(byte[] block, int offset, int count)
        {
            for (var index = offset; index < offset + count; index++)
            {
                var data = block[index] ^ (_result >> 8);
                _result = (ushort) (((_result & 0xFF) << 8) ^ _lookupTable[data]);
            }

            BytesRead += count;
        }

        public void Reset()
        {
            _result = 0;
        }
    }
}