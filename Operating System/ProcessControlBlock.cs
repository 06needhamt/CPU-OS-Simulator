using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System
{
    [Serializable]
    public class ProcessControlBlock
    {
        private int CPUID;
        private int OSID;
        private int processID;
        private string processName;
        private EnumProcessState processState;

        private string programName;
        private int baseAddress;
        private int startAddress;
        private int processPriority;
        private int proceessMemory;
        private double avgBurstTime;
        private double avgWaitingTime;

        private bool resourceStarved;
        private List<SystemResource> allocatedResources;
        private List<SystemResource> requestedResources;

        public ProcessControlBlock()
        {
            
        }

        public ProcessControlBlock(PCBFlags flags)
        {
            
        }

        public int Cpuid
        {
            get { return CPUID; }
            set { CPUID = value; }
        }

        public int Osid
        {
            get { return OSID; }
            set { OSID = value; }
        }

        public int ProcessId
        {
            get { return processID; }
            set { processID = value; }
        }

        public string ProcessName
        {
            get { return processName; }
            set { processName = value; }
        }

        public EnumProcessState ProcessState
        {
            get { return processState; }
            set { processState = value; }
        }

        public string ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }

        public int BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }

        public int StartAddress
        {
            get { return startAddress; }
            set { startAddress = value; }
        }

        public int ProcessPriority
        {
            get { return processPriority; }
            set { processPriority = value; }
        }

        public int ProceessMemory
        {
            get { return proceessMemory; }
            set { proceessMemory = value; }
        }

        public double AvgBurstTime
        {
            get { return avgBurstTime; }
            set { avgBurstTime = value; }
        }

        public double AvgWaitingTime
        {
            get { return avgWaitingTime; }
            set { avgWaitingTime = value; }
        }

        public bool ResourceStarved
        {
            get { return resourceStarved; }
            set { resourceStarved = value; }
        }

        public List<SystemResource> AllocatedResources
        {
            get { return allocatedResources; }
            set { allocatedResources = value; }
        }

        public List<SystemResource> RequestedResources
        {
            get { return requestedResources; }
            set { requestedResources = value; }
        }
    }
}
