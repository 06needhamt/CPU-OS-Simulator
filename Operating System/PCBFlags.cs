using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Operating_System
{
    #pragma warning disable 1591
    public struct PCBFlags
    {
        public int CPUID;
        public int OSID;
        public int processID;
        public string processName;
        public EnumProcessState processState;

        public string programName;
        public int baseAddress;
        public int startAddress;
        public int processPriority;
        public int proceessMemory;
        public double avgBurstTime;
        public double avgWaitingTime;

        public bool resourceStarved;
        public List<SystemResource> allocatedResources;
        public List<SystemResource> requestedResources;

        public Register[] registers;
        public SpecialRegister[] specialRegisters;

        public int lifetimeMills;

    }
}