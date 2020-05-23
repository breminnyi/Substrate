﻿using System;
using System.IO;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// An NBT node representing a signed short tag type.
    /// </summary>
    public sealed class TagNodeShort : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeShort ToTagShort()
        {
            return this;
        }

        /// <summary>
        /// Converts the node to a new int node.
        /// </summary>
        /// <returns>An int node representing the same data.</returns>
        public override TagNodeInt ToTagInt()
        {
            return new TagNodeInt(Data);
        }

        /// <summary>
        /// Converts the node to a new long node.
        /// </summary>
        /// <returns>A long node representing the same data.</returns>
        public override TagNodeLong ToTagLong()
        {
            return new TagNodeLong(Data);
        }

        /// <summary>
        /// Converts the node to a new float node.
        /// </summary>
        /// <returns>A float node representing the same data.</returns>
        public override TagNodeFloat ToTagFloat()
        {
            return new TagNodeFloat(Data);
        }

        /// <summary>
        /// Converts the node to a new double node.
        /// </summary>
        /// <returns>A double node representing the same data.</returns>
        public override TagNodeDouble ToTagDouble()
        {
            return new TagNodeDouble(Data);
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_SHORT tag type.</returns>
        public override TagType GetTagType()
        {
            return TagType.TAG_SHORT;
        }

        /// <summary>
        /// Checks if the node is castable to another node of a given tag type.
        /// </summary>
        /// <param name="type">An NBT tag type.</param>
        /// <returns>Status indicating whether this object could be cast to a node type represented by the given tag type.</returns>
        public override bool IsCastableTo(TagType type)
        {
            return (type == TagType.TAG_SHORT ||
                type == TagType.TAG_INT ||
                type == TagType.TAG_LONG ||
                type == TagType.TAG_FLOAT ||
                type == TagType.TAG_DOUBLE);
        }

        /// <summary>
        /// Gets or sets a short of tag data.
        /// </summary>
        public short Data { get; set; }

        /// <summary>
        /// Constructs a new short node with a data value of 0.
        /// </summary>
        public TagNodeShort() { }

        /// <summary>
        /// Constructs a new short node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeShort(short d)
        {
            Data = d;
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new short node representing the same data.</returns>
        public override TagNode Copy()
        {
            return new TagNodeShort(Data);
        }

        /// <summary>
        /// Gets a string representation of the node's data.
        /// </summary>
        /// <returns>String representation of the node's data.</returns>
        public override string ToString()
        {
            return Data.ToString();
        }

        /// <summary>
        /// Converts a system byte to a short node representing the same value.
        /// </summary>
        /// <param name="b">A byte value.</param>
        /// <returns>A new short node containing the given value.</returns>
        public static implicit operator TagNodeShort(byte b)
        {
            return new TagNodeShort(b);
        }

        /// <summary>
        /// Converts a system short to a short node representing the same value.
        /// </summary>
        /// <param name="s">A short value.</param>
        /// <returns>A new short node containing the given value.</returns>
        public static implicit operator TagNodeShort(short s)
        {
            return new TagNodeShort(s);
        }

        /// <summary>
        /// Converts a short node to a system short representing the same value.
        /// </summary>
        /// <param name="s">A short node.</param>
        /// <returns>A system short set to the node's data value.</returns>
        public static implicit operator short(TagNodeShort s)
        {
            return s.Data;
        }

        /// <summary>
        /// Converts a short node to a system int representing the same value.
        /// </summary>
        /// <param name="s">A short node.</param>
        /// <returns>A system int set to the node's data value.</returns>
        public static implicit operator int(TagNodeShort s)
        {
            return s.Data;
        }

        /// <summary>
        /// Converts a short node to a system long representing the same value.
        /// </summary>
        /// <param name="s">A short node.</param>
        /// <returns>A system long set to the node's data value.</returns>
        public static implicit operator long(TagNodeShort s)
        {
            return s.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var gzBytes = BitConverter.GetBytes(Data).EnsureBigEndian();
            stream.Write(gzBytes, 0, 2);
        }

        protected internal override void Deserialize(Stream stream)
        {
            var gzBytes = new byte[2];
            stream.Read(gzBytes, 0, 2);
            Data = BitConverter.ToInt16(gzBytes.EnsureBigEndian(), 0);
        }
    }
}