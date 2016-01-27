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
    public class PageTableEntryTests
    {
        [TestMethod()]
        public void PageTableEntryTest()
        {
            PageTableEntry entry = new PageTableEntry(0,0,0,false,new MemoryPage(0,0,"Unit Test"));
            Assert.IsInstanceOfType(entry,typeof(PageTableEntry));
        }
    }
}