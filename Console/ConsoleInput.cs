using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class represents an input made through the console
    /// </summary>
    public class ConsoleInput
    {
        private string value = String.Empty;
        private bool isCommand = false;
        private ConsoleCommand command = null;

        /// <summary>
        /// Constructor for a console input that is a string
        /// </summary>
        /// <param name="value"> the inputted string</param>
        public ConsoleInput(string value)
        {
            this.value = value;
            this.isCommand = false;
        }
        /// <summary>
        /// Constructor for a console input that is a valid console command 
        /// </summary>
        /// <param name="command"> the inputted console command</param>
        public ConsoleInput(ConsoleCommand command)
        {
            this.command = command;
            this.isCommand = true;
        }
        /// <summary>
        /// The string representation of the input
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        /// <summary>
        /// whether the input is a valid console command
        /// </summary>
        public bool IsCommand
        {
            get { return isCommand; }
            set { isCommand = value; }
        }
        /// <summary>
        /// The ConaoleCommand object that represents this command
        /// </summary>
        public ConsoleCommand Command
        {
            get { return command; }
            set { command = value; }
        }
    }
}
