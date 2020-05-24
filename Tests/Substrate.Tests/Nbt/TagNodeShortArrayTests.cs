using System;
using System.IO;
using System.Threading.Tasks;
using Substrate.Nbt;
using Substrate.Utilities;
using Xunit;

namespace Substrate.Tests.Nbt
{
    public class TagNodeShortArrayTests
    {
        [Theory]
        [InlineData(new short[0])]
        [InlineData(new short[] {short.MinValue, -1000, -100, -10, -1, 0, 1, 10, 100, 1000, short.MaxValue})]
        public void DeserializeReadsCorrectData(short[] expected)
        {
            var input = Encode(expected);

            short[] actual;
            using (var ms = new MemoryStream(input))
            {
                var tag = new TagNodeShortArray();
                tag.Deserialize(ms);
                actual = tag.Data;
            }

            Assert.Equal<short>(expected, actual);
        }
        
        [Theory]
        [InlineData(new short[0])]
        [InlineData(new short[] {short.MinValue, -1000, -100, -10, -1, 0, 1, 10, 100, 1000, short.MaxValue})]
        public async Task DeserializeAsyncReadsCorrectData(short[] expected)
        {
            var input = Encode(expected);

            short[] actual;
            using (var ms = new MemoryStream(input))
            {
                var tag = new TagNodeShortArray();
                await tag.DeserializeAsync(ms).ConfigureAwait(false);
                actual = tag.Data;
            }

            Assert.Equal<short>(expected, actual);
        }

        private byte[] Encode(short[] data)
        {
            var lengthBytes = BitConverter.GetBytes(data.Length).EnsureBigEndian();
            var result = new byte[4 + sizeof(short) * data.Length];
            Array.Copy(lengthBytes, result, lengthBytes.Length);
            for (var index = 0; index < data.Length; index++)
            {
                var bytes = BitConverter.GetBytes(data[index]).EnsureBigEndian();
                Array.Copy(bytes, 0, result, 4 + index * sizeof(short), bytes.Length);
            }

            return result;
        }
    }
}