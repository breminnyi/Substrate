using System.Diagnostics;
using Xunit;

namespace Ionic.Zlib
{
    public class Crc8Tests
    {
        [Theory]
        [InlineData(new byte[] {0xC2}, 0x1D, 0xF)]
        [InlineData(new byte[] {0xAB, 0xCD, 0xEF}, 0x7, 0x23)]
        [InlineData(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89}, 0xD5, 0x4C)]
        public void ComputeShouldReturnCorrectResult(byte[] input, byte generator, byte expected)
        {
            var crc8 = new Crc8(generator);
            var actual = crc8.Compute(input);
            Assert.Equal(expected, actual);
        }
    }
}