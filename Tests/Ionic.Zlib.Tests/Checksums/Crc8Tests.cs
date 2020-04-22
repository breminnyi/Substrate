using System.Text;
using Xunit;

namespace Ionic.Zlib.Checksums
{
    public class Crc8Tests
    {
        [Theory]
        [InlineData(0xF4, 0x07, 0x00, false, false, 0x00)]
        [InlineData(0xDA, 0x9B, 0xFF, false, false, 0x00)]
        [InlineData(0x15, 0x39, 0x00, true, true, 0x00)]
        [InlineData(0xBC, 0xD5, 0x00, false, false, 0x00)]
        [InlineData(0x97, 0x1D, 0xFF, true, true, 0x00)]
        [InlineData(0x7E, 0x1D, 0xFD, false, false, 0x00)]
        [InlineData(0xA1, 0x07, 0x00, false, false, 0x55)]
        [InlineData(0xA1, 0x31, 0x00, true, true, 0x00)]
        [InlineData(0xD0, 0x07, 0xFF, true, true, 0x00)]
        [InlineData(0x25, 0x9B, 0x00, true, true, 0x00)]
        public void ComputeShouldPassCheck(byte check, byte polynomial, byte initial, bool refIn, bool refOut,
            byte finalXor)
        {
            var input = Encoding.ASCII.GetBytes("123456789");
            var crc8 = new Crc8(polynomial, initial, refIn, refOut, finalXor);
            var actual = crc8.Compute(input);
            Assert.Equal<byte>(new[] {check}, actual);
        }

        [Theory]
        [InlineData(new byte[] {0xC2}, 0x1D, 0x00, 0x0F)]
        [InlineData(new byte[] {0xAB, 0xCD, 0xEF}, 0x07, 0x00, 0x23)]
        [InlineData(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89}, 0xD5, 0x00, 0x4C)]
        public void ComputeShouldReturnCorrectResult(byte[] input, byte generator, byte initial, byte expected)
        {
            var crc8 = new Crc8(generator, initial, false, false, 0x00);
            var actual = crc8.Compute(input);
            Assert.Equal<byte>(new[] {expected}, actual);
        }
    }
}