using Xunit;

namespace Ionic.Zlib.Checksums
{
    public class Crc16Tests
    {
        [Theory]
        [InlineData(new byte[] {1, 2}, 0x1021, 0x1373)]
        [InlineData(
            new byte[]
            {
                0x4C, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x65, 0x00, 0x6D, 0x00, 0x20, 0x00, 0x69, 0x00, 0x70, 0x00, 0x73,
                0x00, 0x75
            }, 0x8005, 0xCB3E)]
        public void ComputeShouldReturnValidCode(byte[] input, ushort generator, ushort expected)
        {
            var crc16 = new Crc16(generator);
            var actual = crc16.Compute(input);
            Assert.Equal(expected, actual);
        }
    }
}