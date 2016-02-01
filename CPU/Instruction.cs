using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows;
using CPU_OS_Simulator.Memory;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents an instruction witch can be executed by the virtual CPU
    /// </summary>
    [Serializable]
    public class Instruction
    {
        #region Global Variables

        /// <summary>
        /// The opcode for the instruction
        /// </summary>
        private int opcode;

        /// <summary>
        /// The category in which the instruction will be displayed within the interface
        /// </summary>
        private string category;

        /// <summary>
        /// The function that will be executed when the instruction is executed
        /// </summary>
        [ScriptIgnore]
        [JsonIgnore]
        private Func<int> execute;

        /// <summary>
        /// The first operand for the instruction
        /// </summary>
        private Operand operand1;

        /// <summary>
        /// The second operand for the instruction
        /// </summary>
        private Operand operand2;

        /// <summary>
        /// The Logical address of this instruction within a program
        /// </summary>
        private int logicalAddress;

        /// <summary>
        /// The physical address of this instruction within memory
        /// </summary>
        private int physicalAddress;

        /// <summary>
        /// The size of the instruction within the program
        /// </summary>
        private int size;

        /// <summary>
        /// The result of the instruction once executed e.g. the result of ADD 1,1 would be 2
        /// </summary>
        private int result;

        /// <summary>
        /// The string representation of this instruction e.g. ADD R01,10
        /// </summary>
        private string instructionString;
        /// <summary>
        /// The execution unit that will be executing this instruction 
        /// </summary>
        
        private EnumAddressType op1mem = EnumAddressType.UNKNOWN;

        private EnumAddressType op2mem = EnumAddressType.UNKNOWN;

        [ScriptIgnore]
        [NonSerialized]
        [JsonIgnore]
        private ExecutionUnit unit;

        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Default constructor for instruction used when deserialising an instruction
        /// NOTE Do not use in code
        /// </summary>
        public Instruction()
        {
        }

        /// <summary>
        /// Constructor for an instruction that takes no operands
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(int opcode, int size)
        {
            this.opcode = opcode;
            operand1 = null;
            op1mem = EnumAddressType.UNKNOWN;
            operand2 = null;
            op2mem = EnumAddressType.UNKNOWN;
            this.size = size;
            instructionString = ToString();
            BindDelegate();
        }

        /// <summary>
        /// Constructor for an instruction that takes one operand
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="op1"> the first operand of the instruction</param>
        /// <param name="op1mem"> whether the first operand is a memory address</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(int opcode, Operand op1, EnumAddressType op1mem, int size)
        {
            this.opcode = opcode;
            operand1 = op1;
            this.op1mem = op1mem;
            operand2 = null;
            this.op2mem = EnumAddressType.UNKNOWN;
            this.size = size;
            instructionString = ToString();
            BindDelegate();
        }

        /// <summary>
        /// Constructor for an instruction that takes two operands
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="op1"> the first operand of the instruction</param>
        /// <param name="op1mem"> whether the first operand is a memory address</param>
        /// <param name="op2"> the second operand of the instruction</param>
        /// <param name="op2mem"> whether the second operand is a memory address</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(int opcode, Operand op1, EnumAddressType op1mem, Operand op2, EnumAddressType op2mem, int size)
        {
            this.opcode = opcode;
            operand1 = op1;
            this.op1mem = op1mem;
            operand2 = op2;
            this.op2mem = op2mem;
            this.size = size;
            instructionString = ToString();
            BindDelegate();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The opcode for the instruction
        /// </summary>
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
        /// <summary>
        /// The category in which the instruction will be displayed within the interface
        /// </summary>
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
        /// <summary>
        /// The first operand for the instruction
        /// </summary>
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
        /// <summary>
        /// The second operand for the instruction
        /// </summary>
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
        /// <summary>
        /// The function that will be executed when the instruction is executed
        /// </summary>
        [ScriptIgnore]
        [JsonIgnore]
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
        /// <summary>
        /// The result of the instruction once executed e.g. the result of ADD 1,1 would be 2
        /// </summary>
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
        /// <summary>
        /// The size of the instruction within the program
        /// </summary>
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
        /// <summary>
        /// The Logical address of this instruction within a program
        /// </summary>
        public int LogicalAddress
        {
            get
            {
                return logicalAddress;
            }

            set
            {
                logicalAddress = value;
            }
        }
        /// <summary>
        /// The string representation of this instruction e.g. ADD R01,10
        /// </summary>
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
        /// <summary>
        /// The physical address of this instruction within memory
        /// </summary>
        public int PhysicalAddress
        {
            get
            {
                return physicalAddress;
            }

            set
            {
                physicalAddress = value;
            }
        }
        /// <summary>
        /// The execution unit that will be executing this instruction
        /// </summary>
        [ScriptIgnore]
        [JsonIgnore]
        public ExecutionUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        /// <summary>
        /// Property for the memory address type of the first operand
        /// </summary>
        public EnumAddressType Op1Mem
        {
            get { return op1mem; }
            set { op1mem = value; }
        }
        /// <summary>
        /// Property for the memory address type of the first operand
        /// </summary>
        public EnumAddressType Op2Mem
        {
            get { return op2mem; }
            set { op2mem = value; }
        }

        #endregion Properties

        #region Methods


        private int GetIndirectAddress(int address)
        {
            int pageNumber = 0;
            int pageOffset = 0;
            byte[] bytes = new byte[sizeof(int)];
            for (int i = 0; i < bytes.Length; i++)
            {
                pageNumber = address / MemoryPage.PAGE_SIZE;
                pageOffset = address % MemoryPage.PAGE_SIZE;
                byte? loadByte = LoadByte(pageNumber, pageOffset);
                if (loadByte != null) bytes[i] = loadByte.Value;
                address++;
            }
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// this function binds a delegate to an instruction.
        /// The delegate bound here will be called when the instruction is executed
        /// </summary>
        public void BindDelegate()
        {
            switch (opcode)
            {
                case 0:
                    {
                        execute = () => MOV(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 1:
                    {
                        execute = () => MVS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 2:
                    {
                        execute = () => CVS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 3:
                    {
                        execute = () => CVI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 4:
                    {
                        execute = () => LDB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 5:
                    {
                        execute = () => LDW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 6:
                    {
                        execute = () => LNS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 7:
                    {
                        execute = () => LDBI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 8:
                    {
                        execute = () => LDWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 9:
                    {
                        execute = () => TAS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 10:
                    {
                        execute = () => STB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 11:
                    {
                        execute = () => STW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 12:
                    {
                        execute = () => STBI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 13:
                    {
                        execute = () => STWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 14:
                    {
                        execute = () => PUSH(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 15:
                    {
                        execute = () => POP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 16:
                    {
                        execute = () => SWP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 17:
                    {
                        execute = () => AND(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 18:
                    {
                        execute = () => OR(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 19:
                    {
                        execute = () => NOT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 20:
                    {
                        execute = () => SHL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 21:
                    {
                        execute = () => SHR(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 22:
                    {
                        execute = () => ADD(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 23:
                    {
                        execute = () => SUB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 24:
                    {
                        execute = () => SUBU(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 25:
                    {
                        execute = () => MUL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 26:
                    {
                        execute = () => DIV(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 27:
                    {
                        execute = () => INC(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 28:
                    {
                        execute = () => DEC(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 29:
                    {
                        execute = () => JMP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 30:
                    {
                        execute = () => JEQ(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 31:
                    {
                        execute = () => JNE(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 32:
                    {
                        execute = () => JGT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 33:
                    {
                        execute = () => JGE(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 34:
                    {
                        execute = () => JLT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 35:
                    {
                        execute = () => JLE(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 36:
                    {
                        execute = () => JNZ(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 37:
                    {
                        execute = () => JZR(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 38:
                    {
                        execute = () => CALL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 39:
                    {
                        execute = () => LOOP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 40:
                    {
                        execute = () => JSEL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 41:
                    {
                        execute = () => TABE(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 42:
                    {
                        execute = () => TABI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 43:
                    {
                        execute = () => MSF(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 44:
                    {
                        execute = () => RET(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 45:
                    {
                        execute = () => IRET(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 46:
                    {
                        execute = () => SWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 47:
                    {
                        execute = () => HLT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 48:
                    {
                        execute = () => CMP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 49:
                    {
                        execute = () => CPS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 50:
                    {
                        execute = () => IN(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 51:
                    {
                        execute = () => OUT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 52:
                    {
                        execute = () => NOP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 53:
                    {
                        execute = () => LDDW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 54:
                    {
                        execute = () => LDDWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 55:
                    {
                        execute = () => STDW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 56:
                    {
                        execute = () => STDWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private int FindRequiredPage(int pagenumber)
        {
            int programIndex = 0;
            int frameNumber = 0;
            dynamic wind = GetMainWindowInstance();
            //Func<Task<List<SimulatorProgram>>> listfunc = (Func<Task<List<SimulatorProgram>>>) wind.asyncGetProgList;
            List<SimulatorProgram> programList = wind.ProgramList;
            if (programIndex >= 0)
            {
                for (int i = 0; i < programIndex; i++)
                {
                    frameNumber += programList[i].Pages;
                }
                frameNumber += pagenumber;
            }
            return frameNumber;
        }

        /// <summary>
        /// Returns a string that represents this instruction
        /// </summary>
        /// <returns> a string that represents this instruction</returns>
        public override string ToString()
        {
            string parsedOpcode = Enum.GetName(typeof(EnumOpcodes), opcode);
            string op1 = "";
            string op2 = "";
            string finalString = "";

            if (operand1 != null)
            {
                if (op1mem == EnumAddressType.DIRECT)
                {
                    op1 += "@";
                }
                else if(op1mem == EnumAddressType.INDIRECT)
                {
                    op1 += "!";
                }
                if (operand1.IsRegister)
                {
                    op1 += Operand1.Register.Name;
                }
                else
                {
                    op1 += Operand1.Value.ToString();
                }
            }
            if (operand2 != null)
            {
                if (op2mem == EnumAddressType.DIRECT)
                {
                    op2 += "@";
                }
                else if (op2mem == EnumAddressType.INDIRECT)
                {
                    op2 += "!";
                }
                if (Operand2.IsRegister)
                {
                    op2 = Operand2.Register.Name;
                }
                else
                {
                    op2 = Operand2.Value.ToString();
                }
            }
            finalString += parsedOpcode.ToUpper() + " ";
            if (!op1.Equals(""))
            {
                finalString += op1.ToUpper();
            }
            if (!op2.Equals(""))
            {
                finalString += ",";
                finalString += op2.ToUpper();
            }
            return finalString;
        }

        private void StoreByte(int pageNumber, int pageOffset, byte value)
        {
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            while (pageOffset > 255)
            {
                pageNumber++;
                pageOffset -= 255;
            }
            int frameNumber = FindRequiredPage(pageNumber);
            if (memory.RequestMemoryPage(frameNumber) != null)
            {
                memory.Pages[frameNumber].Data[pageOffset / 8].SetByte(pageOffset % 8, value);
            }
            else
            {
                MessageBox.Show("Could not find memory page located in frame " + frameNumber);
            }
        }

        private byte? LoadByte(int pageNumber, int pageOffset)
        {
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            while (pageOffset > 255)
            {
                pageNumber++;
                pageOffset -= 255;
            }
            int frameNumber = FindRequiredPage(pageNumber);
            if (memory.RequestMemoryPage(frameNumber) != null)
            {
                return memory.Pages[frameNumber].Data[pageOffset/8].GetByte(pageOffset%8);
            }
            else
            {
                MessageBox.Show("Could not find memory page located in frame " + frameNumber);
                return null;
            }
        }
        #endregion Methods

        #region Instruction Execution Functions

        #region Data Transfer

        /// <summary>
        /// This function is called whenever a MOV instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MOV(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister) 
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                for (int i = 0; i < sourceBytes.Length; i++)
                {
                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte byteToStore = sourceBytes[i];
                    StoreByte(pageNumber, pageOffset, byteToStore);
                }
            }
            else
            {
                if (lhs.IsRegister)
                {
                    Register.FindRegister(lhs.Register.Name)
                        .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of MOV instruction Must be a register or address");
                }
            }
            return result;
            //result = rhs.Value;
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = result;
            //    Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //return result;

        }

        /// <summary>
        /// This function is called whenever a MVS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MVS(Operand lhs, Operand rhs)
        {
           
            MessageBox.Show("MVS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a CVS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CVS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CVS Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a CVI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CVI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CVI Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LDB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDB(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LDW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDW(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LNS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LNS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LNS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LDBI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LDWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a TAS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int TAS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("TAS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STB(Operand lhs, Operand rhs)
        {
            int address = 0;
            int value = 0;
            int pagenumber = 0;
            int pageoffset = 0;
            byte b = 0xFF;
            address = lhs.IsRegister ? Register.FindRegister(lhs.Register.Name).Value : lhs.Value;
            if (rhs.IsRegister)
            {
                SimulatorProgram program = GetCurrentProgram();
                value = Register.FindRegister(rhs.Register.Name).Value;
                b = (byte) (value & 0xFF);
                pagenumber = address/MemoryPage.PAGE_SIZE;
                pageoffset = address%MemoryPage.PAGE_SIZE;
            }
            else
            {
                SimulatorProgram program = GetCurrentProgram();
                pagenumber = address/MemoryPage.PAGE_SIZE;
                pageoffset = address%MemoryPage.PAGE_SIZE;
                value = rhs.Value;
                b = (byte) (value & 0xFF);
            }
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            int frameNumber = FindRequiredPage(pagenumber);
            if (memory.RequestMemoryPage(frameNumber) != null)
            {
                memory.Pages[frameNumber].Data[pageoffset / 8].SetByte(pageoffset % 8,b);
            }
            else
            {
                return int.MinValue;
            }
            //MessageBox.Show("STB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            #region OLD
            //int address;
            //byte value;
            //if (lhs.IsRegister)
            //    address = Register.FindRegister(lhs.Register.Name).Value;
            //else
            //    address = lhs.Value;
            //if (rhs.IsRegister)
            //{
            //    SimulatorProgram program = GetCurrentProgram();
            //    value = (byte)Register.FindRegister(rhs.Register.Name).Value;
            //    int pagenumber = address /MemoryPage.PAGE_SIZE;
            //    int pageOffset = address - (address /MemoryPage.PAGE_SIZE);
            //    program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8, Convert.ToByte(value));
            //}
            //else
            //{
            //    SimulatorProgram program = GetCurrentProgram();
            //    int pagenumber = address /MemoryPage.PAGE_SIZE;
            //    int pageOffset = address - (address /MemoryPage.PAGE_SIZE);
            //    value = (byte)rhs.Value;

            //    program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8, Convert.ToByte(value));
            //}
            //MessageBox.Show("STB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            #endregion OLD
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STW(Operand lhs, Operand rhs)
        {
            int address = 0;
            int value = 0;
            int pagenumber = 0;
            int pageOffset = 0;
            byte low = 0;
            byte high = 0;

            address = lhs.IsRegister ? Register.FindRegister(lhs.Register.Name).Value : lhs.Value;
            if (rhs.IsRegister)
            {
                SimulatorProgram program = GetCurrentProgram();
                value = Register.FindRegister(rhs.Register.Name).Value;
                low = (byte)(value & 0xFF);
                high = (byte)((value >> 8) & 0xFF);
                if (BitConverter.IsLittleEndian)
                {
                    byte templow = low;
                    byte temphigh = high;
                    high = templow;
                    low = temphigh;
                }
                pagenumber = address/MemoryPage.PAGE_SIZE;
                pageOffset = address%MemoryPage.PAGE_SIZE;

            }
            else
            {
                SimulatorProgram program = GetCurrentProgram();
                pagenumber = address/MemoryPage.PAGE_SIZE;
                pageOffset = address%MemoryPage.PAGE_SIZE;
                value = rhs.Value;
                low = (byte)(value & 0xFF);
                high = (byte)((value >> 8) & 0xFF);
                if (BitConverter.IsLittleEndian)
                {
                    byte templow = low;
                    byte temphigh = high;
                    high = templow;
                    low = temphigh;
                }
            }
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            int frameNumber = FindRequiredPage(pagenumber);
            if (memory.RequestMemoryPage(frameNumber) != null)
            {
                memory.Pages[frameNumber].Data[pageOffset / 8].SetByte(pageOffset % 8, high);
                Console.WriteLine("High Byte value = " + memory.Pages[frameNumber].Data[pageOffset/8].GetByte(pageOffset % 8));
                if (pageOffset % 8 == 7)
                {
                    memory.Pages[frameNumber].Data[pageOffset / 8 + 1].SetByte((pageOffset + 1) % 8, low);
                    Console.WriteLine("Low Byte value = " + memory.Pages[frameNumber].Data[pageOffset / 8 + 1].GetByte((pageOffset + 1) % 8));

                }
                else
                {
                    memory.Pages[frameNumber].Data[pageOffset / 8].SetByte((pageOffset % 8 + 1), low);
                    Console.WriteLine("Low Byte value = " + memory.Pages[frameNumber].Data[pageOffset / 8].GetByte(pageOffset % 8 + 1));

                }
            }
            else
            {
                return int.MinValue;
            }

            #region OLD
            //int address;
            //int value;
            //if (lhs.IsRegister)
            //    address = Register.FindRegister(lhs.Register.Name).Value;
            //else
            //    address = lhs.Value;
            //if (rhs.IsRegister)
            //{
            //    SimulatorProgram program = GetCurrentProgram();
            //    value = Register.FindRegister(rhs.Register.Name).Value;
            //    byte lowbyte = (byte) (value & 0xFF);
            //    byte highbyte = (byte)((value >> 8) & 0xFF);
            //    int pagenumber = address /MemoryPage.PAGE_SIZE;
            //    int pageOffset = address - (address /MemoryPage.PAGE_SIZE);
            //    program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8, highbyte);
            //    if (pageOffset%8 == 7)
            //    {
            //        program.Memory.ElementAt(pagenumber).Data[pageOffset / 8 + 1].SetByte((pageOffset + 1) % 8, lowbyte);
            //    }
            //    else
            //    {
            //        program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8, lowbyte);
            //    }


            //}
            //else
            //{
            //    SimulatorProgram program = GetCurrentProgram();
            //    int pagenumber = address /MemoryPage.PAGE_SIZE;
            //    int pageOffset = address - (address /MemoryPage.PAGE_SIZE);
            //    value = rhs.Value;
            //    byte lowbyte = (byte)(value & 0xFF);
            //    byte highbyte = (byte)((value >> 8) & 0xFF);

            //    program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8,highbyte );

            //    if (pageOffset % 8 == 7)
            //    {
            //        program.Memory.ElementAt(pagenumber).Data[pageOffset / 8 + 1].SetByte((pageOffset + 1) % 8, lowbyte);
            //    }
            //    else
            //    {
            //        program.Memory.ElementAt(pagenumber).Data[pageOffset / 8].SetByte(pageOffset % 8, lowbyte);
            //    }
            //}
            //MessageBox.Show("STW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            #endregion OLD
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STBI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a PUSH instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
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
            Thread.Sleep(20);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a POP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int POP(Operand lhs, Operand rhs)
        {
            SimulatorProgram p = GetCurrentProgram();
            if (!lhs.IsRegister)
            {
                MessageBox.Show("Operand for POP instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            lhs.Register.Value = p.Stack.popItem();
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //MessageBox.Show("POP Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            Thread.Sleep(20);
            return result;
        }

        /// <summary>
        /// This function is called whenever a SWP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SWP(Operand lhs, Operand rhs)
        {
            if (!lhs.IsRegister || !rhs.IsRegister)
            {
                MessageBox.Show("ERROR SWP Both operands must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            int leftvalue = lhs.Register.Value;
            int rightvalue = rhs.Register.Value;
            lhs.Register.Value = rightvalue;
            rhs.Register.Value = leftvalue;
            Register.FindRegister(rhs.Register.Name).SetRegisterValue(lhs.Register.Value, EnumOperandType.VALUE);
            Register.FindRegister(lhs.Register.Name).SetRegisterValue(rhs.Register.Value, EnumOperandType.VALUE);
            result = lhs.Register.Value;
            return result;
        }

        /// <summary>
        /// This function is called whenever a LDDW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDDW(Operand lhs, Operand rhs)
        {
            int value = 0;
            int address = 0;
            byte[] bytes = new byte[4];
            if (rhs.Type == EnumOperandType.ADDRESS)
            {
                if (rhs.IsRegister)
                {
                    address = Register.FindRegister(rhs.Register.Name).Value;
                }
                else
                {
                    address = rhs.Value;
                }
                if (op2mem == EnumAddressType.INDIRECT)
                {
                    address = GetIndirectAddress(address);
                }
            }
            else
            {
                MessageBox.Show("Source operand of LDDW must be an address");
                return int.MinValue;
            }
            if (lhs.IsRegister && lhs.Type != EnumOperandType.ADDRESS)
            {
                int pageNumber = address / MemoryPage.PAGE_SIZE;
                int pageOffset = address % MemoryPage.PAGE_SIZE;
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte? loadByte = LoadByte(pageNumber, pageOffset + i);
                    if (loadByte != null)
                        bytes[i] = loadByte.Value;
                }
                value = BitConverter.ToInt32(bytes,0);
                result = value;
                Register.FindRegister(lhs.Register.Name).SetRegisterValue(result,EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("Destination Operand of LDDW must be a value register");
                return int.MinValue;
            }

            //MessageBox.Show("LDDW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return result;
        }

        /// <summary>
        /// This function is called whenever a LDDWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDDWI(Operand lhs, Operand rhs)
        {   
            MessageBox.Show("LDDWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STDW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STDW(Operand lhs, Operand rhs)
        {
            int value = 0;
            int address = 0;

            byte[] bytes = new byte[4];

            if (rhs.IsRegister && rhs.Type != EnumOperandType.ADDRESS)
            {
                value = Register.FindRegister(rhs.Register.Name).Value;
                bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }
            else
            {
                MessageBox.Show("Source Operand of STDW must be a value register");
                return int.MinValue;
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                if (lhs.IsRegister)
                {
                    address = Register.FindRegister(lhs.Register.Name).Value;
                }
                else
                {
                    address = lhs.Value;
                }
                if (op1mem == EnumAddressType.INDIRECT)
                {
                    address = GetIndirectAddress(address);
                }
            }
            else
            {
                MessageBox.Show("Destination Operand of STDW must be a Address");
                return int.MinValue;
            }
            int pageNumber = address/MemoryPage.PAGE_SIZE;
            int pageOffset = address%MemoryPage.PAGE_SIZE;
            for (int i = 0; i < bytes.Length; i++)
            {
                StoreByte(pageNumber, pageOffset + i, bytes[i]);
            }
            //MessageBox.Show("STDW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a STDWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STDWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STDWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        #endregion Data Transfer

        #region Logical

        /// <summary>
        /// This function is called whenever a AND instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int AND(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value &= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value &= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of AND instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of AND instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value &= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value &= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a OR instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int OR(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value |= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value |= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of OR instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of OR instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value |= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value |= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a NOT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int NOT(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int source = BitConverter.ToInt32(sourceBytes, 0);
                int value = ~source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    int value = ~source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of NOT instruction Must be a register or address");
                }
            }
            return result;

            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //}
            //else
            //{
            //    MessageBox.Show("First operand of NOT instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value = ~Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value = ~rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a SHL instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SHL(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof (int)];
            byte[] destBytes = new byte[sizeof (int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address/MemoryPage.PAGE_SIZE;
                    int pageOffset = address%MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address/MemoryPage.PAGE_SIZE;
                    pageOffset = address%MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value <<= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value <<= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of SHL instruction Must be a register or address");
                }
            }
            return result;

            #region OLD

            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of SHL instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value <<= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value <<= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;

            #endregion OLD
        }

        /// <summary>
            /// This function is called whenever a SHR instruction is executed
            /// </summary>
            /// <param name="lhs"> The left hand operand of the instruction </param>
            /// <param name="rhs"> The right hand operand of the instruction </param>
            /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SHR(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value >>= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value >>= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of SHR instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of SHR instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value >>= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value >>= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        #endregion Logical

        #region Arithmetic

        /// <summary>
        /// This function is called whenever a ADD instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int ADD(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes,0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value += source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    if (pageOffset < 0)
                    {
                        pageOffset = 0;
                    }
                    StoreByte(pageNumber,pageOffset + i,destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value += source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value,EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of ADD instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of ADD instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value += Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value += rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a SUB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SUB(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value -= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value -= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of SUB instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of SUB instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value -= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value -= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a SUBU instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SUBU(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value += source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value = Math.Abs(value - source);
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of SUBU instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of ADD instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value = Math.Abs(lhs.Register.Value - Register.FindRegister(rhs.Register.Name).Value);
            //}
            //else
            //{
            //    lhs.Register.Value = Math.Abs(lhs.Register.Value - rhs.Value);
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a MUL instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MUL(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                value *= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    value *= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of MUL instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of MUL instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    lhs.Register.Value *= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    lhs.Register.Value *= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a DIV instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int DIV(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof (int)];
            byte[] destBytes = new byte[sizeof (int)];
            if (rhs.Type == EnumOperandType.ADDRESS) //if the source operand is a memory address 
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address/MemoryPage.PAGE_SIZE;
                    int pageOffset = address%MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address/MemoryPage.PAGE_SIZE;
                    pageOffset = address%MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = BitConverter.ToInt32(sourceBytes, 0);
                if (source == 0)
                {
                    MessageBox.Show("Cannot Divide by zero (0)");
                    return int.MinValue;
                }
                else
                {
                    value /= source;
                    pageOffset -= 3;
                    destBytes = BitConverter.GetBytes(value);
                    for (int i = 0; i < destBytes.Length; i++)
                    {
                        StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                    }
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = BitConverter.ToInt32(sourceBytes, 0);
                    if (source == 0)
                    {
                        MessageBox.Show("Cannot Divide by zero (0)");
                        return int.MinValue;
                    }
                    else
                    {
                        value /= source;
                        Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                    }
                }
                else
                {
                    MessageBox.Show("Destination of DIV instruction Must be a register or address");
                }
            }
            return result;

            #region OLD

            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    result = lhs.Register.Value;
            //    //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //}
            //else
            //{
            //    MessageBox.Show("First operand of DIV instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return int.MinValue;
            //}
            //if (rhs.IsRegister)
            //{
            //    if (Register.FindRegister(rhs.Register.Name).Value == 0)
            //    {
            //        MessageBox.Show("Cannot Divide by ZERO", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //        lhs.Register.Value = 0;
            //        result = int.MinValue;
            //        return result;
            //    }
            //    lhs.Register.Value /= Register.FindRegister(rhs.Register.Name).Value;
            //}
            //else
            //{
            //    if (rhs.Value == 0)
            //    {
            //        MessageBox.Show("Cannot Divide by ZERO", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //        lhs.Register.Value = 0;
            //        result = int.MinValue;
            //        return result;
            //    }
            //    lhs.Register.Value /= rhs.Value;
            //}
            //result = lhs.Register.Value;
            //Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //return result;
            #endregion OLD
        }

        /// <summary>
            /// This function is called whenever a INC instruction is executed
            /// </summary>
            /// <param name="lhs"> The left hand operand of the instruction </param>
            /// <param name="rhs"> The right hand operand of the instruction </param>
            /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int INC(Operand lhs, Operand rhs)
        {
            byte[] destBytes = new byte[sizeof(int)];
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = 1;
                value += source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = 1;
                    value += source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of INC instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    lhs.Register.Value++;
            //    result = lhs.Register.Value;
            //    Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //    return result;
            //}
            //MessageBox.Show("Operand of INC instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //return int.MinValue;
            #endregion OLD
        }

        /// <summary>
        /// This function is called whenever a DEC instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int DEC(Operand lhs, Operand rhs)
        {
            byte[] destBytes = new byte[sizeof(int)];
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                int pageNumber = 0;
                int pageOffset = 0;
                for (int i = 0; i < destBytes.Length; i++)
                {

                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    pageNumber = address / MemoryPage.PAGE_SIZE;
                    pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
                int value = BitConverter.ToInt32(destBytes, 0);
                int source = 1;
                value -= source;
                pageOffset -= 3;
                destBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < destBytes.Length; i++)
                {
                    StoreByte(pageNumber, pageOffset + i, destBytes[i]);
                }

            }
            else
            {
                if (lhs.IsRegister)
                {
                    //Register.FindRegister(lhs.Register.Name)
                    //    .SetRegisterValue(BitConverter.ToInt32(sourceBytes, 0), EnumOperandType.VALUE);
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                    int value = BitConverter.ToInt32(destBytes, 0);
                    int source = 1;
                    value -= source;
                    Register.FindRegister(lhs.Register.Name).SetRegisterValue(value, EnumOperandType.VALUE);
                }
                else
                {
                    MessageBox.Show("Destination of DEC instruction Must be a register or address");
                }
            }
            return result;
            #region OLD
            //if (lhs.IsRegister)
            //{
            //    lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
            //    lhs.Register.Value++;
            //    result = lhs.Register.Value;
            //    Register.FindRegister(lhs.Register.Name).SetRegisterValue(result, EnumOperandType.VALUE);
            //    return result;
            //}
            //MessageBox.Show("Operand of DEC instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
            //return int.MinValue;
            #endregion OLD
        }

        #endregion Arithmetic

        #region Control Transfer
        /// <summary>
        /// This function is called whenever a JMP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JMP(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            dynamic window = GetMainWindowInstance();
            unit = window.ActiveUnit;
            SimulatorProgram prog = GetCurrentProgram();
            if (unit == null)
            {
                dynamic procunit = GetCurrentProcessExecutionUnit();
                procunit.LogicalAddress = lhs.Value;
                procunit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                procunit.Done = false;
                procunit.Stop = false;
            }
            else
            {
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
            }
            return result;
        }
        /// <summary>
        /// This function is called whenever a JEQ instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JEQ(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (StatusFlags.Z.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value/4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JNE instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JNE(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (!StatusFlags.Z.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JGT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JGT(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (!StatusFlags.Z.IsSet && StatusFlags.N.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JGE instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JGE(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (StatusFlags.Z.IsSet || StatusFlags.N.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JLT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JLT(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (!StatusFlags.Z.IsSet && StatusFlags.N.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                if (unit == null)
                {
                    dynamic procunit = GetCurrentProcessExecutionUnit();
                    procunit.LogicalAddress = lhs.Value;
                    procunit.CurrentIndex = (lhs.Value/4);
                    result = lhs.Value;
                    procunit.Done = false;
                    procunit.Stop = false;
                }
                else
                {
                    unit.LogicalAddress = lhs.Value;
                    unit.CurrentIndex = (lhs.Value / 4);
                    result = lhs.Value;
                    unit.Done = false;
                    unit.Stop = false;
                }
                
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                if (unit == null)
                {
                    dynamic procunit = GetCurrentProcessExecutionUnit();
                    procunit.CurrentIndex++;
                }
                else
                {
                    unit = GetExecutionUnit();
                    unit.CurrentIndex++;
                }
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JLE instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JLE(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (StatusFlags.Z.IsSet || !StatusFlags.N.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                ExecutionUnit unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JNZ instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JNZ(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (!StatusFlags.Z.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a JZR instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JZR(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            }
            if (StatusFlags.Z.IsSet)
            {
                dynamic window = GetMainWindowInstance();
                unit = window.ActiveUnit;
                SimulatorProgram prog = GetCurrentProgram();
                unit.LogicalAddress = lhs.Value;
                unit.CurrentIndex = (lhs.Value / 4);
                result = lhs.Value;
                unit.Done = false;
                unit.Stop = false;
                //window.lst_InstructionsList.SelectedIndex = 0;
                return result;
            }
            else
            {
                unit = GetExecutionUnit();
                unit.CurrentIndex++;
            }
            return 0;
        }
        /// <summary>
        /// This function is called whenever a STWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CALL(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CALL Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a LOOP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LOOP(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LOOP Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a JSEL instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int JSEL(Operand lhs, Operand rhs)
        {
            MessageBox.Show("JSEL Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a TABE instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int TABE(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CALL Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a TABI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int TABI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("TABI Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a MSF instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MSF(Operand lhs, Operand rhs)
        {
            MessageBox.Show("MSF Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a RET instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int RET(Operand lhs, Operand rhs)
        {
            MessageBox.Show("RET Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a IRET instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int IRET(Operand lhs, Operand rhs)
        {
            MessageBox.Show("IRET Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a SWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CALL Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        /// <summary>
        /// This function is called whenever a HLT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int HLT(Operand lhs, Operand rhs)
        {
            if (unit == null)
            {
                unit = GetExecutionUnit();
                if (unit == null)
                {
                    dynamic procunit = GetCurrentProcessExecutionUnit();
                    if (procunit == null)
                    {
                        MessageBox.Show("There was an error while fetching the execution unit program terminating");
                        return int.MinValue;
                    }
                    else
                    {
                        procunit.Done = true;
                        result = 0; 
                        return result;
                    }
                }
            }
            unit.Done = true;
            result = 0;
            return result;
        }

        #endregion Control Transfer

        #region Comparison
        /// <summary>
        /// This function is called whenever a CMP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CMP(Operand lhs, Operand rhs)
        {
            byte[] sourceBytes = new byte[sizeof(int)];
            byte[] destBytes = new byte[sizeof(int)];

            StatusFlags.OV.IsSet = false;
            StatusFlags.N.IsSet = false;
            StatusFlags.Z.IsSet = false;

            if (rhs.Type == EnumOperandType.ADDRESS)
            {
                for (int i = 0; i < sourceBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (rhs.IsRegister)
                    {
                        address = Register.FindRegister(rhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = rhs.Value + i;
                    }
                    if (op2mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        sourceBytes[i] = loadedByte.Value;
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (rhs.IsRegister)
                {
                    sourceBytes = BitConverter.GetBytes(Register.FindRegister(rhs.Register.Name).Value);
                }
                else
                {
                    sourceBytes = BitConverter.GetBytes(rhs.Value);
                }
                Array.Reverse(sourceBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(sourceBytes);
                }
            }
            if (lhs.Type == EnumOperandType.ADDRESS)
            {
                for (int i = 0; i < destBytes.Length; i++) // load it from memory
                {
                    int address = 0;
                    if (lhs.IsRegister)
                    {
                        address = Register.FindRegister(lhs.Register.Name).Value + i;
                    }
                    else
                    {
                        address = lhs.Value + i;
                    }
                    if (op1mem == EnumAddressType.INDIRECT)
                    {
                        address = GetIndirectAddress(address - i) + i;
                    }
                    int pageNumber = address / MemoryPage.PAGE_SIZE;
                    int pageOffset = address % MemoryPage.PAGE_SIZE;
                    byte? loadedByte = LoadByte(pageNumber, pageOffset);
                    if (loadedByte != null)
                        destBytes[i] = loadedByte.Value;
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
            }
            else // otherwise load it from a register
            {
                if (lhs.IsRegister)
                {
                    destBytes = BitConverter.GetBytes(Register.FindRegister(lhs.Register.Name).Value);
                }
                else
                {
                    destBytes = BitConverter.GetBytes(lhs.Value);
                }
                Array.Reverse(destBytes);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(destBytes);
                }
            }

            int source = BitConverter.ToInt32(sourceBytes, 0);
            int value = BitConverter.ToInt32(destBytes, 0);

            if (source - value == 0)
            {
                StatusFlags.Z.IsSet = true;
            }
            else if (source - value < 0)
            {
                StatusFlags.N.IsSet = true;
            }
            return 0;
            
            #region OLD

            //StatusFlags.OV.IsSet = false;
            //StatusFlags.N.IsSet = false;
            //StatusFlags.Z.IsSet = false;
            //if (lhs.IsRegister)
            //{
            //    lhs.Value = Register.FindRegister(lhs.Register.Name).Value;
            //}
            //if (rhs.IsRegister)
            //{
            //    rhs.Value = Register.FindRegister(rhs.Register.Name).Value;
            //}
            //if ((rhs.Value - lhs.Value) == 0)
            //{
            //    StatusFlags.Z.IsSet = true;
            //}
            //else if ((rhs.Value - lhs.Value) < 0)
            //{
            //    StatusFlags.N.IsSet = true;
            //}
            //return 0;

            #endregion OLD
        }
        /// <summary>
        /// This function is called whenever a CPS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CPS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CPS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        #endregion Comparison

        #region I/O
        /// <summary>
        /// This function is called whenever a IN instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int IN(Operand lhs, Operand rhs)
        {
            MessageBox.Show("IN Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This function is called whenever a OUT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int OUT(Operand lhs, Operand rhs)
        {
            MessageBox.Show("OUT Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        #endregion I/O

        #region Miscellaneous
        /// <summary>
        /// This function is called whenever a NOP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int NOP(Operand lhs, Operand rhs)
        {
            Thread.Sleep(unit.ClockSpeed);
            //MessageBox.Show("NOP Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }
        #endregion Miscellaneous

        #endregion Instruction Execution Functions

        #region Window Accessor Methods

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        /// <summary>
        /// This Function gets the program to be executed by the CPU from the main window
        /// </summary>
        /// <returns>the program to be executed by the CPU</returns>
        private SimulatorProgram GetCurrentProgram()
        {
            dynamic window = GetMainWindowInstance();
            string programName = window.currentProgram; // get the name of the program that is currently loaded
            List<SimulatorProgram> programs = window.ProgramList; // get a copy of the program list
            SimulatorProgram prog = programs.Where(x => x.Name.Equals(programName)).FirstOrDefault(); // find the current program in the list
            return prog; // return the current program
        }

        private ExecutionUnit GetExecutionUnit()
        {
            dynamic window = GetMainWindowInstance();
            ExecutionUnit currentUnit = window.ActiveUnit;
            return currentUnit;
        }

        /// <summary>
        /// This function gets the operating system main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of operating system main window </returns>
        private dynamic GetOSMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("OperatingSystemMainWindowInstance").GetValue(null); // get the value of the static OperatingSystemMainWindowInstance field
            return window;
        }

        private dynamic GetCurrentProcessExecutionUnit()
        {
            dynamic osWindow = GetOSMainWindowInstance();
            dynamic osCore = osWindow.OsCore;
            dynamic scheduler = osCore.Scheduler;
            dynamic currentProcess = scheduler.RunningProcess;
            if (currentProcess == null)
                return null;
            dynamic currentUnit = currentProcess.Unit;
            return currentUnit;
        }

        /// <summary>
        /// This function returns all of the active window instances of a specific type
        /// </summary>
        /// <param name="WindowType"> the type of window to return</param>
        /// <returns> a list containing all active window instances</returns>
        [Obsolete("GetActiveWindow is depreciated please use GetMainWindowInstance instead", true)]
        private List<Window> GetActiveWindow(Type WindowType)
        {
            List<Window> windows = new List<Window>();
            foreach (Window window in Application.Current.Windows)
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