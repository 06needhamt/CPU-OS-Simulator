using CPU_OS_Simulator.CPU;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPUTests
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void RegisterTest()
        {
            Register r = new Register();
            Assert.IsInstanceOfType(r, typeof(Register));
        }

        [TestMethod]
        public void setRegisterValueTest()
        {
            Register reg = Register.R00;
            reg.SetRegisterValue(10, EnumOperandType.VALUE);
            Assert.AreEqual(reg.Value, 10);
        }

        [TestMethod]
        public void FindRegisterTest()
        {
            Register reg = Register.FindRegister("R00");
            Assert.IsTrue(reg.Equals(Register.R00));
        }
    }
}