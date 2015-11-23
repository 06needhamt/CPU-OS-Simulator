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
    public class StackItemTests
    {
        [TestMethod()]
        public void StackItemTest()
        {
            StackItem si = new StackItem(10);
            Assert.IsTrue(si.GetType().Equals(typeof(StackItem)) && si.Value == 10);
        }
    }
}