using System;
using System.IO;
using Substrate.Nbt;
using Substrate.Utilities;
using Xunit;

namespace Substrate.Tests.Nbt
{
    public class TagNodeLongArrayTests
    {
        [Theory]
        [InlineData(new long[0])]
        [InlineData(new long[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9})]
        public void DeserializeReadsCorrectData(long[] expected)
        {
            var input = Encode(expected);

            long[] actual;
            using (var ms = new MemoryStream(input))
            {
                var tag = new TagNodeLongArray();
                tag.Deserialize(ms);
                actual = tag.Data;
            }

            Assert.Equal<long>(expected, actual);
        }

        private byte[] Encode(long[] data)
        {
            var lengthBytes = BitConverter.GetBytes(data.Length).EnsureBigEndian();
            var result = new byte[4 + sizeof(long) * data.Length];
            Array.Copy(lengthBytes, result, lengthBytes.Length);
            for (var index = 0; index < data.Length; index++)
            {
                var bytes = BitConverter.GetBytes(data[index]).EnsureBigEndian();
                Array.Copy(bytes, 0, result, 4 + index * sizeof(long), bytes.Length);
            }

            return result;
        }
    }
}