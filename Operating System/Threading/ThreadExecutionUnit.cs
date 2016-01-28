using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    public class ThreadExecutionUnit : ProcessExecutionUnit
    {
        /// <summary>
        /// Empty constructor used when deserialising process execution units
        /// NOTE DO NOT USE IN CODE
        /// </summary>
        public ThreadExecutionUnit()
        {
        }

        /// <summary>
        /// Constructor for execution unit that starts executing from the beginning of the program
        /// </summary>
        /// <param name="program"> the thread to execute </param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ThreadExecutionUnit(SimulatorThread thread, int clockSpeed) : base(thread.OwnerProcess, clockSpeed)
        {
        }

        /// <summary>
        /// Constructor for execution unit that starts executing from a specified location in the program
        /// </summary>
        /// <param name="program"> the thread to execute </param>
        /// <param name="currentIndex"> the index to start executing from</param>
        /// <param name="clockSpeed"> the clock speed of the CPU </param>
        public ThreadExecutionUnit(SimulatorThread thread, int clockSpeed, int currentIndex) : base(thread.OwnerProcess, clockSpeed, currentIndex)
        {
        }
    }
}
