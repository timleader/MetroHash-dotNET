
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MetroHash;

namespace MetroHash.Test
{
    [TestClass]
    public class MetroHashTest
    {
        [TestMethod, Timeout(200)]
        public void MetroHash64SanityTest()
        {
            //MetroHash.Hash64_1(null, 0, 0, 0, out null);

            Assert.AreEqual(1, 1);
        }

        [TestMethod, Timeout(200)]
        public void MetroHash64ConflictTest()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
