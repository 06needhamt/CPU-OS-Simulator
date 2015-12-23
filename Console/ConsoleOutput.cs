using System;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class represents an output made through the console
    /// </summary>
    public class ConsoleOutput
    {
        private string value = String.Empty;
        private string source = String.Empty;
        private bool isCommandOutput;
        private ConsoleCommand command;

        /// <summary>
        /// Constructor for a console output that is a string 
        /// </summary>
        /// <param name="value"> the value to be outputted </param>
        /// <param name="source"> the source of the output</param>
        public ConsoleOutput(string value, string source)
        {
            this.value = value;
            this.source = source;
            isCommandOutput = false;
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
            isCommandOutput = true;
        }
        /// <summary>
        /// The string representation of the output
        /// </summary>
        public string Value
        {
             get { return value; }
             set { this.value = value; }
        }
        /// <summary>
        /// The source of the output
        /// </summary>
        public string Source
        {
             get { return source; }
             set { source = value; }
        }
        /// <summary>
        /// whether this output was created by a command
        /// </summary>
        public bool IsCommandOutput
        {
             get { return isCommandOutput; }
             set { isCommandOutput = value; }
        }
        /// <summary>
        /// The command that created this output
        /// </summary>
        public ConsoleCommand Command
        {
             get { return command; }
             set { command = value; }
        }



    }
}
