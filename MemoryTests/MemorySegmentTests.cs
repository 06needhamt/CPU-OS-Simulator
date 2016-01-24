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
            MemorySegment s = new MemorySegment();
            Assert.IsInstanceOfType(s,typeof(MemorySegment));
        }

        [TestMethod()]
        public void MemorySegmentTest1()
        {
            int physicalAddress = 0xFFFF;
            MemorySegment s = new MemorySegment(physicalAddress);
            Assert.IsTrue(s is MemorySegment && s.PhysicalAddress == physicalAddress);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            MemorySegment s = new MemorySegment();
            string str = s.ToString();
            Assert.IsNotNull(str);
        }

        [TestMethod()]
        public void BuildDataStringTest()
        {
            MemorySegment s = new MemorySegment();
            string str = s.BuildDataString();
            Assert.IsNotNull(str);
        }

        [TestMethod()]
        public void GetByteTest()
        {
            MemorySegment s = new MemorySegment();
            for (int i = 0; i < 8; i++)
            {
                s.SetByte(i,(byte) i);
            }
            for (int i = 0; i < 8; i++)
            {
                if (s.GetByte(i) != i)
                {
                    Assert.Fail("Bytes were not equal");
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void SetByteTest()
        {
            MemorySegment s = new MemorySegment();
            for (int i = 0; i < 8; i++)
            {
                s.SetByte(i, (byte)i);
            }
            for (int i = 0; i < 8; i++)
            {
                if (s.GetByte(i) != i)
                {
                    Assert.Fail("Bytes were not equal");
                }
            }
            Assert.IsTrue(true);
        }
    }
}