using System;
using System.Linq;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents the part of the CPU which executes instructions
    /// </summary>
    public class ExecutionUnit
    {
        #region Global Variables

        /// <summary>
        /// The current program being executed
        /// </summary>
        private SimulatorProgram program;

        /// <summary>
        /// The clock speed that the CPU is running at
        /// </summary>
        private int clockSpeed;

        /// <summary>
        /// The index of the instruction currently being executed
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// The instruction currently being executed
        /// </summary>
        private Instruction currentInstruction;
        
        /// <summary>
        /// The Logical address of the instruction currently being executed
        /// </summary>
        private int logicalAddress;

        /// <summary>
        /// Whether the unit has received a stop signal from the main window
        /// </summary>
        private bool stop;

        /// <summary>
        /// Whether the unit has reached the end of the program
        /// </summary>
        private bool done;

        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Constructor for execution unit that starts executing from the beginning of the program
        /// </summary>
        /// <param name="program"> the program to execute </param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ExecutionUnit(SimulatorProgram program, int clockSpeed)
        {
            this.program = program;
            this.clockSpeed = clockSpeed;
            this.currentIndex = 0;
            this.logicalAddress = currentIndex * 4;
            this.currentInstruction = program.Instructions.Where(x => x.LogicalAddress == this.logicalAddress).FirstOrDefault();
            stop = false;
            done = false;
        }

        /// <summary>
        /// Constructor for execution unit that starts executing from a specified location in the program
        /// </summary>
        /// <param name="program"> the program to execute </param>
        /// <param name="currentIndex"> the index to start executing from</param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ExecutionUnit(SimulatorProgram program, int clockSpeed, int currentIndex) : this(program, clockSpeed)
        {
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }
            this.currentIndex = currentIndex;
            this.logicalAddress = currentIndex * 4;
            this.currentInstruction = program.Instructions.Where(x => x.LogicalAddress == this.logicalAddress).FirstOrDefault();
            stop = false;
            done = false;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// This function executes an instruction by calling its delegate function
        /// </summary>
        public void ExecuteInstruction()
        {
            Console.WriteLine("Executing instruction");
            logicalAddress = currentIndex * 4;
            currentInstruction = program.Instructions.Where(x => x.LogicalAddress == this.logicalAddress).FirstOrDefault();
            if (currentInstruction.Opcode == (int)EnumOpcodes.JMP)
            {
                //program.Instructions.ElementAt(currentIndex).Execute();
                currentInstruction.Execute();
            }
            else
            {
                //program.Instructions.ElementAt(currentIndex).Execute();
                currentInstruction.Execute();
                currentIndex++;
            }
            if (currentIndex == program.Instructions.Count)
            {
                Done = true;
            }
        }

        #endregion Methods

        #region Properties

        public SimulatorProgram Program
        {
            get
            {
                return program;
            }

            set
            {
                program = value;
            }
        }

        public int ClockSpeed
        {
            get
            {
                return clockSpeed;
            }

            set
            {
                clockSpeed = value;
            }
        }

        public bool Stop
        {
            get
            {
                return stop;
            }

            set
            {
                stop = value;
            }
        }

        public bool Done
        {
            get
            {
                return done;
            }

            set
            {
                done = value;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }

            set
            {
                currentIndex = value;
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

        public Instruction CurrentInstruction
        {
            get
            {
                return currentInstruction;
            }

            set
            {
                currentInstruction = value;
            }
        }

        #endregion Properties
    }
}