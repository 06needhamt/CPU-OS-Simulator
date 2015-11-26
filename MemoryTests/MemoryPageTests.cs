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
            MemoryPage page = new MemoryPage(0,0,256);
            Assert.IsInstanceOfType(page,typeof(MemoryPage));
        }
    }
}