using System;
using System.IO;
using Substrate.Nbt;
using Substrate.Utilities;
using Xunit;

namespace Substrate.Tests.Nbt
{
    public class TagNodeByteArrayTests
    {
        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] {0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40})]
        public void DeserializeReadsCorrectData(byte[] expected)
        {
            var input = Encode(expected);

            byte[] actual;
            using (var ms = new MemoryStream(input))
            {
                var tag = new TagNodeByteArray();
                tag.Deserialize(ms);
                actual = tag.Data;
            }
            
            Assert.Equal<byte>(expected, actual);
        }

        private byte[] Encode(byte[] data)
        {
            var lengthBytes = BitConverter.GetBytes(data.Length).EnsureBigEndian();
            var result = new byte[4 + data.Length];
            Array.Copy(lengthBytes, result, lengthBytes.Length);
            Array.Copy(data, 0, result, 4, data.Length);
            return result;
        }
    }
}