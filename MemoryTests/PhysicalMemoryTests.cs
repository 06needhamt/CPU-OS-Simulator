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
    public class PhysicalMemoryTests
    {
        [TestMethod()]
        public void PhysicalMemoryTest()
        {
            PhysicalMemory m = new PhysicalMemory(10);
            Assert.IsInstanceOfType(m,typeof(PhysicalMemory));
        }

        [TestMethod()]
        public void isFullTest()
        {
            PhysicalMemory m = new PhysicalMemory(10);
            for (int i = 0; i < m.Capacity; i++)
            {
                m.AddPage(new MemoryPage(0, 0), i);
            }
            Assert.IsTrue(m.isFull());
        }

        [TestMethod()]
        public void AddPageTest()
        {
            PhysicalMemory mem = new PhysicalMemory(10);
            MemoryPage m = new MemoryPage(0,0);
            mem.AddPage(m, 0);
            Assert.IsTrue(mem.Pages.Contains(m));
        }

        [TestMethod()]
        public void SwapOutTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SwapInTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RequestMemoryPageTest()
        {
            PhysicalMemory mem = new PhysicalMemory(10);
            MemoryPage p1 = new MemoryPage(0,0);
            MemoryPage p2 = new MemoryPage(1,1 * MemoryPage.PAGE_SIZE);
            mem.AddPage(p1, 0);
            mem.AddPage(p2, 1);
            MemoryPage req = mem.RequestMemoryPage(1);
            Assert.AreEqual(p2, req);
        }
    }
}