﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass()]
    public class SimulatorProgramTests
    {
        [TestMethod()]
        public void SimulatorProgramTest()
        {
            SimulatorProgram prog = new SimulatorProgram();
            Assert.IsInstanceOfType(prog, typeof(SimulatorProgram));
        }

        [TestMethod()]
        public void SimulatorProgramTest1()
        {
            SimulatorProgram prog = new SimulatorProgram("Test", 0, 1);
            Assert.IsInstanceOfType(prog, typeof(SimulatorProgram));
        }

        [TestMethod()]
        public void AddInstructionTest()
        {
            SimulatorProgram prog = new SimulatorProgram("Test", 0, 1);
            Instruction ins = new Instruction(0, new Operand(Register.R00, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE),4);
            prog.AddInstruction(ref ins);
            Assert.AreEqual(prog.Instructions.Count, 1);
        }

        [TestMethod()]
        public void AddInstructionTest1()
        {
            SimulatorProgram prog = new SimulatorProgram("Test", 0, 1);
            Instruction ins = new Instruction(0, new Operand(Register.R00, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            Instruction ins2 = new Instruction(0, new Operand(Register.R01, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            prog.AddInstruction(ref ins);
            prog.AddInstruction(ref ins2, 0);
            Assert.IsTrue(prog.Instructions.Count == 2 && prog.Instructions.ElementAt(0).Equals(ins2));
        }

        [TestMethod()]
        public void UpdateAddressesTest()
        {
            Assert.Inconclusive();
        }
    }
}