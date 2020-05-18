using System.Text;
using Xunit;

namespace Ionic.Zlib.Checksums
{
    public class Crc8Tests
    {
        [Theory]
        [InlineData(new byte[] {0xC2}, 0x1D, 0xF)]
        [InlineData(new byte[] {0xAB, 0xCD, 0xEF}, 0x7, 0x23)]
        [InlineData(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89}, 0xD5, 0x4C)]
        public void ComputeShouldReturnCorrectResult(byte[] input, byte generator, byte expected)
        {
            var crc8 = new Crc8(generator, 0x00, false, false, 0x00);
            var actual = crc8.Compute(input);
            Assert.Equal<byte>(new[] {expected}, actual);
        }

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
        public void ComputeShouldPassCheckForKnownAlgorithms(byte check, byte poly, byte init, bool refIn, bool refOut,
            byte xorOut)
        {
            var crc8 = new Crc8(poly, init, refIn, refOut, xorOut);
            var actual = crc8.Compute(Encoding.ASCII.GetBytes("123456789"));
            Assert.Equal<byte>(new[] {check}, actual);
        }
    }
}