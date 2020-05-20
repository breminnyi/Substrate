using System.IO;

namespace Ionic.Zlib.Compression
{
    internal class SystemGZipStreamFactory : StreamFactory
    {
        private readonly System.IO.Compression.CompressionLevel _level;

        public SystemGZipStreamFactory(System.IO.Compression.CompressionLevel level)
        {
            _level = level;
        }

        public override Stream CreateCompressor(Stream baseStream)
        {
            return new System.IO.Compression.GZipStream(baseStream, _level);
        }

        public override Stream CreateDecompressor(Stream baseStream)
        {
            return new System.IO.Compression.GZipStream(baseStream, System.IO.Compression.CompressionMode.Decompress);
        }
    }
}