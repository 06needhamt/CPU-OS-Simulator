using CPU_OS_Simulator;
using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPU_OS_SimulatorTests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void MainWindowTest()
        {
            MainWindow wind = new MainWindow();
            Assert.IsInstanceOfType(wind, typeof(MainWindow));
        }

        [TestMethod]
        public void IsAdministratorTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetAssociationTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SayHelloTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CreateInstructionTest()
        {
            Instruction ins = new Instruction(3, 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod]
        public void CreateInstructionTest1()
        {
            Instruction ins = new Instruction(3, new Operand(10, EnumOperandType.VALUE), 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod]
        public void CreateInstructionTest2()
        {
            Instruction ins = new Instruction(3, new Operand(10, EnumOperandType.VALUE), new Operand(10, EnumOperandType.VALUE), 4);
            Assert.IsInstanceOfType(ins, typeof(Instruction));
        }

        [TestMethod]
        public void AddInstructionTest()
        {
            SimulatorProgram prog = new SimulatorProgram("Test", 0, 1);
            Instruction ins = new Instruction(3, 4);
            prog.Instructions.Add(ins);
            Assert.AreEqual(prog.Instructions.Count, 1);
        }

        [TestMethod]
        public void SerializeObjectTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DeSerializeObjectTest()
        {
            Assert.Inconclusive();
        }
    }
}