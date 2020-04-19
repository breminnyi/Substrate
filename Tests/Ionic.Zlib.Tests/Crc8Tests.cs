using System.Diagnostics;
using Xunit;

namespace Ionic.Zlib
{
    public class Crc8Tests
    {
        [Theory]
        [InlineData(new byte[] {0xC2}, 0x11D, 0xF)]
        [InlineData(new byte[] {0xAB, 0xCD, 0xEF}, 0x7, 0x23)]
        [InlineData(new byte[] {0x01, 0x23, 0x45, 0x67,0x89}, 0xD5, 0x4C)]
        public void CalculateSimpleShouldReturnCorrectResult(byte[] input, ushort generator, byte expected)
        {
            var actual = Crc8.CalculateSimple(input, generator);
            Assert.Equal(expected, actual);
        }
    }
}