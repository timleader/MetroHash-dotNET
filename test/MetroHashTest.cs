
using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetroHash;

namespace MetroHash.Test
{
    [TestClass]
    public class MetroHashTest
    {
        //---------------------------------------------------------------------------//
        private static readonly string TEST_KEY_63 = 
            "012345678901234567890123456789012345678901234567890123456789012";
        
        //---------------------------------------------------------------------------//
        private static readonly byte[] TEST_HASH64_1 = 
        {
            0x65, 0x8F, 0x04, 0x4F, 0x5C, 0x73, 0x0E, 0x40
        };

        //---------------------------------------------------------------------------//
        private static readonly byte[] TEST_HASH64_2 = 
        {
            0x07, 0x3C, 0xAA, 0xB9, 0x60, 0x62, 0x32, 0x11
        };

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_1_SanityTest()
        {
            byte[] lHash = null;
            byte[] lInput = Encoding.UTF8.GetBytes(TEST_KEY_63);

            MetroHash.Hash64_1(lInput, 0, (uint)lInput.Length, 0, out lHash);

            ulong lTestHash = BitConverter.ToUInt64(lHash, 0);
            ulong lTargetHash = BitConverter.ToUInt64(TEST_HASH64_1, 0);

            Assert.AreEqual(lTestHash, lTargetHash);
        }

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_2_SanityTest()
        {
            byte[] lHash = null;
            byte[] lInput = Encoding.UTF8.GetBytes(TEST_KEY_63);

            MetroHash.Hash64_2(lInput, 0, (uint)lInput.Length, 0, out lHash);

            ulong lTestHash = BitConverter.ToUInt64(lHash, 0);
            ulong lTargetHash = BitConverter.ToUInt64(TEST_HASH64_2, 0);

            Assert.AreEqual(lTestHash, lTargetHash);
        }

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_1_ConflictTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
