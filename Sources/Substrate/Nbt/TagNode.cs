using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Substrate.Core;

namespace Substrate.Nbt
{
    /// <summary>
    /// An abstract base class representing a node in an NBT tree.
    /// </summary>
    public abstract class TagNode : ICopyable<TagNode>
    {
        /// <summary>
        /// Convert this node to a byte tag type if supported.
        /// </summary>
        /// <returns>A new byte node.</returns>
        public virtual TagNodeByte ToTagByte()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a short tag type if supported.
        /// </summary>
        /// <returns>A new short node.</returns>
        public virtual TagNodeShort ToTagShort()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to an int tag type if supported.
        /// </summary>
        /// <returns>A new int node.</returns>
        public virtual TagNodeInt ToTagInt()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a long tag type if supported.
        /// </summary>
        /// <returns>A new long node.</returns>
        public virtual TagNodeLong ToTagLong()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a float tag type if supported.
        /// </summary>
        /// <returns>A new float node.</returns>
        public virtual TagNodeFloat ToTagFloat()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a double tag type if supported.
        /// </summary>
        /// <returns>A new double node.</returns>
        public virtual TagNodeDouble ToTagDouble()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a byte array tag type if supported.
        /// </summary>
        /// <returns>A new byte array node.</returns>
        public virtual TagNodeByteArray ToTagByteArray()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a string tag type if supported.
        /// </summary>
        /// <returns>A new string node.</returns>
        public virtual TagNodeString ToTagString()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a list tag type if supported.
        /// </summary>
        /// <returns>A new list node.</returns>
        public virtual TagNodeList ToTagList()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Convert this node to a compound tag type if supported.
        /// </summary>
        /// <returns>A new compound node.</returns>
        public virtual TagNodeCompound ToTagCompound()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Conver this node to an int array tag type if supported.
        /// </summary>
        /// <returns>A new int array node.</returns>
        public virtual TagNodeIntArray ToTagIntArray()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Conver this node to a long array tag type if supported.
        /// </summary>
        /// <returns>A new long array node.</returns>
        public virtual TagNodeLongArray ToTagLongArray()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Conver this node to a short array tag type if supported.
        /// </summary>
        /// <returns>A new short array node.</returns>
        public virtual TagNodeShortArray ToTagShortArray()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the underlying tag type of the node.
        /// </summary>
        /// <returns>An NBT tag type.</returns>
        public virtual TagType GetTagType()
        {
            return TagType.TAG_END;
        }

        /// <summary>
        /// Checks if this node is castable to another node of a given tag type.
        /// </summary>
        /// <param name="type">An NBT tag type.</param>
        /// <returns>Status indicating whether this object could be cast to a node type represented by the given tag type.</returns>
        public virtual bool IsCastableTo(TagType type)
        {
            return type == GetTagType();
        }

        /// <summary>
        /// Makes a deep copy of the NBT node.
        /// </summary>
        /// <returns>A new NBT node.</returns>
        public virtual TagNode Copy()
        {
            return null;
        }

        public void Serialize(string name, Stream stream)
        {
            stream.WriteByte((byte) GetTagType());
            new TagNodeString(name).SerializeValue(stream);
            SerializeValue(stream);
        }

        public async Task SerializeAsync(string name, Stream stream)
        {
            await stream.WriteAsync(new[] {(byte) GetTagType()}, 0, 1).ConfigureAwait(false);
            await new TagNodeString(name).SerializeValueAsync(stream).ConfigureAwait(false);
            await SerializeValueAsync(stream).ConfigureAwait(false);
        }

        internal abstract void SerializeValue(Stream stream);

        internal virtual Task SerializeValueAsync(Stream stream) => Task.FromException(new NotImplementedException());

        protected internal abstract void Deserialize(Stream stream);

        public virtual Task DeserializeAsync(Stream stream) => Task.FromException(new NotImplementedException());
    }
}