using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory.Tests
{
    [TestClass()]
    public class MemorySegmentTests
    {
        [TestMethod()]
        public void MemorySegmentTest()
        {
            MemorySegment segment = new MemorySegment();
            Assert.IsInstanceOfType(segment, typeof (MemorySegment));
        }

        [TestMethod()]
        public void MemorySegmentTest1()
        {
            MemorySegment segment = new MemorySegment(1000);
            Assert.IsTrue(segment.GetType().Equals(typeof (MemorySegment)) && segment.PhysicalAddress == 1000);
        }

        [TestMethod()]
        public void GetByteTest()
        {
            MemorySegment segment = new MemorySegment();
            Random random = new Random(int.MinValue);
            int number = random.Next(0, 8);
            segment.SetByte(number, 0x02);
            Assert.AreEqual(segment.GetByte(number), 0x02);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BuildDataStringTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SetByteTest()
        {
            MemorySegment segment = new MemorySegment();
            Random random = new Random(int.MinValue);
            int number = random.Next(0, 8);
            segment.SetByte(number, 0x02);
            Assert.AreEqual(segment.GetByte(number), 0x02);
        }
    }
}