using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            parameters = new string[] {};
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
        /// <summary>
        /// The function to execute when the command is being executed
        /// </summary>
        public Func<string> Execute
        {
            get { return execute; }
            set { execute = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public string[] Parameters
        {
            get { return parameters; }
        }

        public EnumConsoleCommands ECommand1
        {
            get { return ECommand; }
        }

        /// <summary>
        /// This function binds a function pointer to a command
        /// the function pointer bound here will be executed when the command it inputted into the console
        /// </summary>
        private void BindFunction()
        {
            EnumConsoleCommands temp;
            if(!Enum.TryParse(name, true, out temp))
            {
                temp = EnumConsoleCommands.UNKNOWN;
            }
            switch (temp)
            {
                case EnumConsoleCommands.HELP:
                {
                    execute = () => HelpCommand(name, parameters);
                    break;
                }
                case EnumConsoleCommands.PROGRAM:
                {
                    execute = () => ProgramCommand(name, parameters);
                    break;
                }
                case EnumConsoleCommands.SIZE:
                {
                    execute = () => SizeCommand(name, parameters);
                        break;
                }
                case EnumConsoleCommands.CLEAR:
                {
                    execute = () => ClearCommand(name, parameters);
                    break;
                }
                case EnumConsoleCommands.UNKNOWN:
                {
                    execute = () => UnknownCommand(name, parameters);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Function that handles the Clear Command
        /// </summary>
        /// <param name="name"> the command that is being executed</param>
        /// <param name="parameters"> any parameters for that command</param>
        /// <returns> the result of the command</returns>
        private string ClearCommand(string name, string[] parameters)
        {
            return "\"Clear\"";
        }
        /// <summary>
        /// Function that handles when an unknown command is entered into the console
        /// </summary>
        /// <param name="name"> the command that is being executed</param>
        /// <param name="parameters"> any parameters for that command</param>
        /// <returns> the result of the command</returns>
        private string UnknownCommand(string name, string[] parameters)
        {
            string unknownString = "Unknown command type \"//help\" for help on using the console. \n";
            return unknownString;
        }
        /// <summary>
        /// Function that handles the size command
        /// </summary>
        /// <param name="name"> the command that is being executed</param>
        /// <param name="parameters"> any parameters for that command</param>
        /// <returns> the result of the command</returns>
        private string SizeCommand(string name, string[] parameters)
        {
            if (parameters.Length != 1 && ( parameters.Length == 0 || !parameters[0].Equals("pages") || !parameters[0].Equals("bytes")) )
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
                //TODO will need changing when instructions are not the same size.
                return  "Program size = " + Convert.ToString(prog.Instructions.Count*4) + " bytes \n";
            }
            if (parameters[0].Equals("pages"))
            {
                return "Program size = " + Convert.ToString(prog.Pages) + " pages \n";
            }
            return "Invalid Command syntax use like this: \n"
                      + "//size <bytes|pages> \n ";
        }
        /// <summary>
        /// Function that handles the program command
        /// </summary>
        /// <param name="name"> the command that is being executed</param>
        /// <param name="parameters"> any parameters for that command</param>
        /// <returns> the result of the command</returns>
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
                                + "size: Display the size in bytes of the currently loaded program \n"
                                + "clear: Clears The Console output \n";
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
            System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }
    }
}
