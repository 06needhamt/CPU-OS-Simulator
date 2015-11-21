using CPU_OS_Simulator.Memory;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents a program that can be run within the simulator
    /// </summary>
    [Serializable]
    public class SimulatorProgram
    {
        #region Global Variables

        private string name;
        private Int32 baseAddress;
        private Int32 startAddress;
        private Int32 logicalAddress;
        private Int32 pages;
        [ScriptIgnore]
        [NonSerialized]
        private ExecutionUnit unit;
        private List<Instruction> instructions;

        [ScriptIgnore]
        private List<MemoryPage_OLD> memory;

        [ScriptIgnore]
        [NonSerialized]
        private ProgramStack stack;

        #endregion Global Variables

        #region Constructors

        public SimulatorProgram()
        {
        }

        /// <summary>
        /// Constructor for simulator program
        /// </summary>
        /// <param name="name"> The name of the program</param>
        /// <param name="baseAddress"> the base address of the program</param>
        /// <param name="pages"> the number of memory pages in the program</param>
        public SimulatorProgram(string name, Int32 baseAddress, Int32 pages)
        {
            this.name = name;
            this.baseAddress = baseAddress;
            this.pages = pages;
            this.instructions = new List<Instruction>();
            this.logicalAddress = 0;
            this.startAddress = baseAddress;
            unit = new ExecutionUnit(this, 100);
            stack = new ProgramStack();
            Console.WriteLine("Program Created");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Property for the name of the program
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Property for the base address of the program
        /// </summary>
        public int BaseAddress
        {
            get
            {
                return baseAddress;
            }

            set
            {
                baseAddress = value;
            }
        }

        /// <summary>
        /// Property for the number of memory pages in the program
        /// </summary>
        public int Pages
        {
            get
            {
                return pages;
            }

            set
            {
                pages = value;
            }
        }

        /// <summary>
        /// Property for the linked list of instructions that make up the program
        /// </summary>
        public List<Instruction> Instructions
        {
            get
            {
                return instructions;
            }

            set
            {
                instructions = value;
            }
        }

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

        [ScriptIgnore]
        public List<MemoryPage_OLD> Memory
        {
            get
            {
                return memory;
            }

            set
            {
                memory = value;
            }
        }

        [ScriptIgnore]
        public ProgramStack Stack
        {
            get
            {
                return stack;
            }

            set
            {
                stack = value;
            }
        }

        #endregion Properties

        #region Methods

        public void AddInstruction(ref Instruction ins)
        {
            //int address = CalculateAddress(ins,instructions.Count);
            //ins.LogicalAddress = address;
            instructions.Add(ins);
            UpdateAddresses();
        }

        public void AddInstruction(ref Instruction ins, int index)
        {
            //int address = CalculateAddress(ins,instructions.Count);
            //ins.LogicalAddress = address;
            instructions.Insert(index, ins);
            UpdateAddresses();
        }

        public void UpdateAddresses()
        {
            int phyaddress = baseAddress;
            int logaddress = 0;
            for (int i = 0; i < instructions.Count; i++)
            {
                if (i == 0)
                {
                    instructions[i].PhysicalAddress = phyaddress;
                    instructions[i].LogicalAddress = logaddress;
                }
                else
                {
                    logaddress += instructions[i].Size;
                    instructions[i].LogicalAddress = logaddress;
                    phyaddress += instructions[i].Size; // calculate address of the next instruction
                    instructions[i].PhysicalAddress = phyaddress;
                }
            }
            //phyaddress += instruction.Size;
            //return logaddress;
        }

        private int CalculateAddress(Instruction instruction, int index)
        {
            int phyaddress = baseAddress;
            int logaddress = 0;
            for (int i = 0; i < index; i++)
            {
                if (i == 0)
                {
                    instructions[i].PhysicalAddress = phyaddress;
                    instructions[i].LogicalAddress = logaddress;
                }
                else
                {
                    logaddress += instructions[i].Size;
                    instructions[i].LogicalAddress = logaddress;
                    phyaddress += instructions[i].Size; // calculate address of the next instruction
                    instructions[i].PhysicalAddress = phyaddress;
                }
            }
            phyaddress += instruction.Size;
            return logaddress;
        }

        #endregion Methods
    }
}