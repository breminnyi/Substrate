﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Substrate.Nbt
{
    /// <summary>
    /// A node in an NBT schema definition, used to define what values are considered valid for a given NBT node.
    /// </summary>
    public abstract class SchemaNode
    {
        /// <summary>
        /// Gets the name of an expected NBT node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets additional schema options defined for this node.
        /// </summary>
        public SchemaOptions Options { get; }

        /// <summary>
        /// Constructs a new <see cref="SchemaNode"/> representing a <see cref="TagNode"/> named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the corresponding <see cref="TagNode"/>.</param>
        protected SchemaNode (string name)
        {
            Name = name;
        }

        /// <summary>
        /// Constructs a new <see cref="SchemaNode"/> with additional options.
        /// </summary>
        /// <param name="name">The name of the corresponding <see cref="TagNode"/>.</param>
        /// <param name="options">One or more option flags modifying the processing of this node.</param>
        protected SchemaNode (string name, SchemaOptions options)
        {
            Name = name;
            Options = options;
        }

        /// <summary>
        /// Construct a sensible default NBT tree representative of this schema node.
        /// </summary>
        /// <returns>A <see cref="TagNode"/> that is valid for this schema node.</returns>
        public virtual TagNode BuildDefaultTree ()
        {
            return null;
        }
    }
}
