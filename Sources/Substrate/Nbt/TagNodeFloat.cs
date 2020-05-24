﻿using System;
using System.IO;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// An NBT node representing a single-precision floating point tag type.
    /// </summary>
    public sealed class TagNodeFloat : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeFloat ToTagFloat () 
        { 
            return this; 
        }

        /// <summary>
        /// Converts the node to a new double node.
        /// </summary>
        /// <returns>A double node representing the same data.</returns>
        public override TagNodeDouble ToTagDouble () 
        { 
            return new TagNodeDouble(Data); 
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_FLOAT tag type.</returns>
        public override TagType GetTagType () 
        { 
            return TagType.TAG_FLOAT; 
        }

        /// <summary>
        /// Checks if the node is castable to another node of a given tag type.
        /// </summary>
        /// <param name="type">An NBT tag type.</param>
        /// <returns>Status indicating whether this object could be cast to a node type represented by the given tag type.</returns>
        public override bool IsCastableTo (TagType type)
        {
            return (type == TagType.TAG_FLOAT ||
                type == TagType.TAG_DOUBLE);
        }

        /// <summary>
        /// Gets or sets a float of tag data.
        /// </summary>
        public float Data { get; set; }

        /// <summary>
        /// Constructs a new float node with a data value of 0.0.
        /// </summary>
        public TagNodeFloat () { }

        /// <summary>
        /// Constructs a new float node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeFloat (float d)
        {
            Data = d;
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new float node representing the same data.</returns>
        public override TagNode Copy ()
        {
            return new TagNodeFloat(Data);
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
        /// Converts a system float to a float node representing the same value.
        /// </summary>
        /// <param name="f">A float value.</param>
        /// <returns>A new float node containing the given value.</returns>
        public static implicit operator TagNodeFloat (float f)
        {
            return new TagNodeFloat(f);
        }

        /// <summary>
        /// Converts a float node to a system float representing the same value.
        /// </summary>
        /// <param name="f">A float node.</param>
        /// <returns>A system float set to the node's data value.</returns>
        public static implicit operator float (TagNodeFloat f)
        {
            return f.Data;
        }

        /// <summary>
        /// Converts a float node to a system double representing the same value.
        /// </summary>
        /// <param name="f">A float node.</param>
        /// <returns>A system double set to the node's data value.</returns>
        public static implicit operator double (TagNodeFloat f)
        {
            return f.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var gzBytes = BitConverter.GetBytes(Data).EnsureBigEndian();
            stream.Write(gzBytes, 0, 4);
        }

        internal override Task SerializeValueAsync(Stream stream)
        {
            var gzBytes = BitConverter.GetBytes(Data).EnsureBigEndian();
            return stream.WriteAsync(gzBytes, 0, 4);
        }

        protected internal override void Deserialize(Stream stream)
        {
            var gzBytes = new byte[4];
            stream.Read(gzBytes, 0, 4);
            Data = BitConverter.ToSingle(gzBytes.EnsureBigEndian(), 0);
        }

        public override async Task DeserializeAsync(Stream stream)
        {
            var gzBytes = new byte[4];
            await stream.ReadAsync(gzBytes, 0, 4).ConfigureAwait(false);
            Data = BitConverter.ToSingle(gzBytes.EnsureBigEndian(), 0);
        }
    }
}