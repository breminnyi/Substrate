using System;
using System.Collections.Generic;
using System.Text;
using Substrate;
using System.Diagnostics;
using Xunit;

namespace Substrate.Tests
{
    public class BlockTests
    {
        static class DebugWorld
        {
            public const int Y = 70;
            public const int MinX = 1;
            public const int MaxX = 180;
            public const int MinZ = 1;
            public const int MaxZ = 180;
        }
        
        [Fact]
        public void BlockTest_1_8_3_debug()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_8_3-debug\");
            Assert.NotNull(world);

            for (int x = DebugWorld.MinX; x < DebugWorld.MaxX; x += 2)
            {
                for (int z = DebugWorld.MinZ; z < DebugWorld.MaxZ; z += 2)
                {
                    var blockRef = world.GetBlockManager().GetBlockRef(x, DebugWorld.Y, z);
                    var blockInfo = BlockInfo.BlockTable[blockRef.ID];

                    Debug.WriteLine("ID:{0} ({1}), Data:{2}", blockRef.ID, blockInfo.Name, blockRef.Data);

                    Assert.True(blockInfo.Registered, $"Block ID {blockRef.ID} has not been registered" );
                    Assert.True(blockInfo.TestData(blockRef.Data),
                        $"Data value '0x{blockRef.Data:X4}' not recognised for block '{blockInfo.Name}' at {x},{z}");
                }
            }
        }

        [Fact]
        public void BlockTest_1_9_2_debug()
        {
            NbtWorld world = NbtWorld.Open(@"..\..\..\Data\1_9_2-debug\");
            Assert.NotNull(world);

            bool dataError = false;

            for (int x = DebugWorld.MinX; x < DebugWorld.MaxX; x += 2)
            {
                for (int z = DebugWorld.MinZ; z < DebugWorld.MaxZ; z += 2)
                {
                    var blockRef = world.GetBlockManager().GetBlockRef(x, DebugWorld.Y, z);
                    var blockInfo = BlockInfo.BlockTable[blockRef.ID];

                    Debug.WriteLine($"ID:{blockRef.ID} ({blockInfo.Name}), Data:{blockRef.Data}");

                    Assert.True(blockInfo.Registered, $"Block ID {blockRef.ID} has not been registered");
                    if (!blockInfo.TestData(blockRef.Data))
                    {
                        dataError = true;
                        Debug.WriteLine("Data value '0x{0:X4}' not recognised for block '{1}' at {2},{3}", blockRef.Data, blockInfo.Name, x, z);
                    }
                }
            }
        }
    }
}
