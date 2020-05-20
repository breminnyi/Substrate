using System.IO;

namespace Ionic.Zlib.Compression
{
    public abstract class StreamFactory
    {
        public abstract Stream CreateCompressor(Stream baseStream);
        public abstract Stream CreateDecompressor(Stream baseStream);
    }
}