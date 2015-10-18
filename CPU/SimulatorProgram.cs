using System;
using System.Collections.Generic;

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
        private Int32 pages;
        private List<Instruction> instructions;

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
            this.startAddress = baseAddress;
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

        public int StartAddress
        {
            get
            {
                return startAddress;
            }

            set
            {
                startAddress = value;
            }
        }

        #endregion Properties

        #region Methods

        public void AddInstruction(ref Instruction ins)
        {
            int address = CalculateAddress(ins);
            ins.Address = address;
            instructions.Add(ins);
        }

        private int CalculateAddress(Instruction instruction)
        {
            int address = baseAddress;
            foreach (Instruction ins in instructions)
            {
                address += ins.Size; // calculate address of the next intruction
            }
            address += instruction.Size;
            return address;
        }

        #endregion Methods
    }
}