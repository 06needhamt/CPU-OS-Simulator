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
    public class ConsoleCommandTests
    {
        [TestMethod()]
        public void ConsoleCommandTest()
        {
            ConsoleCommand com = new ConsoleCommand("help");
            Assert.IsTrue(com is ConsoleCommand && com.Name.Equals("help"));
        }

        [TestMethod()]
        public void ConsoleCommandTest1()
        {
            string[] pars = new[] {"pages"};
            ConsoleCommand com = new ConsoleCommand("size", pars);
            Assert.IsTrue(com is ConsoleCommand && com.Name.Equals("size") && com.Parameters.Equals(pars));
        }

        [TestMethod()]
        public void ParseCommandTest()
        {
            string name = "size";
            string[] pars = new[] {"pages"};
            ConsoleCommand com = new ConsoleCommand(name,pars);
            Assert.IsTrue(com.ParseCommand().Equals("size pages "));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            string name = "size";
            string[] pars = new[] { "pages" };
            ConsoleCommand com = new ConsoleCommand(name, pars);
            Assert.IsTrue(com.ToString().Equals("size pages "));
        }
    }
}