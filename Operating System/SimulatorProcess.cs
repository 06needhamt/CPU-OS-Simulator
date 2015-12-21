using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This class represents a process that can be run on the virtual operating system
    /// </summary>
    [Serializable]
    public class SimulatorProcess : IComparable<int>
    {
        private SimulatorProgram program;
        private SimulatorProcess parentProcess;
        private List<SimulatorProcess> childProcesses;
        private string programName;
        private string processName;
        private string processStateString;
        private int processPriority;
        private int processMemory;
        private int processID;
        private int parentProcessID;
        private int processLifetime;
        private int burstTime;
        private bool processSwapped;
        private EnumProcessState processState = EnumProcessState.UNKNOWN;

        /// <summary>
        /// Default Constructor for a process used when deserialising processes
        /// NOTE: DO NOT USE IN CODE
        /// </summary>
        public SimulatorProcess()
        {

        }
        /// <summary>
        /// Constructor for a process 
        /// </summary>
        /// <param name="flags"> the flags to create this process with</param>
        public SimulatorProcess(ProcessFlags flags)
        {
            this.program = flags.program;
            this.ProcessName = flags.processName;
            this.processPriority = flags.processPriority;
            this.processMemory = flags.processMemory;
            this.processLifetime = flags.processLifetime;
            this.burstTime = flags.burstTime;
            this.parentProcess = flags.parentProcess;
            if (parentProcess != null)
            {
                parentProcessID = parentProcess.processID;
            }
            this.childProcesses = flags.childProcesses;
            this.processSwapped = flags.processSwapped;
        }

        /// <summary>
        /// Property for the program that this process represents
        /// </summary>
        public SimulatorProgram Program
        {
             get { return program; }
             set { program = value; }
        }
        /// <summary>
        /// Property for this process's parent
        /// </summary>
        public SimulatorProcess ParentProcess
        {
             get { return parentProcess; }
             set { parentProcess = value; }
        }
        /// <summary>
        /// Property for a list of this process's child processes
        /// </summary>
        public List<SimulatorProcess> ChildProcesses
        {
             get { return childProcesses; }
             set { childProcesses = value; }
        }
        /// <summary>
        /// Property for the name of the program that makes up this process
        /// </summary>
        public string ProgramName
        {
             get { return programName; }
             set { programName = value; }
        }
        /// <summary>
        /// Property for the name of this process
        /// </summary>
        public string ProcessName
        {
             get { return processName; }
             set { processName = value; }
        }
        /// <summary>
        /// Property for the string representation of this process's state
        /// </summary>
        public string ProcessStateString
        {
             get { return processStateString; }
             set { processStateString = value; }
        }
        /// <summary>
        /// Property for the priority of this process
        /// </summary>
        public int ProcessPriority
        {
             get { return processPriority; }
             set { processPriority = value; }
        }
        /// <summary>
        /// Property for the amount of memory pages this program has
        /// </summary>
        public int ProcessMemory
        {
             get { return processMemory; }
             set { processMemory = value; }
        }
        /// <summary>
        /// Property for the unique ID of this process
        /// </summary>
        public int ProcessId
        {
             get { return processID; }
             set { processID = value; }
        }
        /// <summary>
        /// Property for the unique ID of the parent process
        /// </summary>
        public int ParentProcessId
        {
             get { return parentProcessID; }
             set { parentProcessID = value; }
        }
        /// <summary>
        /// Property For the lifetime of this process
        /// </summary>
        public int ProcessLifetime
        {
             get { return processLifetime; }
             set { processLifetime = value; }
        }
        /// <summary>
        /// Property for the burst time of this process
        /// </summary>
        public int BurstTime
        {
             get { return burstTime; }
             set { burstTime = value; }
        }
        /// <summary>
        /// Property for whether this process is swapped out
        /// </summary>
        public bool ProcessSwapped
        {
             get { return processSwapped; }
             set { processSwapped = value; }
        }
        /// <summary>
        /// Property for the state of this process
        /// </summary>
        public EnumProcessState ProcessState
        {
             get { return processState; }
             set { processState = value; }
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(int other)
        {
            if (processPriority > other)
            {
                return -1;
            }
            else if (processPriority == other)
            {
                return 0;
            }
            else if (processPriority < other)
            {
                return 1;
            }
            return 0;
        }
    }
}