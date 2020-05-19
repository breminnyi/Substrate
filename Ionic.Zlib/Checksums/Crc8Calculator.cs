using Ionic.Zlib.Utils;

namespace Ionic.Zlib.Checksums
{
    public class Crc8Calculator : ICrcCalculator
    {
        private readonly byte[] _lookupTable;
        private readonly byte _initial;
        private readonly bool _reflectInput;
        private readonly bool _reflectOutput;
        private readonly byte _finalXor;
        private byte _result;

        public Crc8Calculator(byte polynomial, byte initial, bool reflectInput, bool reflectOutput, byte finalXor)
        {
            _lookupTable = GenerateLookup(polynomial);

            _initial = initial;
            _reflectInput = reflectInput;
            _reflectOutput = reflectOutput;
            _finalXor = finalXor;

            Reset();
        }

        public byte[] Result
        {
            get
            {
                var crc = _reflectOutput ? BitHelper.ReverseOrder(_result) : _result;
                return new[] {(byte) (crc ^ _finalXor)};
            }
        }

        private static byte[] GenerateLookup(byte polynomial)
        {
            var lookup = new byte[256];
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

                lookup[value] = crc;
            }

            return lookup;
        }

        public long BytesRead { get; private set; }

        public void Advance(byte[] block, int offset, int count)
        {
            for (var index = offset; index < offset + count; index++)
            {
                var data = _result ^ (_reflectInput ? BitHelper.ReverseOrder(block[index]) : block[index]);
                _result = _lookupTable[data];
            }
        }

        public void Reset()
        {
            _result = _initial;
            BytesRead = 0;
        }
    }
}