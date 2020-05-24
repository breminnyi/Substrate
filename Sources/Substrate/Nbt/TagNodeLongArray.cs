using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    public sealed class TagNodeLongArray : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeLongArray ToTagLongArray () 
        {
            return this;
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_LONG_ARRAY tag type.</returns>
        public override TagType GetTagType ()
        {
            return TagType.TAG_LONG_ARRAY; 
        }

        /// <summary>
        /// Gets or sets an long array of tag data.
        /// </summary>
        public long[] Data { get; set; }

        /// <summary>
        /// Gets the length of the stored byte array.
        /// </summary>
        public int Length
        {
            get { return Data.Length; }
        }

        /// <summary>
        /// Constructs a new byte array node with a null data value.
        /// </summary>
        public TagNodeLongArray() { }

        /// <summary>
        /// Constructs a new byte array node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeLongArray(long[] d)
        {
            Data = d;
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new long array node representing the same data.</returns>
        public override TagNode Copy ()
        {
            long[] arr = new long[Data.Length];
            Data.CopyTo(arr, 0);

            return new TagNodeLongArray(arr);
        }

        /// <summary>
        /// Gets a string representation of the node's data.
        /// </summary>
        /// <returns>String representation of the node's data.</returns>
        public override string ToString ()
        {
            return Data.ToString();
        }

        /// <summary>
        /// Gets or sets a single long at the specified index.
        /// </summary>
        /// <param name="index">Valid index within stored long array.</param>
        /// <returns>The long value at the given index of the stored byte array.</returns>
        public long this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        /// Converts a system long array to a long array node representing the same data.
        /// </summary>
        /// <param name="i">A long array.</param>
        /// <returns>A new long array node containing the given value.</returns>
        public static implicit operator TagNodeLongArray(long[] i)
        {
            return new TagNodeLongArray(i);
        }

        /// <summary>
        /// Converts an long array node to a system long array representing the same data.
        /// </summary>
        /// <param name="i">An long array node.</param>
        /// <returns>A system long array set to the node's data.</returns>
        public static implicit operator long[] (TagNodeLongArray i)
        {
            return i.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            stream.Write(lenBytes, 0, 4);
            var data = new byte[Length * 8];
            for (var i = 0; i < Length; i++)
            {
                var buffer = BitConverter.GetBytes(Data[i]).EnsureBigEndian();
                Array.Copy(buffer, 0, data, i * 8, 8);
            }

            stream.Write(data, 0, data.Length);
        }

        internal override async Task SerializeValueAsync(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            await stream.WriteAsync(lenBytes, 0, 4).ConfigureAwait(false);
            var data = new byte[Length * 8];
            for (var i = 0; i < Length; i++)
            {
                var buffer = BitConverter.GetBytes(Data[i]).EnsureBigEndian();
                Array.Copy(buffer, 0, data, i * 8, 8);
            }

            await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }

        protected internal override void Deserialize(Stream stream)
        {
            var lenBytes = new byte[4];
            stream.Read(lenBytes, 0, 4);
            var length = BitConverter.ToInt32(lenBytes.EnsureBigEndian(), 0);
            if (length < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            var data = new long[length];
            var buffer = new byte[8];
            for (var i = 0; i < length; i++)
            {
                stream.Read(buffer, 0, 8);
                data[i] = BitConverter.ToInt64(buffer.EnsureBigEndian(), 0);
            }

            Data = data;
        }

        public override async Task DeserializeAsync(Stream stream)
        {
            var lenBytes = new byte[4];
            await stream.ReadAsync(lenBytes, 0, 4).ConfigureAwait(false);
            var length = BitConverter.ToInt32(lenBytes.EnsureBigEndian(), 0);
            if (length < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            var data = new long[length];
            var buffer = new byte[8];
            for (var i = 0; i < length; i++)
            {
                await stream.ReadAsync(buffer, 0, 8).ConfigureAwait(false);
                data[i] = BitConverter.ToInt64(buffer.EnsureBigEndian(), 0);
            }

            Data = data;
        }
    }
}
