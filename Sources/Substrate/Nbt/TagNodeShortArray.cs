﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    public sealed class TagNodeShortArray : TagNode
    {
        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeShortArray ToTagShortArray ()
        {
            return this;
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_SHORT_ARRAY tag type.</returns>
        public override TagType GetTagType ()
        {
            return TagType.TAG_SHORT_ARRAY;
        }

        /// <summary>
        /// Gets or sets an short array of tag data.
        /// </summary>
        public short[] Data { get; set; }

        /// <summary>
        /// Gets the length of the stored short array.
        /// </summary>
        public int Length
        {
            get { return Data.Length; }
        }

        /// <summary>
        /// Constructs a new short array node with a null data value.
        /// </summary>
        public TagNodeShortArray () { }

        /// <summary>
        /// Constructs a new short array node.
        /// </summary>
        /// <param name="d">The value to set the node's tag data value.</param>
        public TagNodeShortArray (short[] d)
        {
            Data = d;
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new int array node representing the same data.</returns>
        public override TagNode Copy ()
        {
            short[] arr = new short[Data.Length];
            Data.CopyTo(arr, 0);

            return new TagNodeShortArray(arr);
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
        /// Gets or sets a single short at the specified index.
        /// </summary>
        /// <param name="index">Valid index within stored short array.</param>
        /// <returns>The short value at the given index of the stored short array.</returns>
        public short this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        /// <summary>
        /// Converts a system short array to a short array node representing the same data.
        /// </summary>
        /// <param name="i">A short array.</param>
        /// <returns>A new short array node containing the given value.</returns>
        public static implicit operator TagNodeShortArray (short[] i)
        {
            return new TagNodeShortArray(i);
        }

        /// <summary>
        /// Converts an short array node to a system short array representing the same data.
        /// </summary>
        /// <param name="i">A short array node.</param>
        /// <returns>A system short array set to the node's data.</returns>
        public static implicit operator short[] (TagNodeShortArray i)
        {
            return i.Data;
        }

        internal override void SerializeValue(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            stream.Write(lenBytes, 0, 4);
            var data = new byte[Length * 2];
            for (var i = 0; i < Length; i++)
            {
                var buffer = BitConverter.GetBytes(Data[i]).EnsureBigEndian();
                Array.Copy(buffer, 0, data, i * 2, 2);
            }

            stream.Write(data, 0, data.Length);
        }

        internal override async Task SerializeValueAsync(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Length).EnsureBigEndian();
            await stream.WriteAsync(lenBytes, 0, 4).ConfigureAwait(false);
            var data = new byte[Length * 2];
            for (var i = 0; i < Length; i++)
            {
                var buffer = BitConverter.GetBytes(Data[i]).EnsureBigEndian();
                Array.Copy(buffer, 0, data, i * 2, 2);
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

            var data = new short[length];
            var buffer = new byte[2];
            for (var i = 0; i < length; i++)
            {
                stream.Read(buffer, 0, 2);
                data[i] = BitConverter.ToInt16(buffer.EnsureBigEndian(), 0);
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

            var data = new short[length];
            var buffer = new byte[2];
            for (var i = 0; i < length; i++)
            {
                await stream.ReadAsync(buffer, 0, 2).ConfigureAwait(false);
                data[i] = BitConverter.ToInt16(buffer.EnsureBigEndian(), 0);
            }

            Data = data;
        }
    }
}