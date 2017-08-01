using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Core;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Neo.UnitTests
{
    [TestClass]
    public class UT_SpentCointState
    {
        SpentCoinState uut;

        [TestInitialize]
        public void TestSetup()
        {
            uut = new SpentCoinState();
        }

        [TestMethod]
        public void TransactionHash_Get()
        {
            uut.TransactionHash.Should().BeNull();
        }

        [TestMethod]
        public void TransactionHash_Set()
        {
            UInt256 val = new UInt256();
            uut.TransactionHash = val;
            uut.TransactionHash.Should().Be(val);
        }
        [TestMethod]
        public void TransactionHeight_Get()
        {
            uut.TransactionHeight.Should().Be(0u);
        }

        [TestMethod]
        public void TransactionHeight_Set()
        {
            uint val = 4294967295;
            uut.TransactionHeight = val;
            uut.TransactionHeight.Should().Be(val);
        }
        [TestMethod]
        public void Items_Get()
        {
            uut.Items.Should().BeNull();
        }

        [TestMethod]
        public void Items_Set()
        {
            ushort key = new ushort();
            uint val = new uint();
            Dictionary<ushort, uint> dict = new Dictionary<ushort, uint>();
            dict.Add(key, val);
            uut.Items = dict;
            uut.Items[key].Should().Be(val);
        }
        private void setupSpentCoinStateWithValues(SpentCoinState spentCoinState, out UInt256 transactionHash, out uint transactionHeight, out ushort key, out uint dictVal)
        {
            transactionHash = new UInt256(TestUtils.GetByteArray(32, 0x20));
            spentCoinState.TransactionHash = transactionHash;
            transactionHeight = 69u;
            spentCoinState.TransactionHeight = transactionHeight;
            key = new ushort();
            dictVal = new uint();
            Dictionary<ushort, uint> dict = new Dictionary<ushort, uint>();
            dict.Add(key, dictVal);
            spentCoinState.Items = dict;
        }
        [TestMethod]
        public void DeserializeSCS()
        {
            byte[] dataArray = new byte[] { 0, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 42, 3, 44, 45, 48, 42, 0, 0, 0, 0, 0, 0, 0, 43, 0, 0, 0, 0, 0, 0, 0, 66, 0, 44, 0, 0, 0, 0, 0, 0, 0, 33, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 };
            Stream stream = new MemoryStream(dataArray);
            try
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    uut.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void SerializeSCS()
        {
            UInt256 transactionHash;
            uint transactionHeight;
            ushort key;
            uint dictVal;
            setupSpentCoinStateWithValues(uut, out transactionHash, out transactionHeight, out key, out dictVal);

            byte[] dataArray;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
                {
                    uut.Serialize(writer);
                    dataArray = stream.ToArray();
                }
            }

            byte[] requiredData = new byte[] { 0, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 42, 3, 44, 45, 48, 42, 0, 0, 0, 0, 0, 0, 0, 43, 0, 0, 0, 0, 0, 0, 0, 66, 0, 44, 0, 0, 0, 0, 0, 0, 0, 33, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 };
            dataArray.Length.Should().Be(44);
            try
            {
                for (int i = 0; i < 44; i++)
                {
                    dataArray[i].Should().Be(requiredData[i]);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }


}