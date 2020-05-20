using System.IO;

namespace Ionic.Zlib.Compression
{
    internal class DeflateStreamFactory : StreamFactory
    {
        private readonly CompressionLevel _level;

        public DeflateStreamFactory(CompressionLevel level)
        {
            _level = level;
        }

        public override Stream CreateCompressor(Stream baseStream)
        {
            return new DeflateStream(baseStream, CompressionMode.Compress, _level);
        }

        public override Stream CreateDecompressor(Stream baseStream)
        {
            return new DeflateStream(baseStream, CompressionMode.Decompress, _level);
        }
    }
}