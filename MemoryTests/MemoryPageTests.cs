using CPU_OS_Simulator.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryTests
{
    [TestClass()]
    public class MemoryPageTests
    {
        [TestMethod()]
        public void MemoryPageTest()
        {
            MemoryPage page = new MemoryPage(0,0);
            Assert.IsInstanceOfType(page,typeof(MemoryPage));
        }
    }
}