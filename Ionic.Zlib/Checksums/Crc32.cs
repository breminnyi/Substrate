// Crc32.cs
// ------------------------------------------------------------------
//
// Copyright (c) 2006-2009 Dino Chiesa and Microsoft Corporation.
// All rights reserved.
//
// This code module is part of DotNetZip, a zipfile class library.
//
// ------------------------------------------------------------------
//
// This code is licensed under the Microsoft Public License.
// See the file License.txt for the license details.
// More info on: http://dotnetzip.codeplex.com
//
// ------------------------------------------------------------------
//
// last saved (in emacs):
// Time-stamp: <2010-January-16 13:16:27>
//
// ------------------------------------------------------------------
//
// Implements the CRC algorithm, which is used in zip files.  The zip format calls for
// the zipfile to contain a CRC for the unencrypted byte stream of each file.
//
// It is based on example source code published at
//    http://www.vbaccelerator.com/home/net/code/libraries/CRC32/Crc32_zip_CRC32_CRC32_cs.asp
//
// This implementation adds a tweak of that code for use within zip creation.  While
// computing the CRC we also compress the byte stream, in the same read loop. This
// avoids the need to read through the uncompressed stream twice - once to compute CRC
// and another time to compress.
//
// ------------------------------------------------------------------


using System;
using System.IO;


namespace Ionic.Zlib.Checksums
{
    /// <summary>
    /// Calculates a 32-bit Cyclic Redundancy Checksum (CRC) using the same polynomial
    /// used by Zip. This type is used internally by DotNetZip; it is generally not used
    /// directly by applications wishing to create, read, or manipulate zip archive
    /// files.
    /// </summary>
    internal class Crc32 : ICrcCalculator
    {
        private const int BufferSize = 8192;
        private const uint Initial = 0xFFFFFFFF;
        private const uint Polynomial = 0xEDB88320;

        private static readonly uint[] LookupTable;
        private uint _result = Initial;

        static Crc32()
        {
            // pre-initialize the CRC table for speed of lookup.
            LookupTable = GenerateLookup();
        }

        /// <summary>
        /// indicates the total number of bytes read on the CRC stream.
        /// This is used when writing the ZipDirEntry when compressing files.
        /// </summary>
        public long TotalBytesRead { get; private set; }

        /// <summary>
        /// Indicates the current CRC for all blocks slurped in.
        /// </summary>
        // return one's complement of the running result
        public int Crc32Result => unchecked((int) (~_result));

        /// <summary>
        /// Returns the CRC32 for the specified stream.
        /// </summary>
        /// <param name="input">The stream over which to calculate the CRC32</param>
        /// <returns>the CRC32 calculation</returns>
        public int GetCrc32(Stream input)
        {
            return GetCrc32AndCopy(input, null);
        }

        /// <summary>
        /// Returns the CRC32 for the specified stream, and writes the input into the
        /// output stream.
        /// </summary>
        /// <param name="input">The stream over which to calculate the CRC32</param>
        /// <param name="output">The stream into which to deflate the input</param>
        /// <returns>the CRC32 calculation</returns>
        public int GetCrc32AndCopy(Stream input, Stream output)
        {
            if (input == null)
                throw new ZlibException("The input stream must not be null.");

            unchecked
            {
                //UInt32 crc32Result;
                //crc32Result = 0xFFFFFFFF;
                var buffer = new byte[BufferSize];
                var readSize = BufferSize;

                TotalBytesRead = 0;
                var count = input.Read(buffer, 0, readSize);
                if (output != null) output.Write(buffer, 0, count);
                TotalBytesRead += count;
                while (count > 0)
                {
                    SlurpBlock(buffer, 0, count);
                    count = input.Read(buffer, 0, readSize);
                    if (output != null) output.Write(buffer, 0, count);
                    TotalBytesRead += count;
                }

                return (int) (~_result);
            }
        }

        /// <summary>
        /// Get the CRC32 for the given (word, byte) combo. This is a computation
        /// defined by PKzip.
        /// </summary>
        /// <param name="initial">The word to start with.</param>
        /// <param name="value">The byte to combine it with.</param>
        /// <returns>The CRC-ized result.</returns>
        public int Compute(int initial, byte value)
        {
            return ComputeInternal((uint) initial, value);
        }

        internal int ComputeInternal(uint initial, byte value)
        {
            return (int) (LookupTable[(initial ^ value) & 0xFF] ^ (initial >> 8));
        }

        /// <summary>
        /// Update the value for the running CRC32 using the given block of bytes.
        /// This is useful when using the CRC32() class in a Stream.
        /// </summary>
        /// <param name="block">block of bytes to slurp</param>
        /// <param name="offset">starting point in the block</param>
        /// <param name="count">how many bytes within the block to slurp</param>
        public void SlurpBlock(byte[] block, int offset, int count)
        {
            if (block == null)
                throw new ZlibException("The data buffer must not be null.");

            for (var i = 0; i < count; i++)
            {
                var x = offset + i;
                _result = ((_result) >> 8) ^ LookupTable[(block[x]) ^ ((_result) & 0x000000FF)];
            }

            TotalBytesRead += count;
        }

        /// <summary>
        /// Combines the given CRC32 value with the current running total.
        /// </summary>
        /// <remarks>
        /// This is useful when using a divide-and-conquer approach to calculating a CRC.
        /// Multiple threads can each calculate a CRC32 on a segment of the data, and then
        /// combine the individual CRC32 values at the end.
        /// </remarks>
        /// <param name="crc">the crc value to be combined with this one</param>
        /// <param name="length">the length of data the CRC value was calculated on</param>
        public void Combine(int crc, int length)
        {
            if (length == 0)
                return;

            var even = new uint[32]; // even-power-of-two zeros operator
            var odd = new uint[32]; // odd-power-of-two zeros operator

            // put operator for one zero bit in odd
            odd[0] = Polynomial; // the CRC-32 polynomial
            uint row = 1;
            for (var i = 1; i < 32; i++)
            {
                odd[i] = row;
                row <<= 1;
            }

            // put operator for two zero bits in even
            Gf2MatrixSquare(even, odd);

            // put operator for four zero bits in odd
            Gf2MatrixSquare(odd, even);

            var len2 = (uint) length;

            var crc1 = ~_result;
            // apply len2 zeros to crc1 (first square will put the operator for one
            // zero byte, eight zero bits, in even)
            do
            {
                // apply zeros operator for this bit of len2
                Gf2MatrixSquare(even, odd);

                if ((len2 & 1) == 1)
                    crc1 = Gf2MatrixTimes(even, crc1);
                len2 >>= 1;

                if (len2 == 0)
                    break;

                // another iteration of the loop with odd and even swapped
                Gf2MatrixSquare(odd, even);
                if ((len2 & 1) == 1)
                    crc1 = Gf2MatrixTimes(odd, crc1);
                len2 >>= 1;
            } while (len2 != 0);

            _result = ~(crc1 ^ unchecked((uint) crc));
            TotalBytesRead += length;
        }

        public void Combine(Crc32 other)
        {
            Combine(other.Crc32Result, (int) other.TotalBytesRead);
        }

        private static uint[] GenerateLookup()
        {
            // PKZip specifies CRC32 with a polynomial of 0xEDB88320;
            // This is also the CRC-32 polynomial used bby Ethernet, FDDI,
            // bzip2, gzip, and others.
            // Often the polynomial is shown reversed as 0x04C11DB7.
            // For more details, see http://en.wikipedia.org/wiki/Cyclic_redundancy_check
            var lookup = new uint[256];
            for (uint index = 0; index < 256; index++)
            {
                var crc = index;
                for (var bit = 0; bit < 8; bit++)
                {
                    var msbSet = (crc & 1) == 1;
                    crc >>= 1;
                    if (msbSet)
                    {
                        crc ^= Polynomial;
                    }
                }

                lookup[index] = crc;
            }

            return lookup;
        }

        private uint Gf2MatrixTimes(uint[] matrix, uint vec)
        {
            uint sum = 0;
            var i = 0;
            while (vec != 0)
            {
                if ((vec & 0x01) == 0x01)
                {
                    sum ^= matrix[i];
                }

                vec >>= 1;
                i++;
            }

            return sum;
        }

        private void Gf2MatrixSquare(uint[] square, uint[] mat)
        {
            for (var i = 0; i < 32; i++)
            {
                square[i] = Gf2MatrixTimes(mat, mat[i]);
            }
        }

        byte[] ICrcCalculator.Result => BitConverter.GetBytes(Crc32Result);

        long ICrcCalculator.BytesRead => TotalBytesRead;

        void ICrcCalculator.Advance(byte[] block, int offset, int count)
        {
            SlurpBlock(block, offset, count);
        }

        void ICrcCalculator.Reset()
        {
            _result = Initial;
            TotalBytesRead = 0;
        }
    }
}