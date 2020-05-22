using System.IO;
using Ionic.Zlib;
using Substrate.Nbt;
using Xunit;
using Xunit.Abstractions;

namespace Substrate.Tests.Nbt
{
    public class NbtTreeTests
    {
        private readonly ITestOutputHelper _output;
        
        public NbtTreeTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        private byte[] ReadFile(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var stream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (var ms = new MemoryStream())
                    {
                        var buffer = new byte[4096];
                        var bytesRead = 0;
                        do
                        {
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, bytesRead);
                        } while (bytesRead == buffer.Length);

                        return ms.ToArray();
                    }
                }
            }
        }

        [Theory]
        [InlineData(@"..\..\..\Data\1_8_3-debug\level.dat")]
        [InlineData(@"..\..\..\Data\1_8_3-debug\data\villages.dat")]
        [InlineData(@"..\..\..\Data\1_8_3-debug\data\villages_end.dat")]
        [InlineData(@"..\..\..\Data\1_8_3-debug\data\villages_nether.dat")]
        public void ReadWriteTreeDoesNotAlterTheData(string file)
        {
            var input = ReadFile(file);
            NbtTree tree;
            using (var ms = new MemoryStream(input))
            {
                tree = new NbtTree();
                tree.ReadFrom(ms);
            }

            byte[] output;
            using (var ms = new MemoryStream())
            {
                tree.WriteTo(ms);
                output = ms.ToArray();
            }

            CompareCollections(input, output);
        }

        private void CompareCollections(byte[] input, byte[] output)
        {
            Assert.Equal(input.Length, output.Length);
            for (var index = 0; index < input.Length; index++)
            {
                if (input[index] != output[index])
                {
                    _output.WriteLine($"[{index}]: 0x{input[index]:X2} != 0x{output[index]:X2}");
                    Assert.True(false);
                }
            }
        }
    }
}