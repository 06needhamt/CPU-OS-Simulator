using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPU_OS_Simulator.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Console.Tests
{
    [TestClass()]
    public class ConsoleInputTests
    {
        [TestMethod()]
        public void ConsoleInputTest()
        {
            ConsoleInput input = new ConsoleInput("blah");
            Assert.IsTrue(input is ConsoleInput && input.Value.Equals("blah"));
        }

        [TestMethod()]
        public void ConsoleInputTest1()
        {
            ConsoleCommand com = new ConsoleCommand("help");
            ConsoleInput input = new ConsoleInput(com);
            Assert.IsTrue(input is ConsoleInput && input.IsCommand);
        }
    }
}