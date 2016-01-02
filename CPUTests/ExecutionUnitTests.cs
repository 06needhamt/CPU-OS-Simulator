using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPUTests
{
    [TestClass]
    public class ExecutionUnitTests
    {
        private SimulatorProgram prog = new SimulatorProgram("Execution UnitTest", 0, 1);

        [TestMethod]
        public void ExecutionUnitTest()
        {
            ExecutionUnit unit = new ExecutionUnit(prog, 10);
            Assert.IsInstanceOfType(unit, typeof(ExecutionUnit));
        }

        [TestMethod]
        public void ExecutionUnitTest1()
        {
            ExecutionUnit unit = new ExecutionUnit(prog, 10, 10);
            Assert.IsInstanceOfType(unit, typeof(ExecutionUnit));
            Assert.AreEqual(unit.CurrentIndex, 10);
        }

        [TestMethod]
        public void ExecuteInstructionTest()
        {
            Operand op1 = new Operand(Register.R00, EnumOperandType.VALUE);
            Operand op2 = new Operand(10, EnumOperandType.VALUE);
            Instruction ins = new Instruction((int)EnumOpcodes.ADD, op1, false, op2, false, 4);
            int result = ins.Execute();
            Assert.AreEqual(result, 10);
        }
    }
}