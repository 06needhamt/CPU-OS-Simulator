using System;
using System.Linq;
using System.Threading;

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
            currentIndex = 0;
            logicalAddress = currentIndex * 4;
            currentInstruction = program.Instructions.Where(x => x.LogicalAddress == logicalAddress).FirstOrDefault();
            stop = false;
            done = false;
            SpecialRegister.FindSpecialRegister("BR").setRegisterValue(program.BaseAddress, EnumOperandType.ADDRESS);
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
            logicalAddress = currentIndex * 4;
            currentInstruction = program.Instructions.Where(x => x.LogicalAddress == logicalAddress).FirstOrDefault();
            stop = false;
            done = false;
            SpecialRegister.FindSpecialRegister("BR").setRegisterValue(program.BaseAddress, EnumOperandType.ADDRESS);
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
            Console.WriteLine(logicalAddress);
            currentInstruction = program.Instructions.Where(x => x.LogicalAddress == logicalAddress).FirstOrDefault();
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
            Thread.Sleep(clockSpeed);
        }

        #endregion Methods

        #region Properties
        /// <summary>
        /// Property for the current program being executed
        /// </summary>
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
        /// <summary>
        /// Property for the clock speed that the virtual CPU is running at
        /// </summary>
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
        /// <summary>
        /// Property for whether the execution unit should be stopped
        /// </summary>
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
        /// <summary>
        /// Property for whether the execution unit has reached the end of the program
        /// </summary>
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
        /// <summary>
        /// Property for the index of the instruction currently being executed
        /// </summary>
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
        /// <summary>
        /// Property for the logical address of the instruction currently being executed
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
        /// Property for the current instruction being executed
        /// </summary>
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