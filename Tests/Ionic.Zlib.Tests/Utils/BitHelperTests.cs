using System;
using System.Linq;
using Xunit;

namespace Ionic.Zlib.Utils
{
    public class BitHelperTests
    {
        private const int NumberOfRepetitions = 0x200;

        [Fact]
        public void ReverseOrderReturnsCorrectResultForEachInputByte()
        {
            for (var value = 0; value < 0x100; value++)
            {
                var expected = (byte) ReverseOrderDummy((byte) value, 8);

                var actual = BitHelper.ReverseOrder((byte) value);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ReverseOrderReturnsCorrectResultForInputUInt16()
        {
            for (var index = 0; index < NumberOfRepetitions; index++)
            {
                var input = (ushort) new Random().Next(0, ushort.MaxValue + 1);
                var expected = (ushort) ReverseOrderDummy(input, 16);

                var actual = BitHelper.ReverseOrder(input);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ReverseOrderReturnsCorrectResultForInputUInt32()
        {
            for (var index = 0; index < NumberOfRepetitions; index++)
            {
                var input = (uint) (new Random().NextDouble() * uint.MaxValue);
                var expected = (uint) ReverseOrderDummy(input, 32);

                var actual = BitHelper.ReverseOrder(input);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ReverseOrderReturnsCorrectResultForInputUInt64()
        {
            for (var index = 0; index < NumberOfRepetitions; index++)
            {
                var input = (ulong) (new Random().NextDouble() * ulong.MaxValue);
                var expected = (ulong) ReverseOrderDummy(input);

                var actual = BitHelper.ReverseOrder(input);

                Assert.Equal(expected, actual);
            }
        }

        private static long ReverseOrderDummy(long input, int length)
        {
            var binaryString = Convert.ToString(input, 2);
            var reversed = new string('0', length).ToArray();
            Array.Copy(binaryString.Reverse().ToArray(), reversed, binaryString.Length);
            return Convert.ToInt64(new string(reversed), 2);
        }

        private static ulong ReverseOrderDummy(ulong input)
        {
            var part1 = (uint) ReverseOrderDummy((uint) (input >> 32), 32);
            ulong part2 = (uint) ReverseOrderDummy((uint) (input & 0xFFFF_FFFF), 32);
            return (part2 << 32) | part1;
        }
    }
}