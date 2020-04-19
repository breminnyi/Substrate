using System;
using System.Linq;

namespace Ionic.Zlib
{
    public class Crc8
    {
        public static byte CalculateSimple(byte[] input, ushort generator)
        {
            ushort current = 0;
            input = input.Append((byte) 0).ToArray();
            for (var index = 0; index < input.Length; index++)
            {
                for (var offset = 7; offset >=0; offset--)
                {
                    if ((current & 0b1000_0000) == 0b1000_0000) // if MSB == 1
                    {
                        current = (ushort) ((current << 1) + ((input[index] >> offset) & 0x1));
                        current = (ushort) (current ^ generator);
                    }
                    else
                    {
                        current = (ushort) ((current << 1) + ((input[index] >> offset) & 0x1));
                    }
                }
            }

            return (byte) current;
        }
    }
}