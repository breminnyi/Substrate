using System;
using System.IO;
using Substrate.Nbt;
using Substrate.Utilities;
using Xunit;

namespace Substrate.Tests.Nbt
{
    public class TagNodeIntArrayTests
    {
        [Theory]
        [InlineData(new int[0])]
        [InlineData(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9})]
        public void DeserializeReadsCorrectData(int[] expected)
        {
            var input = Encode(expected);

            int[] actual;
            using (var ms = new MemoryStream(input))
            {
                var tag = new TagNodeIntArray();
                tag.Deserialize(ms);
                actual = tag.Data;
            }

            Assert.Equal<int>(expected, actual);
        }

        private byte[] Encode(int[] data)
        {
            var lengthBytes = BitConverter.GetBytes(data.Length).EnsureBigEndian();
            var result = new byte[4 + 4 * data.Length];
            Array.Copy(lengthBytes, result, lengthBytes.Length);
            for (var index = 0; index < data.Length; index++)
            {
                var bytes = BitConverter.GetBytes(data[index]).EnsureBigEndian();
                Array.Copy(bytes, 0, result, 4 * (index + 1), bytes.Length);
            }

            return result;
        }
    }
}