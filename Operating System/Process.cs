using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    [Serializable]
    public class Process
    {
        private SimulatorProgram program;
        private Process parentProcess;
        private List<Process> childProcesses;
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

        public SimulatorProgram Program
        {
            [DebuggerStepThrough] get { return program; }
            [DebuggerStepThrough] set { program = value; }
        }

        public Process ParentProcess
        {
            [DebuggerStepThrough] get { return parentProcess; }
            [DebuggerStepThrough] set { parentProcess = value; }
        }

        public List<Process> ChildProcesses
        {
            [DebuggerStepThrough] get { return childProcesses; }
            [DebuggerStepThrough] set { childProcesses = value; }
        }

        public string ProgramName
        {
            [DebuggerStepThrough] get { return programName; }
            [DebuggerStepThrough] set { programName = value; }
        }

        public string ProcessName
        {
            [DebuggerStepThrough] get { return processName; }
            [DebuggerStepThrough] set { processName = value; }
        }

        public string ProcessStateString
        {
            [DebuggerStepThrough] get { return processStateString; }
            [DebuggerStepThrough] set { processStateString = value; }
        }

        public int ProcessPriority
        {
            [DebuggerStepThrough] get { return processPriority; }
            [DebuggerStepThrough] set { processPriority = value; }
        }

        public int ProcessMemory
        {
            [DebuggerStepThrough] get { return processMemory; }
            [DebuggerStepThrough] set { processMemory = value; }
        }

        public int ProcessId
        {
            [DebuggerStepThrough] get { return processID; }
            [DebuggerStepThrough] set { processID = value; }
        }

        public int ParentProcessId
        {
            [DebuggerStepThrough] get { return parentProcessID; }
            [DebuggerStepThrough] set { parentProcessID = value; }
        }

        public int ProcessLifetime
        {
            [DebuggerStepThrough] get { return processLifetime; }
            [DebuggerStepThrough] set { processLifetime = value; }
        }

        public int BurstTime
        {
            [DebuggerStepThrough] get { return burstTime; }
            [DebuggerStepThrough] set { burstTime = value; }
        }

        public bool ProcessSwapped
        {
            [DebuggerStepThrough] get { return processSwapped; }
            [DebuggerStepThrough] set { processSwapped = value; }
        }

        public EnumProcessState ProcessState
        {
            [DebuggerStepThrough] get { return processState; }
            [DebuggerStepThrough] set { processState = value; }
        }
        /// <summary>
        /// Default Constructor for a process used when deserialising processes
        /// NOTE: DO NOT USE IN CODE
        /// </summary>
        public Process()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="processName"></param>
        /// <param name="processPriority"></param>
        /// <param name="processMemory"></param>
        /// <param name="processLifetime"></param>
        /// <param name="burstTime"></param>
        /// <param name="parentProcess"></param>
        /// <param name="childProcesses"></param>
        /// <param name="processSwapped"></param>
        public Process(SimulatorProgram program, string processName, int processPriority, int processMemory,
            int processLifetime, int burstTime, Process parentProcess = null, List<Process> childProcesses = null,
            bool processSwapped = false)
        {
            
        }

    }
}