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
    public class MemoryPageTests
    {
        [TestMethod()]
        public void MemoryPageTest()
        {
            MemoryPage m = new MemoryPage(0, 0,"Unit Test",-1);
            Assert.IsInstanceOfType(m, typeof(MemoryPage));
        }

        [TestMethod()]
        public void SwapOutTest()
        {
            PhysicalMemory mem = new PhysicalMemory(10);
            PageTable table = new PageTable(0);
            MemoryPage m = new MemoryPage(0, 0,"Unit Test",-1);
            mem.AddPage(m, 0);
            m.SwapOut(0, 0);
            Assert.IsTrue(table.Entries[0].SwappedOut);
        }

        [TestMethod()]
        public void SwapInTest()
        {
            PhysicalMemory mem = new PhysicalMemory(10);
            PageTable table = new PageTable(0);
            MemoryPage m = new MemoryPage(0, 0,"Unit Test",-1);
            mem.AddPage(m, 0);
            m.SwapOut(0, 0);
            m.SwapIn(0, 0);
            Assert.IsFalse(table.Entries[0].SwappedOut);
        }

        [TestMethod()]
        public void ZeroMemoryTest()
        {
            MemoryPage m = new MemoryPage(0, 0,"Unit Test",-1);
            int count = 0;
            m.ZeroMemory();
            for (int i = 0; i < MemoryPage.PAGE_SIZE / 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (m.Data[i].GetByte(j) == 0x00)
                    {
                        count++;
                    }
                }
            }
            Assert.IsTrue(count == 256);
        }
    }
}