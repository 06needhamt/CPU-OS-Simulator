using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Tests
{
    [TestClass()]
    public class RegisterTests
    {
        [TestMethod()]
        public void RegisterTest()
        {
            Register r = new Register();
            Assert.IsInstanceOfType(r, typeof(Register));
        }

        [TestMethod()]
        public void setRegisterValueTest()
        {
            Register reg = Register.R00;
            reg.setRegisterValue(10, EnumOperandType.VALUE);
            Assert.AreEqual(reg.Value, 10);
        }

        [TestMethod()]
        public void FindRegisterTest()
        {
            Register reg = Register.FindRegister("R00");
            Assert.IsTrue(reg.Equals(Register.R00));
        }
    }
}