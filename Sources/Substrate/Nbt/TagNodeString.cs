using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// An NBT node representing a string tag type.
    /// </summary>
    public sealed class TagNodeString : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeString ToTagString () 
        {
            return this; 
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_STRING tag type.</returns>
        public override TagType GetTagType () 
        { 
            return TagType.TAG_STRING;
        }

        /// <summary>
        /// Gets or sets a string of tag data.
        /// </summary>
        public string Data { get; set; } = "";

        /// <summary>
        /// Gets the length of the stored string.
        /// </summary>
        public int Length
        {
            get { return Data.Length; }
        }

        /// <summary>
        /// Constructs a new byte array node with an empty string.
        /// </summary>
        public TagNodeString () { }

        /// <summary>
        /// Constructs a new string node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeString (string d)
        {
            Data = d;
            if (Data == null)
                Data = "";
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new string node representing the same data.</returns>
        public override TagNode Copy ()
        {
            return new TagNodeString(Data);
        }

        /// <summary>
        /// Gets a string representation of the node's data.
        /// </summary>
        /// <returns>String representation of the node's data.</returns>
        public override string ToString ()
        {
            return Data;
        }

        /// <summary>
        /// Converts a system string to a string node representing the same data.
        /// </summary>
        /// <param name="s">A string.</param>
        /// <returns>A new string node containing the given value.</returns>
        public static implicit operator TagNodeString (string s)
        {
            return new TagNodeString(s);
        }

        /// <summary>
        /// Converts a string node to a system string representing the same data.
        /// </summary>
        /// <param name="s">A string node.</param>
        /// <returns>A system string set to the node's data.</returns>
        public static implicit operator string (TagNodeString s)
        {
            return s.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var gzBytes = Encoding.UTF8.GetBytes(Data);
            var lenBytes = BitConverter.GetBytes((short) gzBytes.Length).EnsureBigEndian();
            stream.Write(lenBytes, 0, 2);
            stream.Write(gzBytes, 0, gzBytes.Length);
        }

        internal override async Task SerializeValueAsync(Stream stream)
        {
            var gzBytes = Encoding.UTF8.GetBytes(Data);
            var lenBytes = BitConverter.GetBytes((short) gzBytes.Length).EnsureBigEndian();
            await stream.WriteAsync(lenBytes, 0, 2).ConfigureAwait(false);
            await stream.WriteAsync(gzBytes, 0, gzBytes.Length).ConfigureAwait(false);
        }

        protected internal override void Deserialize(Stream stream)
        {
            var lenBytes = new byte[2];
            stream.Read(lenBytes, 0, 2);
            var len = BitConverter.ToInt16(lenBytes.EnsureBigEndian(), 0);
            if (len < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            var strBytes = new byte[len];
            stream.Read(strBytes, 0, len);
            Data = Encoding.UTF8.GetString(strBytes);
        }

        public override async Task DeserializeAsync(Stream stream)
        {
            var lenBytes = new byte[2];
            await stream.ReadAsync(lenBytes, 0, 2).ConfigureAwait(false);
            var len = BitConverter.ToInt16(lenBytes.EnsureBigEndian(), 0);
            if (len < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            var strBytes = new byte[len];
            await stream.ReadAsync(strBytes, 0, len).ConfigureAwait(false);
            Data = Encoding.UTF8.GetString(strBytes);
        }
    }
}