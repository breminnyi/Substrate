using System;

namespace Ionic.Zlib.Checksums
{
    public interface ICrcCalculator
    {
        byte[] Result { get; }
        long BytesRead { get; }

        void Advance(byte[] block, int offset, int count);
        void Reset();
    }

    public static class CrcCalculatorExtensions
    {
        public static byte[] Compute(this ICrcCalculator calculator, byte[] block)
        {
            return Compute(calculator, block, 0, block == null ? -1 : block.Length);
        }

        public static byte[] Compute(this ICrcCalculator calculator, byte[] block, int offset, int count)
        {
            calculator.Reset();
            calculator.Advance(block, offset, count);
            return calculator.Result;
        }
    }
}