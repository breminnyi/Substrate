using System;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Sys = System.IO.Compression;

namespace Ionic.Zlib.Compression
{
    public class CompressionStreamTests
    {
        private const string Text = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ultrices dui sapien eget mi proin. Malesuada fames ac turpis egestas maecenas. Pharetra massa massa ultricies mi quis hendrerit. Semper viverra nam libero justo laoreet sit amet cursus sit. Mauris vitae ultricies leo integer malesuada nunc. Felis eget nunc lobortis mattis aliquam. Et magnis dis parturient montes nascetur ridiculus mus. Amet luctus venenatis lectus magna fringilla urna porttitor rhoncus dolor. Sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus. Eget felis eget nunc lobortis mattis. Neque egestas congue quisque egestas diam in arcu cursus euismod. Pharetra convallis posuere morbi leo urna molestie at elementum. At urna condimentum mattis pellentesque id nibh tortor id aliquet. Vitae tempus quam pellentesque nec. Velit aliquet sagittis id consectetur purus ut faucibus pulvinar elementum. Egestas pretium aenean pharetra magna.

In cursus turpis massa tincidunt. Ligula ullamcorper malesuada proin libero nunc. Ornare lectus sit amet est placerat in egestas erat. Sit amet luctus venenatis lectus. Luctus accumsan tortor posuere ac. Duis ut diam quam nulla porttitor massa. Ullamcorper a lacus vestibulum sed arcu non odio euismod. Sed felis eget velit aliquet sagittis id consectetur purus ut. Tellus in metus vulputate eu scelerisque felis. Eget dolor morbi non arcu risus quis. Sed blandit libero volutpat sed. Parturient montes nascetur ridiculus mus mauris. Dolor sit amet consectetur adipiscing elit. Viverra aliquet eget sit amet tellus cras adipiscing enim eu. Maecenas volutpat blandit aliquam etiam erat velit scelerisque in. Commodo ullamcorper a lacus vestibulum sed arcu non odio euismod. Lectus urna duis convallis convallis. Semper risus in hendrerit gravida rutrum quisque non tellus orci.

Ante metus dictum at tempor commodo. Nulla facilisi nullam vehicula ipsum. Ullamcorper eget nulla facilisi etiam dignissim diam quis enim. Ornare suspendisse sed nisi lacus. Consectetur a erat nam at. Et magnis dis parturient montes nascetur ridiculus mus. Massa vitae tortor condimentum lacinia quis vel eros donec. Purus in mollis nunc sed id semper. Cum sociis natoque penatibus et magnis. Suspendisse interdum consectetur libero id faucibus nisl tincidunt eget. Maecenas pharetra convallis posuere morbi leo. Elementum nisi quis eleifend quam adipiscing vitae proin sagittis. Neque gravida in fermentum et sollicitudin ac orci phasellus egestas. Cursus eget nunc scelerisque viverra. Elit ut aliquam purus sit amet luctus venenatis. Sit amet commodo nulla facilisi nullam vehicula ipsum a. Sit amet commodo nulla facilisi nullam vehicula ipsum a arcu.

Vestibulum mattis ullamcorper velit sed ullamcorper morbi. Tristique senectus et netus et malesuada fames ac. Sit amet massa vitae tortor condimentum lacinia. In est ante in nibh mauris cursus mattis molestie. Hendrerit dolor magna eget est lorem ipsum dolor. Massa massa ultricies mi quis hendrerit dolor magna eget est. Quam vulputate dignissim suspendisse in est ante in. Nunc pulvinar sapien et ligula ullamcorper malesuada. Quis commodo odio aenean sed adipiscing diam. Amet purus gravida quis blandit turpis cursus in hac.

Nam aliquam sem et tortor consequat id porta nibh venenatis. Eget lorem dolor sed viverra ipsum nunc. Quis varius quam quisque id. Nisi porta lorem mollis aliquam ut porttitor. Aliquam eleifend mi in nulla posuere. Lorem mollis aliquam ut porttitor leo. In nibh mauris cursus mattis molestie a iaculis. Blandit volutpat maecenas volutpat blandit aliquam. Odio ut sem nulla pharetra. Non arcu risus quis varius quam quisque id diam vel. Aenean euismod elementum nisi quis eleifend quam. Semper risus in hendrerit gravida rutrum quisque non tellus. Fames ac turpis egestas sed tempus. Integer feugiat scelerisque varius morbi enim nunc faucibus a pellentesque. Suscipit adipiscing bibendum est ultricies integer quis auctor. Sodales ut etiam sit amet nisl purus in. Id faucibus nisl tincidunt eget nullam. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Eleifend quam adipiscing vitae proin. Pellentesque adipiscing commodo elit at imperdiet dui accumsan.";

        private static readonly byte[] Input = Encoding.Default.GetBytes(Text);
        private readonly ITestOutputHelper _output;

        public CompressionStreamTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory(Skip = "Default GZip implementation doesn't write timestamp into header.")]
        [InlineData(Sys.CompressionLevel.NoCompression, CompressionLevel.None)]
        [InlineData(Sys.CompressionLevel.Fastest, CompressionLevel.BestSpeed)]
        [InlineData(Sys.CompressionLevel.Optimal, CompressionLevel.Default)]
        public void CompareGZipOutput(Sys.CompressionLevel levelReferenced, CompressionLevel levelTestable)
        {
            var input = Encoding.ASCII.GetBytes("1234567890");
            CompareOutput(input, new SystemGZipStreamFactory(levelReferenced), new GZipStreamFactory(levelTestable));
        }

        [Theory]
        [InlineData(Sys.CompressionLevel.NoCompression, CompressionLevel.None)]
        [InlineData(Sys.CompressionLevel.Fastest, CompressionLevel.BestSpeed)]
        [InlineData(Sys.CompressionLevel.Optimal, CompressionLevel.Default)]
        public void CompareDeflateOutput(Sys.CompressionLevel levelReferenced, CompressionLevel levelTestable)
        {
            CompareOutput(Input, new SystemDeflateStreamFactory(levelReferenced), new DeflateStreamFactory(levelTestable));
        }

        private void CompareOutput(byte[] data, StreamFactory factory1, StreamFactory factory2)
        {
            var expected = CompressBytes(factory1.CreateCompressor, data);

            var actual = CompressBytes(factory2.CreateCompressor, data);

            Assert.Equal<byte>(expected, actual);
            _output.WriteLine("Compression: {0} -> {1}", data.Length, expected.Length);
        }

        [Theory]
        [InlineData(Sys.CompressionLevel.NoCompression, CompressionLevel.None)]
        [InlineData(Sys.CompressionLevel.Fastest, CompressionLevel.BestSpeed)]
        [InlineData(Sys.CompressionLevel.Optimal, CompressionLevel.Default)]
        public void DoGZipCrossComparison(Sys.CompressionLevel levelReferenced, CompressionLevel levelTestable)
        {
            DoCrossComparison(Input, new SystemGZipStreamFactory(levelReferenced), new GZipStreamFactory(levelTestable));
        }

        [Theory]
        [InlineData(Sys.CompressionLevel.NoCompression, CompressionLevel.None)]
        [InlineData(Sys.CompressionLevel.Fastest, CompressionLevel.BestSpeed)]
        [InlineData(Sys.CompressionLevel.Optimal, CompressionLevel.Default)]
        public void DoDeflateCrossComparison(Sys.CompressionLevel levelReferenced, CompressionLevel levelTestable)
        {
            DoCrossComparison(Input, new SystemDeflateStreamFactory(levelReferenced), new DeflateStreamFactory(levelTestable));
        }

        private void DoCrossComparison(byte[] data, StreamFactory factory1, StreamFactory factory2)
        {
            var compression1 = CompressBytes(factory1.CreateCompressor, data);
            var result1 = DecompressBytes(factory2.CreateDecompressor, compression1, data.Length);

            var compression2 = CompressBytes(factory2.CreateCompressor, data);
            var result2 = DecompressBytes(factory1.CreateDecompressor, compression2, data.Length);

            Assert.Equal<byte>(result1, result2);
        }

        private byte[] CompressBytes(Func<Stream, Stream> compressorFactory, byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var stream = compressorFactory(ms))
                {
                    stream.Write(data, 0, data.Length);
                }

                return ms.ToArray();
            }
        }

        private byte[] DecompressBytes(Func<Stream, Stream> decompressorFactory, byte[] data, int originLength)
        {
            var result = new byte[originLength];
            using (var ms = new MemoryStream(data))
            {
                using (var stream = decompressorFactory(ms))
                {
                    stream.Read(result, 0, originLength);
                }

                return result;
            }
        }
    }
}