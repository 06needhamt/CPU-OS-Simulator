using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class represents an output made through the console
    /// </summary>
    public class ConsoleOutput
    {
        private string value = String.Empty;
        private string source = String.Empty;
        private bool isCommandOutput = false;
        private ConsoleCommand command = null;

        /// <summary>
        /// Constructor for a console output that is a string 
        /// </summary>
        /// <param name="value"> the value to be outputted </param>
        /// <param name="source"> the source of the output</param>
        public ConsoleOutput(string value, string source)
        {
            this.value = value;
            this.source = source;
            this.isCommandOutput = false;
        }
        /// <summary>
        /// Constructor for a console output resulting from a command
        /// </summary>
        /// <param name="command"> the command that produced the output</param>
        /// <param name="source"> the source of the output</param>
        public ConsoleOutput(ConsoleCommand command, string source)
        {
            this.command = command;
            this.source = source;
            this.isCommandOutput = true;
        }
        /// <summary>
        /// The string representation of the output
        /// </summary>
        public string Value
        {
            [DebuggerStepThrough] get { return value; }
            [DebuggerStepThrough] set { this.value = value; }
        }
        /// <summary>
        /// The source of the output
        /// </summary>
        public string Source
        {
            [DebuggerStepThrough] get { return source; }
            [DebuggerStepThrough] set { source = value; }
        }
        /// <summary>
        /// whether this output was created by a command
        /// </summary>
        public bool IsCommandOutput
        {
            [DebuggerStepThrough] get { return isCommandOutput; }
            [DebuggerStepThrough] set { isCommandOutput = value; }
        }
        /// <summary>
        /// The command that created this output
        /// </summary>
        public ConsoleCommand Command
        {
            [DebuggerStepThrough] get { return command; }
            [DebuggerStepThrough] set { command = value; }
        }



    }
}
