using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script;
using System.Web.Script.Serialization;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.CPU.Interrupts
{
    /// <summary>
    /// This Class Represents an interrupt used by the operating system to interrupt the CPU
    /// </summary>
    [Serializable]
    public abstract class Interrupt
    {
        /// <summary>
        /// This function fires the interrupt
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// This function returns from the interrupt routine
        /// </summary>
        /// <param name="returnAddress">the logical address to return back to</param>
        public abstract void Return(int returnAddress);

        /// <summary>
        /// This function executes an interrupt
        /// </summary>
        /// <param name="handle"> the interrupts handler</param>
        /// <returns> the return value of the interrupt or null if the interrupt did not return a value</returns>
        public abstract int? Execute(InterruptHandler handle);

        /// <summary>
        /// Property for the logical address of the interrupt routine
        /// </summary>
        public abstract int LogicalAddress { get; }

        /// <summary>
        /// Property For the interrupt ID
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Property for the name of the routine
        /// </summary>
        public abstract string RoutineName { get; }

        /// <summary>
        /// Property for the interrupt handler
        /// </summary>
        public abstract InterruptHandler Handler { get; } 

        /// <summary>
        /// Property for the interrupt type
        /// </summary>
        public abstract EnumInterruptType InterruptType { get; }
    }
}
