using System;
using System.Collections.Generic;
using System.Text;
using Substrate;
using System.IO;
using Substrate.Nbt;
using Substrate.Core;
using Xunit;

namespace Substrate.Tests
{
    public class LevelTests
    {
        NbtTree LoadLevelTree(string path)
        {
            NBTFile nf = new NBTFile(path);
            NbtTree tree = null;

            using (Stream nbtstr = nf.GetDataInputStream())
            {
                if (nbtstr == null)
                {
                    return null;
                }

                tree = new NbtTree(nbtstr);
            }

            return tree;
        }

        [Fact]
        public void LoadTreeTest_1_6_4_survival()
        {
            NbtTree levelTree = LoadLevelTree(@"..\..\..\Data\1_6_4-survival\level.dat");

            Level level = new Level(null);
            level = level.LoadTreeSafe(levelTree.Root);
            Assert.NotNull(level);
        }

        [Fact]
        public void LoadTreeTest_1_7_2_survival()
        {
            NbtTree levelTree = LoadLevelTree(@"..\..\..\Data\1_7_2-survival\level.dat");

            Level level = new Level(null);
            level = level.LoadTreeSafe(levelTree.Root);
            Assert.NotNull(level);
        }

        [Fact]
        public void LoadTreeTest_1_7_10_survival()
        {
            NbtTree levelTree = LoadLevelTree(@"..\..\..\Data\1_7_10-survival\level.dat");

            Level level = new Level(null);
            level = level.LoadTreeSafe(levelTree.Root);
            Assert.NotNull(level);
        }

        [Fact]
        public void LoadTreeTest_1_8_3_survival()
        {
            NbtTree levelTree = LoadLevelTree(@"..\..\..\Data\1_8_3-survival\level.dat");

            Level level = new Level(null);
            level = level.LoadTreeSafe(levelTree.Root);
            Assert.NotNull(level);
        }
    }
}
