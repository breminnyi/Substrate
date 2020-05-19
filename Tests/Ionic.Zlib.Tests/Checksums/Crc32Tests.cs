using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ionic.Zlib.Checksums
{
    public class Crc32Tests
    {
        [Fact]
        public void ShouldPassCheck()
        {
            var input = Encoding.ASCII.GetBytes("123456789");
            var crc32 = new Crc32();

            crc32.SlurpBlock(input, 0, input.Length);

            Assert.Equal(0xCBF43926, (uint) crc32.Crc32Result);
        }

        [Fact]
        public void BytesReadShouldBeEqualByDefault()
        {
            var crc = new Crc32();

            Assert.Equal(0, crc.TotalBytesRead);
        }

        [Fact]
        public void ResultShouldBeZeroByDefault()
        {
            var crc = new Crc32();

            Assert.Equal(0, crc.Crc32Result);
        }

        [Fact]
        public void CombineWithZeroShouldNotAlterTheCallee()
        {
            var input = Encoding.ASCII.GetBytes("123456789");
            var crc = new Crc32();

            crc.SlurpBlock(input, 0, input.Length);
            crc.Combine(new Crc32());
            Assert.Equal(0xCBF43926, (uint) crc.Crc32Result);
        }

        [Fact]
        public void CombinationOfSeveralChunksShouldBeEqualToSingleRun()
        {
            const int numberOfChunks = 10;
            const int chunkSize = 100;
            var random = new Random();
            var input = Enumerable.Range(0, numberOfChunks * chunkSize).Select(n => (byte) random.Next(256)).ToArray();

            var chunkedCrc = ComputeUsingChunks(numberOfChunks, input, chunkSize);

            var singleCrc = new Crc32();
            singleCrc.SlurpBlock(input, 0, input.Length);

            Assert.Equal(singleCrc.Crc32Result, chunkedCrc.Crc32Result);
            Assert.Equal(singleCrc.TotalBytesRead, chunkedCrc.TotalBytesRead);
        }
        
        private Crc32 ComputeUsingChunks(int numberOfChunks, byte[] input, int chunkSize)
        {
            var chunkedCrc = new Crc32();
            for (var index = 0; index < numberOfChunks; index++)
            {
                var crc = new Crc32();
                crc.SlurpBlock(input, index * chunkSize, chunkSize);
                chunkedCrc.Combine(crc);
            }

            return chunkedCrc;
        }

        [Fact]
        public void ShouldPassCheckIfUsedAsInterface()
        {
            var input = Encoding.ASCII.GetBytes("123456789");
            ICrcCalculator crc = new Crc32();
            var expected = BitConverter.GetBytes(0xCBF43926);
            
            var actual = crc.Compute(input);

            Assert.Equal<byte>(expected, actual);
        }
    }
}