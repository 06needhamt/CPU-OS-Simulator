﻿using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPUTests
{
    [TestClass]
    public class StackItemTests
    {
        [TestMethod]
        public void StackItemTest()
        {
            StackItem si = new StackItem(10);
            Assert.IsTrue(si.GetType().Equals(typeof(StackItem)) && si.Value == 10);
        }
    }
}