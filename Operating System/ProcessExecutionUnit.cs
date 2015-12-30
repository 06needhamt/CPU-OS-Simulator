using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    public class ProcessExecutionUnit : ExecutionUnit
    {
        private ProcessControlBlock processControlBlock;
        private SimulatorProgram program;
        private SimulatorProcess process;

        /// <summary>
        /// Constructor for execution unit that starts executing from the beginning of the program
        /// </summary>
        /// <param name="program"> the program to execute </param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ProcessExecutionUnit(SimulatorProcess program, int clockSpeed) : base(program.Program, clockSpeed)
        {
            this.process = program;
            this.program = process.Program;
        }

        /// <summary>
        /// Constructor for execution unit that starts executing from a specified location in the program
        /// </summary>
        /// <param name="program"> the program to execute </param>
        /// <param name="currentIndex"> the index to start executing from</param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ProcessExecutionUnit(SimulatorProcess program, int clockSpeed, int currentIndex) : base(program.Program, clockSpeed, currentIndex)
        {
        }

        public void Terminate()
        { 

        }
    }
}