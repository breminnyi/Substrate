using System;
using System.IO;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// An NBT node representing an unsigned byte array tag type.
    /// </summary>
    public sealed class TagNodeByteArray : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeByteArray ToTagByteArray () 
        {
            return this;
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_BYTE_ARRAY tag type.</returns>
        public override TagType GetTagType ()
        {
            return TagType.TAG_BYTE_ARRAY; 
        }

        /// <summary>
        /// Gets or sets a byte array of tag data.
        /// </summary>
        public byte[] Data { get; set; }

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
        public TagNodeByteArray () { }

        /// <summary>
        /// Constructs a new byte array node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeByteArray (byte[] d)
        {
            Data = d;
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new byte array node representing the same data.</returns>
        public override TagNode Copy ()
        {
            byte[] arr = new byte[Data.Length];
            Data.CopyTo(arr, 0);

            return new TagNodeByteArray(arr);
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
        /// Gets or sets a single byte at the specified index.
        /// </summary>
        /// <param name="index">Valid index within stored byte array.</param>
        /// <returns>The byte value at the given index of the stored byte array.</returns>
        public byte this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        /// Converts a system byte array to a byte array node representing the same data.
        /// </summary>
        /// <param name="b">A byte array.</param>
        /// <returns>A new byte array node containing the given value.</returns>
        public static implicit operator TagNodeByteArray (byte[] b)
        {
            return new TagNodeByteArray(b);
        }

        /// <summary>
        /// Converts a byte array node to a system byte array representing the same data.
        /// </summary>
        /// <param name="b">A byte array node.</param>
        /// <returns>A system byte array set to the node's data.</returns>
        public static implicit operator byte[] (TagNodeByteArray b)
        {
            return b.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            stream.Write(lenBytes, 0, 4);
            stream.Write(Data, 0, Data.Length);
        }

        internal override async Task SerializeValueAsync(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            await stream.WriteAsync(lenBytes, 0, 4).ConfigureAwait(false);
            await stream.WriteAsync(Data, 0, Data.Length).ConfigureAwait(false);
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

            var data = new byte[length];
            stream.Read(data, 0, length);
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

            var data = new byte[length];
            await stream.ReadAsync(data, 0, length).ConfigureAwait(false);
            Data = data;
        }
    }
}