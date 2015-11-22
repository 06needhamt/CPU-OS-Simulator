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
    public class OperandTests
    {
        [TestMethod()]
        public void OperandTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void OperandTest1()
        {
            Operand op = new Operand(10, EnumOperandType.VALUE);
            Assert.IsInstanceOfType(op, typeof(Operand));
        }

        [TestMethod()]
        public void OperandTest2()
        {
            Operand op = new Operand(Register.R00, EnumOperandType.VALUE);
            Assert.IsInstanceOfType(op, typeof(Operand));
        }
    }
}
