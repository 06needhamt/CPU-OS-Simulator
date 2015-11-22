using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass()]
    public class InstructionTests
    {
        [TestMethod()]
        public void InstructionTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void InstructionTest1()
        {
            Instruction ins = new Instruction(3, 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod()]
        public void InstructionTest2()
        {
            Instruction ins = new Instruction(3, new Operand(10, EnumOperandType.VALUE), 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod()]
        public void InstructionTest3()
        {
            Instruction ins = new Instruction(3, new Operand(10, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod()]
        public void BindDelegateTest()
        {
            Instruction ins = new Instruction(0, new Operand(10, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            ins.BindDelegate();
            Assert.IsNotNull(ins.Execute);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Instruction ins = new Instruction(0, new Operand(10, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            Assert.AreEqual("MOV 10,10", ins.ToString());
        }
    }
}
