using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPUTests
{
    [TestClass]
    public class ProgramStackTests
    {
        [TestMethod]
        public void ProgramStackTest()
        {
            ProgramStack stack = new ProgramStack();
            Assert.IsInstanceOfType(stack, typeof(ProgramStack));
        }

        [TestMethod]
        public void pushItemTest()
        {
            ProgramStack stack = new ProgramStack();
            stack.pushItem(new StackItem(10));
            Assert.AreEqual(stack.StackItems.Count, 1);
        }

        [TestMethod]
        public void popItemTest()
        {
            ProgramStack stack = new ProgramStack();
            stack.pushItem(new StackItem(10));
            Assert.AreEqual(stack.popItem(), 10);
        }
    }
}