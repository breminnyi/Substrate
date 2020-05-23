using System;
using System.IO;
using System.Text;
using Substrate.Nbt;
using Xunit;

namespace Substrate.Tests.Nbt
{
    public class TagNodeByteTests
    {
        [Theory]
        [InlineData("FooBar", 0xFF)]
        [InlineData("", 0x00)]
        [InlineData(null, 0x12)]
        [InlineData("hello world", 0x40)]
        public void Test(string name, byte input)
        {
            if (name == null) name = string.Empty;
            var expected = new byte[1 + 2 + name.Length + 1];
            expected[0] = (byte) TagType.TAG_BYTE;
            var nameLengthBytes = BitConverter.GetBytes((short) name.Length);
            if(BitConverter.IsLittleEndian) Array.Reverse(nameLengthBytes);
            Array.Copy(nameLengthBytes, 0, expected, 1, nameLengthBytes.Length);
            Encoding.UTF8.GetBytes(name, 0, name.Length, expected, 3);
            expected[expected.Length - 1] = input;
            
            MemoryStream ms;
            using (ms = new MemoryStream())
            {
                new TagNodeByte(input).Serialize(name, ms);
            }

            Assert.Equal<byte>(expected, ms.ToArray());
        }
    }
}