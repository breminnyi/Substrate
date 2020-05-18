namespace Ionic.Zlib.Checksums
{
    public class Crc8
    {
        private readonly byte[] _lookupTable;

        public Crc8(byte generator)
        {
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
            byte crc = 0;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = offset; index < offset + count; index++)
            {
                var data = block[index] ^ crc;
                crc = _lookupTable[data];
            }

            return new[] {crc};
        }
    }
}