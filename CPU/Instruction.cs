using System;
using System.Web.Script.Serialization;
using System.Reflection;
using CPU_OS_Simulator.CPU;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class Instruction
    {
        #region Global Variables

        private Int32 opcode;
        private string category;
        [ScriptIgnore]
        private Func<int> execute;
        private Operand operand1;
        private Operand operand2;
        private Int32 address;
        private Int32 size;
        private Int32 result;
        private string instructionString;

        #endregion Global Variables

        #region Constructors

        public Instruction()
        {
        }

        public Instruction(Int32 opcode, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = null;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        public Instruction(Int32 opcode, Operand op1, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        public Instruction(Int32 opcode, Operand op1, Operand op2, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = op2;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        #endregion Constructors

        #region Properties

        public int Opcode
        {
            get
            {
                return opcode;
            }

            set
            {
                opcode = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public Operand Operand1
        {
            get
            {
                return operand1;
            }

            set
            {
                operand1 = value;
            }
        }

        public Operand Operand2
        {
            get
            {
                return operand2;
            }

            set
            {
                operand2 = value;
            }
        }
        [ScriptIgnore]
        public Func<int> Execute
        {
            get
            {
                return execute;
            }

            set
            {
                execute = value;
            }
        }

        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public int Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public string InstructionString
        {
            get
            {
                return instructionString;
            }

            set
            {
                instructionString = value;
            }
        }

        #endregion Properties

        #region Methods

        public void BindDelegate()
        {
            switch (opcode)
            {
                case 0:
                    {
                        this.execute = () => MOV(operand1, operand2);
                        break;
                    }
                case 1:
                    {
                        this.execute = () => MVS(operand1, operand2);
                        break;
                    }
                case 2:
                    {
                        this.execute = () => CVS(operand1, operand2);
                        break;
                    }
                case 3:
                    {
                        this.execute = () => CVI(operand1, operand2);
                        break;
                    }
                case 4:
                    {
                        this.execute = () => LDB(operand1, operand2);
                        break;
                    }
                case 5:
                    {
                        this.execute = () => LDW(operand1, operand2);
                        break;
                    }
                case 6:
                    {
                        this.execute = () => LNS(operand1, operand2);
                        break;
                    }
                case 7:
                    {
                        this.execute = () => LDBI(operand1, operand2);
                        break;
                    }
                case 8:
                    {
                        this.execute = () => LDWI(operand1, operand2);
                        break;
                    }
                case 9:
                    {
                        this.execute = () => TAS(operand1, operand2);
                        break;
                    }
                case 10:
                    {
                        this.execute = () => STB(operand1, operand2);
                        break;
                    }
                case 11:
                    {
                        this.execute = () => STW(operand1, operand2);
                        break;
                    }
                case 12:
                    {
                        this.execute = () => STBI(operand1, operand2);
                        break;
                    }
                case 13:
                    {
                        this.execute = () => STWI(operand1, operand2);
                        break;
                    }
                case 14:
                    {
                        this.execute = () => PUSH(operand1, operand2);
                        break;
                    }
                case 15:
                    {
                        this.execute = () => POP(operand1, operand2);
                        break;
                    }
                case 16:
                    {
                        this.execute = () => SWP(operand1, operand2);
                        break;
                    }
                case 22:
                    {
                        this.execute = () => ADD(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override string ToString()
        {
            string parsedOpcode = Enum.GetName(typeof(EnumOpcodes), opcode);
            string op1 = "";
            string op2 = "";
            if (operand1 != null)
            {
                if (operand1.IsRegister)
                {
                    op1 = Operand1.Register.Name;
                }
                else
                {
                    op1 = Operand1.Value.ToString();
                }
            }
            if (operand2 != null)
            {
                if (Operand2.IsRegister)
                {
                    op2 = Operand2.Register.Name;
                }
                else
                {
                    op2 = Operand2.Value.ToString();
                }
            }
            return parsedOpcode.ToUpper() + " " + op1.ToUpper() + "," + op2.ToUpper();
        }

        #endregion Methods

        #region Instruction Execution Functions

        //TODO Allow for memory operands
        #region Data Transfer
        private int MOV(Operand lhs, Operand rhs)
        {
            result = rhs.Value;
            if (lhs.IsRegister)
            {
                lhs.Register.Value = result;
                Register.FindRegister(lhs.Register.Name).setRegisterValue(result,EnumOperandType.VALUE);
            }
            return result;
        }
        private int MVS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("MVS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int CVS(Operand lhs,Operand rhs)
        {
            MessageBox.Show("CVS Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int CVI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CVI Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int LDB(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int LDW(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int LNS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LNS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int LDBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int LDWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int TAS(Operand lhs, Operand rhs)
        {
            //TODO Implement TAS
            MessageBox.Show("TAS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int STB(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int STW(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int STBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int STWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        private int PUSH(Operand lhs, Operand rhs)
        {
            SimulatorProgram p = GetCurrentProgram();
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                p.Stack.pushItem(new StackItem(lhs.Register.Value));
            }
            else
            {
                p.Stack.pushItem(new StackItem(lhs.Value));
            }
            //MessageBox.Show("PUSH Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        private int POP(Operand lhs, Operand rhs)
        {
            SimulatorProgram p = GetCurrentProgram();
            if (!lhs.IsRegister)
            {
                MessageBox.Show("Operand for POP instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            lhs.Register.Value = p.Stack.popItem();
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result,EnumOperandType.VALUE);
            //MessageBox.Show("POP Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return result;
        }
        private int SWP(Operand lhs, Operand rhs)
        {
            if (!lhs.IsRegister || !rhs.IsRegister)
            {
                MessageBox.Show("ERROR SWP Both operands must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return 0;
            }
            else
            {
                int leftvalue = lhs.Register.Value;
                int rightvalue = rhs.Register.Value;
                lhs.Register.Value = rightvalue;
                rhs.Register.Value = leftvalue;
                result = lhs.Register.Value;
                return result;
            }
        }
        private int ADD(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of ADD instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if(rhs.IsRegister)
            {
                lhs.Register.Value += Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value += rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        #endregion Data Transfer
        #endregion Instruction Execution Functions

        #region Window Accessor Methods
        private SimulatorProgram GetCurrentProgram()
        {
            //Assembly main = Assembly.GetEntryAssembly();
            //Type WindowType = main.GetType("MainWindow");
            //var window = GetActiveWindow(WindowType).First();

            //return null;
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll");
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString());
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null);
            string programName = window.currentProgram;
            List<SimulatorProgram> programs = window.ProgramList;
            SimulatorProgram prog = programs.Where(x => x.Name.Equals(programName)).FirstOrDefault();
            return prog;
        }

        private List<Window> GetActiveWindow(Type WindowType)
        {
            List<Window> windows = new List<Window>();
            foreach(Window window in Application.Current.Windows)
            {
                if (window.GetType() == WindowType)
                    windows.Add(window);
                else
                    continue;

            }
            return windows;
        }
        #endregion Window Accessor Methods
    }
}