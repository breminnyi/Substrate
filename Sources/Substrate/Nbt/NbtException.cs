using System;

namespace Substrate.Nbt
{
    // TODO: Revise exceptions?

    public class NbtException : Exception
    {
        public const String MSG_GZIP_ENDOFSTREAM = "Gzip Error: Unexpected end of stream";

        public const String MSG_READ_NEG = "Read Error: Negative length";
        public const String MSG_READ_TYPE = "Read Error: Invalid value type";

        public NbtException()
        {
        }

        public NbtException(String msg) : base(msg)
        {
        }

        public NbtException(String msg, Exception innerException) : base(msg, innerException)
        {
        }
    }

    public class InvalidNBTObjectException : Exception
    {
    }

    public class InvalidTagException : Exception
    {
    }

    public class InvalidValueException : Exception
    {
    }
}