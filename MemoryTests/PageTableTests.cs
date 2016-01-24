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
    public class PageTableTests
    {
        [TestMethod()]
        public void PageTableTest()
        {
            PageTable t = new PageTable(0);
            Assert.IsInstanceOfType(t,typeof(PageTable));
        }

        [TestMethod()]
        public void PageTableTest1()
        {
            List<PageTableEntry> entries = new List<PageTableEntry>();
            entries.Add(new PageTableEntry(0,0,0,false,new MemoryPage(0,0)));
            PageTable t = new PageTable(0,entries);
            Assert.IsTrue(t is PageTable && t.Entries.Count == 1);
        }
    }
}