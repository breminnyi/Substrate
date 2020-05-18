using Ionic.Zlib.Utils;

namespace Ionic.Zlib.Checksums
{
    public class Crc8
    {
        private readonly byte[] _lookupTable;
        private readonly byte _initial;
        private readonly bool _reflectInput;
        private readonly bool _reflectOutput;
        private readonly byte _finalXor;

        public Crc8(byte polynomial, byte initial, bool reflectInput, bool reflectOutput, byte finalXor)
        {
            _initial = initial;
            _reflectInput = reflectInput;
            _reflectOutput = reflectOutput;
            _finalXor = finalXor;

            _lookupTable = new byte[256];
            for (var value = 0; value < 256; value++)
            {
                var crc = (byte) value;
                for (byte bit = 0; bit < 8; bit++)
                {
                    var msbSet = (crc & 0b1000_0000) == 0b1000_0000;
                    crc <<= 1;
                    if (msbSet)
                    {
                        crc ^= polynomial;
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
            var crc = _initial;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = offset; index < offset + count; index++)
            {
                var data = crc ^ (_reflectInput ? BitHelper.ReverseOrder(block[index]) : block[index]);
                crc = _lookupTable[data];
            }

            crc = _reflectOutput ? BitHelper.ReverseOrder(crc) : crc;
            return new[] {(byte) (crc ^ _finalXor)};
        }
    }
}