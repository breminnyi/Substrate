using System;
using System.Text;
using Xunit;

namespace Ionic.Zlib
{
    public class Crc32Tests
    {
        [Fact]
        public void ShouldCompleteDefaultCheck()
        {
            byte[] input = Encoding.ASCII.GetBytes("123456789");
            var crc32 = new Crc32();

            crc32.SlurpBlock(input,0,input.Length);

            Assert.Equal((uint)0xCBF43926, (uint)crc32.Crc32Result);
        }
    }
}