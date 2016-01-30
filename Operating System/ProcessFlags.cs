using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    #pragma warning disable 1591
    /// <summary>
    /// this struct represents the flags passed to a process
    /// </summary>
    public struct ProcessFlags
    {
        public SimulatorProgram program;
        public string programName;
        public string processName;
        public int processPriority;
        public int processMemory;
        public int processLifetime;
        public EnumTimeUnit processLifetimeTimeUnit;
        public int processID;
        public int CPUid;
        public int burstTime;
        public bool displayProfile;
        public bool parentDiesChildrenDie;
        public bool defaultScheduler;
        public bool delayedProcess;
        public int delayedProcessTime;
        public EnumTimeUnit delayTimeUnit;
        public SimulatorProcess parentProcess;
        public int parentProcessID;
        public List<SimulatorProcess> childProcesses;
        public bool processSwapped;
        public EnumProcessState processState;
        public EnumProcessState previousState;
        public bool resourceStarved;
        public List<SimulatorResource> allocatedResources;
        public List<SimulatorResource> requestedResources;
        public bool terminated;
        public ProcessControlBlock processControlBlock;
        public int OSid;
        public int clockSpeed;
        public ProcessExecutionUnit unit;
        public List<LotteryTicket> lotteryTickets;
        public bool ownsSemaphore;
        public bool waitingForSemaphore;

    }
}