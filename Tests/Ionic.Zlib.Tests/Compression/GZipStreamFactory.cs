using System.IO;

namespace Ionic.Zlib.Compression
{
    internal class GZipStreamFactory : StreamFactory
    {
        private readonly CompressionLevel _level;

        public GZipStreamFactory(CompressionLevel level)
        {
            _level = level;
        }

        public override Stream CreateCompressor(Stream baseStream)
        {
            return new GZipStream(baseStream, CompressionMode.Compress, _level);
        }

        public override Stream CreateDecompressor(Stream baseStream)
        {
            return new GZipStream(baseStream, CompressionMode.Decompress, _level);
        }
    }
}