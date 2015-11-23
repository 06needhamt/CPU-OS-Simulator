using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CPU_OS_Simulator.Tests
{
    [TestClass()]
    public class InstructionsWindowTests
    {
        [TestMethod()]
        public void InstructionsWindowTest()
        {
            InstructionsWindow window = new InstructionsWindow();
            Assert.IsInstanceOfType(window, typeof(InstructionsWindow));
        }

        [TestMethod()]
        public void InstructionsWindowTest1()
        {
            MainWindow mainwindow = new MainWindow();
            InstructionsWindow window = new InstructionsWindow(mainwindow);
            Assert.IsInstanceOfType(window, typeof(InstructionsWindow));
        }

        [TestMethod()]
        public void InstructionsWindowTest2()
        {
            MainWindow mainwindow = new MainWindow();
            InstructionsWindow window = new InstructionsWindow(mainwindow,EnumInstructionMode.ADD_NEW);
            Assert.IsInstanceOfType(window, typeof(InstructionsWindow));
        }
    }
}
