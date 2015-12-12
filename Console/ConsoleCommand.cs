using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class represents a console command
    /// </summary>
    public class ConsoleCommand
    {
        private string name = String.Empty;
        private string[] parameters;
        private EnumConsoleCommands ECommand = EnumConsoleCommands.UNKNOWN;
        private Func<string> execute;
         
        /// <summary>
        /// constructor for console command that takes no parameters
        /// </summary>
        /// <param name="name"> the name of the command</param>
        public ConsoleCommand(string name)
        {
            this.name = name;
            this.parameters = new string[] {};
            BindFunction();
        }
        /// <summary>
        /// Constructor for console command that takes parameters
        /// </summary>
        /// <param name="name"> the name of the command</param>
        /// <param name="parameters"> the parameters of the command</param>
        public ConsoleCommand(string name, string[] parameters)
        {
            this.name = name;
            this.parameters = parameters;
            BindFunction();
        }

        public Func<string> Execute
        {
            get { return execute; }
            set { execute = value; }
        }

        private void BindFunction()
        {
            EnumConsoleCommands temp = (EnumConsoleCommands) Enum.Parse(typeof (EnumConsoleCommands), name.ToUpper());
            if (temp == null)
            {
                temp = EnumConsoleCommands.UNKNOWN;
            }
            switch (temp)
            {
                case EnumConsoleCommands.HELP:
                {
                    this.execute = () => HelpCommand(name, parameters);
                    break;
                }
                case EnumConsoleCommands.PROGRAM:
                {
                    this.execute = () => ProgramCommand(name, parameters);
                    break;
                }
                case EnumConsoleCommands.SIZE:
                {
                    this.execute = () => SizeCommand(name, parameters);
                        break;
                }
                case EnumConsoleCommands.UNKNOWN:
                {
                    this.execute = () => UnknownCommand(name, parameters);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private string UnknownCommand(string name, string[] parameters)
        {
            string unknownString = "Unknown command type \"//help\" for help on using the console. \n";
            return unknownString;
        }

        private string SizeCommand(string name, string[] parameters)
        {
            if (parameters.Length != 1 && (!parameters[0].Equals("pages") || !parameters[0].Equals("bytes")))
            {
                return "Invalid Command syntax use like this: \n"
                       + "//size <bytes|pages> \n";
            }
            dynamic wind = GetMainWindowInstance();
            List<SimulatorProgram> programList = wind.ProgramList;
            SimulatorProgram prog = programList.FirstOrDefault(x => x.Name.Equals(wind.currentProgram));

            if (parameters[0].Equals("bytes"))
            {
                
                if (prog == null)
                {
                    return "ERROR : No program is loaded \n";
                }
                else
                {
                    //TODO will need changing when instructions are not the same size.
                    return  "Program size = " + Convert.ToString(prog.Instructions.Count*4) + " bytes \n";
                }
            }
            else if (parameters[0].Equals("pages"))
            {
                return "Program size = " + Convert.ToString(prog.Pages) + " pages \n";
            }
            return "Invalid Command syntax use like this: \n"
                      + "//size <bytes|pages> \n ";
        }

        private string ProgramCommand(string name, string[] parameters)
        {
            dynamic wind = GetMainWindowInstance();
            List<SimulatorProgram> programList = wind.ProgramList;
            SimulatorProgram prog = programList.FirstOrDefault(x => x.Name.Equals(wind.currentProgram));
            if (prog == null)
            {
                return "ERROR : No program is loaded \n";
            }
            return "Program name = " + prog.Name + "\n";
        }

        private string HelpCommand(string name, string[] parameters)
        {
            string helpString = "CPU-OS Simulator Console Help: \n"
                                + "Commands: \n "
                                + "help : Display console help message \n"
                                + "program: Display the name of the currently loaded program \n"
                                + "size: Display the size in bytes of the currently loaded program \n";
            return helpString;
        }

        /// <summary>
        /// parses this command into a string
        /// </summary>
        /// <returns> the string representation of this command</returns>
        public string ParseCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            sb.Append(" ");
            foreach (string par in parameters)
            {
                sb.Append(par);
                sb.Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return ParseCommand();
        }

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }
    }
}
