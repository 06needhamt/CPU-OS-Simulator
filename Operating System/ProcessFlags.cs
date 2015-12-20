using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// this struct represents the flags passed to a process
    /// </summary>
    public struct ProcessFlags
    {
        public SimulatorProgram program;
        public string processName;
        public int processPriority;
        public int processMemory;
        public int processLifetime;
        public EnumTimeUnit processLifetimeTimeUnit;
        public int burstTime;
        public bool displayProfile;
        public bool parentDiesChildrenDie;
        public bool defaultScheduler;
        public bool delayedProcess;
        public int delayedProcessTime;
        public EnumTimeUnit delayTimeUnit;
        public SimulatorProcess parentProcess;
        public List<SimulatorProcess> childProcesses;
        public bool processSwapped;
        public EnumProcessState processState;
    }
}