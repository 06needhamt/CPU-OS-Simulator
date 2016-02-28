﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Interrupts
{
    public class VectoredInterrupt : Interrupt
    {
        private int logicalAddress;
        private string routineName;
        private InterruptHandler handler;
        private int id;
        private EnumInterruptType interruptType;
        private bool handled = false;

        /// <summary>
        /// This function fires the interrupt
        /// </summary>
        /// <param name="RoutineName"> the name of the interrupt handling routine </param>
        /// <param name="logicalAddress"> the logical address of the start of the routine</param>
        public override void Fire(string RoutineName, int logicalAddress)
        {
            handled = true;
            handler.HandlerFunction();
        }

        /// <summary>
        /// This function returns from the interrupt routine
        /// </summary>
        /// <param name="returnAddress">the logical address to return back to</param>
        public override void Return(int returnAddress)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function executes an interrupt
        /// </summary>
        /// <param name="handle"> the interrupts handler</param>
        /// <returns> the return value of the interrupt or null if the interrupt did not return a value</returns>
        public override int? Execute(InterruptHandler handle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property for the logical address of the interrupt routine
        /// </summary>
        public override int LogicalAddress
        {
            get { return logicalAddress; }
        }

        /// <summary>
        /// Property For the interrupt ID
        /// </summary>
        public override int ID
        {
            get { return id; }
        }

        /// <summary>
        /// Property for the name of the routine
        /// </summary>
        public override string RoutineName
        {
            get { return routineName; }
        }

        /// <summary>
        /// Property for the interrupt handler
        /// </summary>
        public override InterruptHandler Handler
        {
            get { return handler; }
        }

        /// <summary>
        /// Property for the interrupt type
        /// </summary>
        public override EnumInterruptType InterruptType
        {
            get { interruptType = EnumInterruptType.VECTORED; return interruptType; }
        }
    }
}