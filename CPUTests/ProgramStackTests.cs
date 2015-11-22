﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass()]
    public class ProgramStackTests
    {
        [TestMethod()]
        public void ProgramStackTest()
        {
            ProgramStack stack = new ProgramStack();
            Assert.IsInstanceOfType(stack, typeof(ProgramStack));
        }

        [TestMethod()]
        public void pushItemTest()
        {
            ProgramStack stack = new ProgramStack();
            stack.pushItem(new StackItem(10));
            Assert.AreEqual(stack.popItem(), 10);
        }

        [TestMethod()]
        public void popItemTest()
        {
            Assert.Fail();
        }
    }
}
