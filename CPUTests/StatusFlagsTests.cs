using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass]
    public class StatusFlagsTests
    {
        [TestMethod]
        public void ToggleFlagTest()
        {
            StatusFlags N = StatusFlags.N;
            N.IsSet = false;
            N.ToggleFlag();
            Assert.IsTrue(N.IsSet);
        }
    }
}