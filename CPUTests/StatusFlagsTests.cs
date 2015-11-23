using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass()]
    public class StatusFlagsTests
    {
        [TestMethod()]
        public void ToggleFlagTest()
        {
            StatusFlags N = StatusFlags.N;
            N.IsSet = false;
            N.ToggleFlag();
            Assert.IsTrue(N.IsSet == true);
        }
    }
}