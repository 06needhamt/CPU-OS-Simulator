﻿using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using CPU_OS_Simulator.CPU;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This class represents a process that can be run on the virtual operating system
    /// </summary>
    [Serializable]
    public class SimulatorProcess : IComparable<Int32>
    {
        private SimulatorProgram program;
        private string programName;
        private string processName;
        private int processPriority;
        private int processMemory;
        private int processLifetime;
        private EnumTimeUnit processLifetimeTimeUnit = EnumTimeUnit.UNKNOWN;
        private int processID;
        private int parentProcessID;
        private int CPUid;
        private int OSid;
        private int burstTime;
        private bool displayProfile;
        private bool parentDiesChildrenDie;
        private bool defaultScheduler;
        private bool delayedProcess;
        private int delayedProcessTime;
        private EnumTimeUnit delayTimeUnit = EnumTimeUnit.UNKNOWN;
        private SimulatorProcess parentProcess;
        private List<SimulatorProcess> childProcesses;
        private bool processSwapped;
        private EnumProcessState currentState = EnumProcessState.UNKNOWN;
        private EnumProcessState previousState = EnumProcessState.UNKNOWN;
        private bool resourceStarved;
        private List<SimulatorResource> allocatedResources;
        private List<SimulatorResource> requestedResources;
        private bool terminated;
        private ProcessControlBlock processControlBlock;
        private int clockSpeed;
        private ProcessExecutionUnit unit;
        private List<LotteryTicket> lotteryTickets;
        private bool ownsSemaphore;
        private bool waitingForSemaphore;

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
            this.programName = flags.programName;
            this.processName = flags.processName;
            this.processPriority = flags.processPriority;
            this.processMemory = flags.processMemory;
            this.processLifetime = flags.processLifetime;
            this.processID = flags.processID;
            this.CPUID = flags.CPUid;
            this.burstTime = flags.burstTime;
            this.displayProfile = flags.displayProfile;
            this.parentDiesChildrenDie = flags.parentDiesChildrenDie;
            this.delayedProcess = flags.delayedProcess;
            this.delayedProcessTime = flags.delayedProcessTime;
            this.delayTimeUnit = flags.delayTimeUnit;
            this.parentProcess = flags.parentProcess;
            if (parentProcess != null)
            {
                parentProcessID = parentProcess.processID;
            }
            else
            {
                parentProcessID = -1;
            }
            this.childProcesses = flags.childProcesses;
            this.processSwapped = flags.processSwapped;
            this.currentState = flags.processState;
            this.previousState = flags.previousState;
            this.resourceStarved = flags.resourceStarved;
            this.allocatedResources = flags.allocatedResources;
            this.requestedResources = flags.requestedResources;
            this.processControlBlock = flags.processControlBlock;
            this.OSid = flags.OSid;
            this.clockSpeed = flags.clockSpeed;
            this.unit = new ProcessExecutionUnit(this,clockSpeed);
            this.lotteryTickets = flags.lotteryTickets;
            this.ownsSemaphore = flags.ownsSemaphore;
            this.waitingForSemaphore = flags.waitingForSemaphore;
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
        public int ProcessID
        {
             get { return processID; }
             set { processID = value; }
        }
        /// <summary>
        /// Property for the unique ID of the parent process
        /// </summary>
        public int ParentProcessID
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
        public EnumProcessState CurrentState
        {
             get { return currentState; }
             set { currentState = value; }
        }

        public int CPUID
        {
            get { return CPUid; }
            set { CPUid = value; }
        }

        public bool ResourceStarved
        {
            get
            {   
                return isResourceStarved();
            }
            set
            {
                resourceStarved = value;
                //resourceStarved = isResourceStarved();
            }
        }

        public List<SimulatorResource> AllocatedResources
        {
            get { return allocatedResources; }
            set { allocatedResources = value; }
        }

        public List<SimulatorResource> RequestedResources
        {
            get { return requestedResources; }
            set { requestedResources = value; }
        }

        public bool Terminated
        {
            get { return terminated; }
            set { terminated = value; }
        }

        public ProcessControlBlock ControlBlock
        {
            get { return processControlBlock; }
            set { processControlBlock = value; }
        }

        public int OSID
        {
            get { return OSid; }
            set { OSid = value; }
        }

        public int ClockSpeed
        {
            get { return clockSpeed; }
            set { clockSpeed = value; }
        }

        public ProcessExecutionUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public EnumTimeUnit ProcessLifetimeTimeUnit
        {
            get { return processLifetimeTimeUnit; }
            set { processLifetimeTimeUnit = value; }
        }

        public List<LotteryTicket> LotteryTickets
        {
            get { return lotteryTickets; }
            set { lotteryTickets = value; }
        }

        public bool OwnsSemaphore
        {
            get { return ownsSemaphore; }
            set { ownsSemaphore = value; }
        }

        public bool WaitingForSemaphore
        {
            get { return waitingForSemaphore; }
            set { waitingForSemaphore = value; }
        }

        public EnumProcessState PreviousState
        {
            get { return previousState; }
            set { previousState = value; }
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

        public bool isResourceStarved()
        {
            if (resourceStarved)
            {
                currentState = EnumProcessState.WAITING;
                return true;
            }
            return false;
        }

        public void Terminate()
        {
            terminated = true;
        }
    }
}