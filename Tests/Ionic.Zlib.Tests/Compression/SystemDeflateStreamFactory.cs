using System.IO;

namespace Ionic.Zlib.Compression
{
    internal class SystemDeflateStreamFactory : StreamFactory
    {
        private readonly System.IO.Compression.CompressionLevel _level;

        public SystemDeflateStreamFactory(System.IO.Compression.CompressionLevel level)
        {
            _level = level;
        }

        public override Stream CreateCompressor(Stream baseStream)
        {
            return new System.IO.Compression.DeflateStream(baseStream, _level);
        }

        public override Stream CreateDecompressor(Stream baseStream)
        {
            return new System.IO.Compression.DeflateStream(baseStream, System.IO.Compression.CompressionMode.Decompress);
        }
    }
}