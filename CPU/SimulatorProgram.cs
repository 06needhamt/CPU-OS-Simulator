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
        private int baseAddress;
        private int startAddress;
        private int logicalAddress;
        private int pages;
        private const int DEFAULT_PAGE_SIZE = 256;

        [ScriptIgnore]
        [NonSerialized]
        private ExecutionUnit unit;

        private List<Instruction> instructions;

        [ScriptIgnore]
        private List<MemoryPage> memory;

        [ScriptIgnore]
        [NonSerialized]
        private ProgramStack stack;

        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Default constructor used when deserialising a simulator program
        /// NOTE: Do not use in code!
        /// </summary>
        public SimulatorProgram()
        {
        }

        /// <summary>
        /// Constructor for simulator program
        /// </summary>
        /// <param name="name"> The name of the program</param>
        /// <param name="baseAddress"> the base address of the program</param>
        /// <param name="pages"> the number of memory pages in the program</param>
        public SimulatorProgram(string name, int baseAddress, int pages)
        {
            this.name = name;
            this.baseAddress = baseAddress;
            this.pages = pages;
            instructions = new List<Instruction>();
            logicalAddress = 0;
            startAddress = baseAddress;
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
        /// <summary>
        /// Property for the logical address of the program
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
        /// Property for the stack of the program
        /// </summary>
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
        [Obsolete("function no longer used with new memory manager",true)]
        public List<MemoryPage> AllocateMemory()
        {
            List<MemoryPage> memoryPages = new List<MemoryPage>(pages);
            for (int i = 0; i < pages; i++)
            {
                memoryPages.Add(new MemoryPage(i, (i * MemoryPage.PAGE_SIZE)));
            }
            return memoryPages;
        }
        /// <summary>
        /// This function adds an instruction to the end of the program
        /// </summary>
        /// <param name="ins">The instruction to add</param>
        public void AddInstruction(ref Instruction ins)
        {
            //int address = CalculateAddress(ins,instructions.Count);
            //ins.LogicalAddress = address;
            instructions.Add(ins);
            UpdateAddresses();
        }

        /// <summary>
        /// This function adds an instruction to the program at the passed index
        /// </summary>
        /// <param name="ins"> the instruction to add</param>
        /// <param name="index"> the index to add the instruction at</param>
        public void AddInstruction(ref Instruction ins, int index)
        {
            //int address = CalculateAddress(ins,instructions.Count);
            //ins.LogicalAddress = address;
            instructions.Insert(index, ins);
            UpdateAddresses();
        }
        /// <summary>
        /// This function updates the addresses of the instructions 
        /// after an instruction is moved or deleted
        /// </summary>
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
        /// <summary>
        /// This function calculates an address for an instruction
        /// </summary>
        /// <param name="instruction"> the instruction to calculate the address for</param>
        /// <param name="index"> the index of the instruction </param>
        /// <returns></returns>
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