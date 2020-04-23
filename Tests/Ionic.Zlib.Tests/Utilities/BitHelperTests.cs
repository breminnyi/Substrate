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

        [Theory]
        [InlineData(0b0000_0000_1111_1111, 0b1111_1111_0000_0000)]
        [InlineData(0b1010_1010_1010_1010, 0b0101_0101_0101_0101)]
        [InlineData(0b1100_1100_1100_1100, 0b0011_0011_0011_0011)]
        [InlineData(0b1100_1100_0011_0011, 0b1100_1100_0011_0011)]
        [InlineData(0x0000, 0x0000)]
        [InlineData(0xFFFF, 0xFFFF)]
        public void ReverseShouldHandleUInt16Input(ushort input, ushort expected)
        {
            var actual = BitHelper.Reverse(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0x0000_0000, 0x0000_0000)]
        [InlineData(0xFFFF_FFFF, 0xFFFF_FFFF)]
        [InlineData(0x0000_FFFF, 0xFFFF_0000)]
        [InlineData(0x0000_00F0, 0x0F00_0000)]
        [InlineData(0b0111, 0b1110_0000_0000_0000_0000_0000_0000_0000)]
        [InlineData(0b1100_0100, 0b0010_0011_0000_0000_0000_0000_0000_0000)]
        public void ReverseShouldHandleUInt32Input(uint input, uint expected)
        {
            var actual = BitHelper.Reverse(input);
            Assert.Equal(expected, actual);
        }
    }
}