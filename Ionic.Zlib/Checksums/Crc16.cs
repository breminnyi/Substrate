namespace Ionic.Zlib.Checksums
{
    public class Crc16
    {
        public static Crc16 Default { get; } = new Crc16(0x1021);

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
        
        public ushort Compute(byte[] block)
        {
            return Compute(block, 0, block.Length);
        }

        public ushort Compute(byte[] block, int offset, int count)
        {
            ushort crc = 0;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = offset; index < offset + count; index++)
            {
                var data = block[index] ^ (crc >> 8);
                crc = unchecked((ushort) ((crc << 8) ^ _lookupTable[data]));
            }

            return crc;
        }
    }
}