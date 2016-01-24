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
    public class ConsoleOutputTests
    {
        [TestMethod()]
        public void ConsoleOutputTest()
        {
            ConsoleOutput output = new ConsoleOutput("blah","blah");
            Assert.IsTrue(output is ConsoleOutput && output.Value.Equals("blah") && output.Source.Equals("blah"));
        }

        [TestMethod()]
        public void ConsoleOutputTest1()
        {
            ConsoleCommand com = new ConsoleCommand("help");
            ConsoleOutput output = new ConsoleOutput(com,"blah");
            Assert.IsTrue(output is ConsoleOutput && output.Command != null && output.Source.Equals("blah"));
        }
    }
}