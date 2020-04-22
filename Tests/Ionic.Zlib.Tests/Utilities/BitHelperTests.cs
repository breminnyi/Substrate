using Xunit;

namespace Ionic.Zlib.Utilities
{
    public class BitHelperTests
    {
        [Theory]
        [InlineData(0b0000_1111, 0b1111_0000)]
        [InlineData(0b1010_1010, 0b0101_0101)]
        [InlineData(0b1100_1100, 0b0011_0011)]
        [InlineData(0b1100_0011, 0b1100_0011)]
        [InlineData(0x00, 0x00)]
        [InlineData(0xFF, 0xFF)]
        public void ReverseReturnsCorrectValueForInputByte(byte input, byte expected)
        {
            var actual = BitHelper.Reverse(input);

            Assert.Equal(expected, actual);
        }
    }
}