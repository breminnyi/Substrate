﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Substrate.Utilities;

namespace Substrate.Nbt
{
    /// <summary>
    /// An NBT node representing a list tag type containing other nodes.
    /// </summary>
    /// <remarks>
    /// A list node contains 0 or more nodes of the same type.  The nodes are unnamed
    /// but can be accessed by sequential index.
    /// </remarks>
    public sealed class TagNodeList : TagNode, IList<TagNode>
    {
        private List<TagNode> _items;

        /// <summary>
        /// Converts the node to itself.
        /// </summary>
        /// <returns>A reference to itself.</returns>
        public override TagNodeList ToTagList () 
        {
            return this;
        }

        /// <summary>
        /// Gets the tag type of the node.
        /// </summary>
        /// <returns>The TAG_STRING tag type.</returns>
        public override TagType GetTagType () 
        { 
            return TagType.TAG_LIST; 
        }

        /// <summary>
        /// Gets the number of subnodes contained in the list.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Gets the tag type of the subnodes contained in the list.
        /// </summary>
        public TagType ValueType { get; private set; }

        public TagNodeList() : this(TagType.TAG_END)
        {
        }

        /// <summary>
        /// Constructs a new empty list node.
        /// </summary>
        /// <param name="type">The tag type of the list's subnodes.</param>
        public TagNodeList (TagType type)
        {
            ValueType = type;
            _items = new List<TagNode>();
        }

        /// <summary>
        /// Makes a deep copy of the node.
        /// </summary>
        /// <returns>A new list node containing new subnodes representing the same data.</returns>
        public override TagNode Copy ()
        {
            TagNodeList list = new TagNodeList(ValueType);
            foreach (TagNode item in _items) {
                list.Add(item.Copy());
            }
            return list;
        }

        /// <summary>
        /// Finds the first subnode that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{TagNode}"/> delegate that defines the conditions of the subnode to search for.</param>
        /// <returns>The first subnode matching the predicate.</returns>
        public TagNode Find (Predicate<TagNode> match)
        {
            return _items.Find(match);
        }

        /// <summary>
        /// Retrieves all the subnodes that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{TagNode}"/> delegate that defines the conditions of the subnode to search for.</param>
        /// <returns>A list of all subnodes matching the predicate.</returns>
        public List<TagNode> FindAll (Predicate<TagNode> match)
        {
            return _items.FindAll(match);
        }

        /// <summary>
        /// Removes all subnodes that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{TagNode}"/> delegate that defines the conditions of the subnode to search for.</param>
        /// <returns>The number of subnodes removed from the node.</returns>
        public int RemoveAll (Predicate<TagNode> match)
        {
            return _items.RemoveAll(match);
        }

        /// <summary>
        /// Reverses the order of all the subnodes in the list.
        /// </summary>
        public void Reverse ()
        {
            _items.Reverse();
        }

        /// <summary>
        /// Reverse the order of subnodes in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the subnodes to reverse</param>
        /// <param name="count">The number of subnodes in the range to reverse.</param>
        /// <exception cref="ArgumentOutOfRangeException"><para><paramref name="index"/> is less than 0.</para><para>-or-</para>
        ///   <para><paramref name="count"/> is less than 0.</para></exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the list.</exception>
        public void Reverse (int index, int count)
        {
            _items.Reverse(index, count);
        }

        /// <summary>
        /// Sorts all the subnodes in the list using the default comparator
        /// </summary>
        public void Sort ()
        {
            _items.Sort();
        }

        /// <summary>
        /// Gets a string representation of the node's data.
        /// </summary>
        /// <returns>String representation of the node's data.</returns>
        public override string ToString ()
        {
            return _items.ToString();
        }

        /// <summary>
        /// Resets and changes the storage type of the list.
        /// </summary>
        /// <param name="type">The new tag type to store in the list.</param>
        public void ChangeValueType (TagType type)
        {
            if (type == ValueType)
                return;

            _items.Clear();
            ValueType = type;
        }

        internal override void SerializeValue(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Count).EnsureBigEndian();
            stream.WriteByte((byte) ValueType);
            stream.Write(lenBytes, 0, 4);

            foreach (var item in _items)
            {
                item.SerializeValue(stream);
            }
        }

        internal override async Task SerializeValueAsync(Stream stream)
        {
            var lenBytes = BitConverter.GetBytes(Count).EnsureBigEndian();
            
            await stream.WriteAsync(new[]{(byte) ValueType},0,1).ConfigureAwait(false);
            await stream.WriteAsync(lenBytes, 0, 4).ConfigureAwait(false);

            foreach (var item in _items)
            {
                await item.SerializeValueAsync(stream).ConfigureAwait(false);
            }
        }

        protected internal override void Deserialize(Stream stream)
        {
            var gzByte = stream.ReadByte();
            if (gzByte == -1)
            {
                throw new NbtException(NbtException.MSG_GZIP_ENDOFSTREAM);
            }

            ValueType = (TagType) gzByte;
            if (ValueType > (TagType) Enum.GetValues(typeof(TagType)).GetUpperBound(0))
            {
                throw new NbtException(NbtException.MSG_READ_TYPE);
            }

            var lenBytes = new byte[4];
            stream.Read(lenBytes, 0, 4);
            var length = BitConverter.ToInt32(lenBytes.EnsureBigEndian(), 0);
            if (length < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            for (var i = 0; i < length; i++)
            {
                var child = TagNodeFactory.Instance.Create(ValueType);
                child.Deserialize(stream);
                Add(child);
            }
        }

        public override async Task DeserializeAsync(Stream stream)
        {
            var buffer = new byte[1];
            var read = await stream.ReadAsync(buffer, 0, 1).ConfigureAwait(false);
            if (read != 1)
            {
                throw new NbtException(NbtException.MSG_GZIP_ENDOFSTREAM);
            }

            ValueType = (TagType) buffer[0];
            if (ValueType > (TagType) Enum.GetValues(typeof(TagType)).GetUpperBound(0))
            {
                throw new NbtException(NbtException.MSG_READ_TYPE);
            }

            var lenBytes = new byte[4];
            await stream.ReadAsync(lenBytes, 0, 4).ConfigureAwait(false);
            var length = BitConverter.ToInt32(lenBytes.EnsureBigEndian(), 0);
            if (length < 0)
            {
                throw new NbtException(NbtException.MSG_READ_NEG);
            }

            for (var i = 0; i < length; i++)
            {
                var child = TagNodeFactory.Instance.Create(ValueType);
                await child.DeserializeAsync(stream).ConfigureAwait(false);
                Add(child);
            }
        }

        #region IList<NBT_Value> Members

        /// <summary>
        /// Searches for the specified subnode and returns the zero-based index of the first occurrence within the entire node's list.
        /// </summary>
        /// <param name="item">The subnode to locate.</param>
        /// <returns>The zero-based index of the subnode within the node's list if found, or -1 otherwise.</returns>
        public int IndexOf (TagNode item)
        {
            return _items.IndexOf(item);
        }

        /// <summary>
        /// Inserts a subnode into the node's list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the subnode should be inserted.</param>
        /// <param name="item">The subnode to insert.</param>
        /// <exception cref="ArgumentException">Thrown when a subnode being inserted has the wrong tag type.</exception>
        public void Insert (int index, TagNode item)
        {
            if (item.GetTagType() != ValueType) {
                throw new ArgumentException("The tag type of item is invalid for this node");
            }
            _items.Insert(index, item);
        }

        /// <summary>
        /// Removes the subnode from the node's list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index to remove a subnode at.</param>
        public void RemoveAt (int index)
        {
            _items.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the subnode in the node's list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index to get or set from.</param>
        /// <returns>The subnode at the specified index.</returns>
        /// <exception cref="ArgumentException">Thrown when a subnode being assigned has the wrong tag type.</exception>
        public TagNode this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                if (value.GetTagType() != ValueType) {
                    throw new ArgumentException("The tag type of the assigned subnode is invalid for this node");
                }
                _items[index] = value;
            }
        }

        #endregion

        #region ICollection<NBT_Value> Members

        /// <summary>
        /// Adds a subnode to the end of the node's list.
        /// </summary>
        /// <param name="item">The subnode to add.</param>
        /// <exception cref="ArgumentException">Thrown when a subnode being added has the wrong tag type.</exception>
        public void Add (TagNode item)
        {
            if (item.GetTagType() != ValueType) {
                throw new ArgumentException("The tag type of item is invalid for this node");
            }

            _items.Add(item);
        }

        /// <summary>
        /// Removes all subnode's from the node's list.
        /// </summary>
        public void Clear ()
        {
            _items.Clear();
        }

        /// <summary>
        /// Checks if a subnode is contained within the node's list.
        /// </summary>
        /// <param name="item">The subnode to check for existance.</param>
        /// <returns>Status indicating if the subnode exists in the node's list.</returns>
        public bool Contains (TagNode item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the entire node's list to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the subnodes copied. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo (TagNode[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a value indicating whether the node is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurance of a subnode from the node's list.
        /// </summary>
        /// <param name="item">The subnode to remove.</param>
        /// <returns>Status indicating whether a subnode was removed.</returns>
        public bool Remove (TagNode item)
        {
            return _items.Remove(item);
        }

        #endregion

        #region IEnumerable<NBT_Value> Members

        /// <summary>
        /// Returns an enumerator that iterates through all of the subnodes in the node's list.
        /// </summary>
        /// <returns>An enumerator for this node.</returns>
        public IEnumerator<TagNode> GetEnumerator ()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through all of the subnodes in the node's list.
        /// </summary>
        /// <returns>An enumerator for this node.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }
}