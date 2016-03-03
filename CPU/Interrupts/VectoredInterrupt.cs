using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Interrupts
{
    [Serializable]
    public class VectoredInterrupt : Interrupt
    {
        private int logicalAddress;
        private string routineName;
        private InterruptHandler handler;
        private int id;
        private EnumInterruptType interruptType;
        private bool handled = false;

        public VectoredInterrupt()
        {
            
        }

        public VectoredInterrupt(int logicalAddress)
        {
            this.logicalAddress = logicalAddress;
        }

        public VectoredInterrupt(int logicalAddress, string routineName)
        {
            this.logicalAddress = logicalAddress;
            this.routineName = routineName;
        }

        public VectoredInterrupt(InterruptHandler handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// This function fires the interrupt
        /// </summary>
        public override void Fire()
        {
            handled = true;
            if (handler.HandlerFunction != null)
            {
                handler.HandlerFunction();
            }
            else
            {
                SimulatorProgram prog = GetCurrentProgram();
                ExecutionUnit unit = GetExecutionUnit();
                dynamic procunit = GetCurrentProcessExecutionUnit();
                if (procunit == null)
                {
                    unit = GetExecutionUnit();
                    unit.LogicalAddress = handler.LogicalAddress;
                    unit.CurrentIndex = handler.LogicalAddress/4;
                    unit.Done = false;
                    unit.Stop = false;
                }
                else
                {
                    procunit.LogicalAddress = handler.LogicalAddress;
                    procunit.CurrentIndex = handler.LogicalAddress/4;
                    procunit.Stop = false;
                    procunit.Done = false;
                }
            }
        }

        /// <summary>
        /// This function returns from the interrupt routine
        /// </summary>
        /// <param name="returnAddress">the logical address to return back to</param>
        public override void Return(int returnAddress)
        {
            handled = true;
            if (handler.HandlerFunction != null)
            {
                handler.HandlerFunction();
            }
            else
            {
                SimulatorProgram prog = GetCurrentProgram();
                ExecutionUnit unit = GetExecutionUnit();
                dynamic procunit = GetCurrentProcessExecutionUnit();
                if (procunit == null)
                {
                    unit = GetExecutionUnit();
                    unit.LogicalAddress = returnAddress;
                    unit.CurrentIndex = returnAddress / 4;
                    unit.Done = false;
                    unit.Stop = false;
                }
                else
                {
                    procunit.LogicalAddress = returnAddress;
                    procunit.CurrentIndex = returnAddress / 4;
                    procunit.Stop = false;
                    procunit.Done = false;
                }
            }
        }

        /// <summary>
        /// This function executes an interrupt
        /// </summary>
        /// <param name="handle"> the interrupts handler</param>
        /// <returns> the return value of the interrupt or null if the interrupt did not return a value</returns>
        public override int? Execute(InterruptHandler handle)
        {
            throw new NotSupportedException("Only Polled Interrupts can be Executed call Fire() instead");
        }

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

        public bool Handled
        {
            get { return handled; }
            set { handled = value; }
        }
    }
}
