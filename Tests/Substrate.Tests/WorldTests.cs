using System;
using System.Collections.Generic;
using System.Text;
using Substrate;
using Xunit;

namespace Substrate.Tests
{
    public class WorldTests
    {
        [Fact]
        public void OpenTest_1_6_4_survival()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_6_4-survival\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_7_2_survival()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_7_2-survival\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_7_10_survival()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_7_10-survival\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_8_3_survival()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_8_3-survival\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_8_3_debug()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_8_3-debug\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_8_7_debug()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_8_7-debug\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_8_7_survival()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_8_7-survival\");
            Assert.NotNull(world);
        }

        [Fact]
        public void OpenTest_1_9_2_debug()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_9_2-debug\");
            Assert.NotNull(world);
        }
    }
}
