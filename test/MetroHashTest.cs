
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
        private static readonly char[] TEST_KEY_63 = 
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '1', '2'
        };
        
        //---------------------------------------------------------------------------//
        private static readonly char[] TEST_HASH64_1 = 
        {
            '6', '5', '8', 'F', '0', '4', '4', 'F', '5', 'C', '7', '3', '0', 'E', '4', '0'
        };

        //---------------------------------------------------------------------------//
        private static readonly char[] TEST_HASH64_2 = 
        {
            '0', '7', '3', 'C', 'A', 'A', 'B', '9', '6', '0', '6', '2', '3', '2', '1', '1'
        };

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_1_SanityTest()
        {
            byte[] lHash = null;

            byte[] lInput = new byte[TEST_KEY_63.Length];
            Buffer.BlockCopy(TEST_KEY_63, 0, lInput, 0, TEST_KEY_63.Length);

            MetroHash.Hash64_1(lInput, 0, (uint)lInput.Length, 0, out lHash);

            Assert.AreEqual(1, TEST_HASH64_1);
        }

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_2_SanityTest()
        {
            byte[] lHash = null;

            byte[] lInput = new byte[TEST_KEY_63.Length];
            Buffer.BlockCopy(TEST_KEY_63, 0, lInput, 0, TEST_KEY_63.Length);

            MetroHash.Hash64_2(lInput, 0, (uint)lInput.Length, 0, out lHash);

            Assert.AreEqual(1, TEST_HASH64_2);
        }

        //---------------------------------------------------------------------------//
        [TestMethod]
        public void Hash64_1_ConflictTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
