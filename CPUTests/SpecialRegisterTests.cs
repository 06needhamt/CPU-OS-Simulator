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
    public class SpecialRegisterTests
    {
        [TestMethod()]
        public void SpecialRegisterTest()
        {
            SpecialRegister sp = new SpecialRegister();
            Assert.IsInstanceOfType(sp, typeof(SpecialRegister));
        }

        [TestMethod()]
        public void FindSpecialRegisterTest()
        {
            SpecialRegister sp;
            sp = SpecialRegister.FindSpecialRegister("PC");
            Assert.AreEqual(sp, SpecialRegister.PC);
        }

        [TestMethod()]
        public void setRegisterValueTest()
        {
            SpecialRegister sp = SpecialRegister.FindSpecialRegister("PC");
            sp.setRegisterValue(100, EnumOperandType.ADDRESS);
            Assert.AreEqual(sp.Value, 100);
        }

        [TestMethod()]
        public void setRegisterValueTest1()
        {
            SpecialRegister sp = SpecialRegister.FindSpecialRegister("IR");
            sp.setRegisterValue("MOV R00,10",EnumOperandType.VALUE);
            Assert.IsTrue(sp.ValueString.Equals("MOV R00,10"));
        }
    }
}