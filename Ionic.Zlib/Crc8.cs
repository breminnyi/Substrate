using System;
using System.Linq;

namespace Ionic.Zlib
{
    public class Crc8
    {
        public static Crc8 Default { get; } = new Crc8(7);

        private readonly byte[] _lookupTable;

        public Crc8(byte generator)
        {
            _lookupTable = new byte[256];
            for (var @byte = 0; @byte < 256; @byte++)
            {
                var crc = (byte) @byte;
                for (byte bit = 0; bit < 8; bit++)
                {
                    var msbSet = (crc & 0b1000_0000) == 0b1000_0000;
                    crc <<= 1;
                    if (msbSet)
                    {
                        crc ^= generator;
                    }
                }

                _lookupTable[@byte] = crc;
            }
        }

        public byte Compute(byte[] input)
        {
            return Compute(input, 0, input.Length);
        }

        public byte Compute(byte[] input, int offset, int count)
        {
            byte crc = 0;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = offset; index < offset + count; index++)
            {
                var data = input[index] ^ crc;
                crc = _lookupTable[data];
            }

            return crc;
        }
    }
}