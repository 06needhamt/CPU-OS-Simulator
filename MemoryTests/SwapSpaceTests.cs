﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory.Tests
{
    [TestClass()]
    public class SwapSpaceTests
    {
        [TestMethod()]
        public void SwapSpaceTest()
        {
            SwapSpace swap = new SwapSpace();
            Assert.IsInstanceOfType(swap,typeof(SwapSpace));
        }
    }
}