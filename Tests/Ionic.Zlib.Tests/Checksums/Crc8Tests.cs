using System.Text;
using Xunit;

namespace Ionic.Zlib.Checksums
{
    public class Crc8Tests
    {
        [Theory]
        [InlineData(new byte[] {0xC2}, 0x1D, 0x00, 0x0F)]
        [InlineData(new byte[] {0xAB, 0xCD, 0xEF}, 0x07, 0x00, 0x23)]
        [InlineData(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89}, 0xD5, 0x00, 0x4C)]
        public void ComputeShouldReturnCorrectResult(byte[] input, byte generator, byte initial, byte expected)
        {
            var crc8 = new Crc8(generator, initial);
            var actual = crc8.Compute(input);
            Assert.Equal<byte>(new[] {expected}, actual);
        }

        [Theory]
        [InlineData(0x07, 0x00, 0xF4)]
        [InlineData(0x9B, 0xFF, 0xDA)]
        [InlineData(0xD5, 0x00, 0xBC)]
        [InlineData(0x1D, 0xFD, 0x7E)]
        public void ComputeShouldPassCheck(byte polynomial, byte initial, byte check)
        {
            var input = Encoding.ASCII.GetBytes("123456789");
            var crc8 = new Crc8(polynomial, initial);
            var actual = crc8.Compute(input);
            Assert.Equal<byte>(new[] {check}, actual);
        }
    }
}