using System;

namespace Substrate.Nbt
{
    public class TagNodeFactory
    {
        public static TagNodeFactory Instance { get; } = new TagNodeFactory();
        
        public TagNode Create(TagType type)
        {
            switch (type)
            {
                case TagType.TAG_END:
                    return null;

                case TagType.TAG_BYTE:
                    return new TagNodeByte();

                case TagType.TAG_SHORT:
                    return new TagNodeShort();

                case TagType.TAG_INT:
                    return new TagNodeInt();

                case TagType.TAG_LONG:
                    return new TagNodeLong();

                case TagType.TAG_FLOAT:
                    return new TagNodeFloat();

                case TagType.TAG_DOUBLE:
                    return new TagNodeDouble();

                case TagType.TAG_BYTE_ARRAY:
                    return new TagNodeByteArray();

                case TagType.TAG_STRING:
                    return new TagNodeString();

                case TagType.TAG_LIST:
                    return new TagNodeList();

                case TagType.TAG_COMPOUND:
                    return new TagNodeCompound();

                case TagType.TAG_INT_ARRAY:
                    return new TagNodeIntArray();

                case TagType.TAG_LONG_ARRAY:
                    return new TagNodeLongArray();

                case TagType.TAG_SHORT_ARRAY:
                    return new TagNodeShortArray();
                default:
                    throw new InvalidOperationException("Not supported tag type.");
            }
        }
    }
}