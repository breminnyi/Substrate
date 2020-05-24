using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Substrate.Core;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// Contains the root node of an NBT tree and handles IO of tree nodes.
    /// </summary>
    /// <remarks>
    /// NBT, or Named Byte Tag, is a tree-based data structure for storing most Minecraft data.
    /// NBT_Tree is more of a helper class for NBT trees that handles reading and writing nodes to data streams.
    /// Most of the API takes a TagValue or derived node as the root of the tree, rather than an NBT_Tree object itself.
    /// </remarks>
    public class NbtTree : ICopyable<NbtTree>
    {
        /// <summary>
        /// Gets the root node of this tree.
        /// </summary>
        public TagNodeCompound Root { get; private set; }

        /// <summary>
        /// Gets or sets the name of the tree's root node.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Constructs a wrapper around a new NBT tree with an empty root node.
        /// </summary>
        public NbtTree()
        {
            Root = new TagNodeCompound();
        }

        /// <summary>
        /// Constructs a wrapper around another NBT tree.
        /// </summary>
        /// <param name="tree">The root node of an NBT tree.</param>
        public NbtTree(TagNodeCompound tree)
        {
            Root = tree;
        }

        /// <summary>
        /// Constructs a wrapper around another NBT tree and gives it a name.
        /// </summary>
        /// <param name="tree">The root node of an NBT tree.</param>
        /// <param name="name">The name for the root node.</param>
        public NbtTree(TagNodeCompound tree, string name)
        {
            Root = tree;
            Name = name;
        }

        /// <summary>
        /// Constructs and wrapper around a new NBT tree parsed from a source data stream.
        /// </summary>
        /// <param name="s">An open, readable data stream containing NBT data.</param>
        public NbtTree(Stream s)
        {
            ReadFrom(s);
        }

        /// <summary>
        /// Rebuild the internal NBT tree from a source data stream.
        /// </summary>
        /// <param name="stream">An open, readable data stream containing NBT data.</param>
        public void ReadFrom(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            var type = (TagType) stream.ReadByte();
            if (type == TagType.TAG_COMPOUND)
            {
                var nameTag = new TagNodeString();
                nameTag.Deserialize(stream);
                Name = nameTag.Data; // name

                var rootTag = TagNodeFactory.Instance.Create(type);
                rootTag.Deserialize(stream);
                Root = (TagNodeCompound) rootTag;
            }
            else
            {
                Root = null;
            }
        }
        
        /// <summary>
        /// Rebuild the internal NBT tree from a source data stream.
        /// </summary>
        /// <param name="stream">An open, readable data stream containing NBT data.</param>
        public async Task ReadFromAsync(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[1];
            await stream.ReadAsync(buffer, 0, 1).ConfigureAwait(false);
            var type = (TagType) buffer[0];
            if (type == TagType.TAG_COMPOUND)
            {
                var nameTag = new TagNodeString();
                await nameTag.DeserializeAsync(stream).ConfigureAwait(false);
                Name = nameTag.Data; // name

                var rootTag = TagNodeFactory.Instance.Create(type);
                await rootTag.DeserializeAsync(stream).ConfigureAwait(false);
                Root = (TagNodeCompound) rootTag;
            }
            else
            {
                Root = null;
            }
        }

        /// <summary>
        /// Writes out the internal NBT tree to a destination data stream.
        /// </summary>
        /// <param name="stream">An open, writable data stream.</param>
        public void WriteTo(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (Root == null) throw new InvalidOperationException("NBT tree not initialized yet.");
            Root.Serialize(Name, stream);
        }
        
        public Task WriteToAsync(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (Root == null) throw new InvalidOperationException("NBT tree not initialized yet.");
            return Root.SerializeAsync(Name, stream);
        }

        #region ICopyable<NBT_Tree> Members

        /// <summary>
        /// Creates a deep copy of the NBT_Tree and underlying nodes.
        /// </summary>
        /// <returns>A new NBT_tree.</returns>
        public NbtTree Copy()
        {
            var tree = new NbtTree();
            tree.Root = Root.Copy() as TagNodeCompound;

            return tree;
        }

        #endregion
    }
}