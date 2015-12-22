using System.Collections.Generic;

namespace CPU_OS_Simulator.Operating_System
{
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

    }
}