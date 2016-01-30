using System;
using System.Collections.Generic;
using System.Linq;
using CPU_OS_Simulator.CPU;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.Operating_System
{
    
    public class ProcessControlBlock
    {
        private int CPUID;
        private int OSID;
        private int processID;
        private string processName;
        private EnumProcessState processState = EnumProcessState.UNKNOWN;

        private string programName;
        private int baseAddress;
        private int startAddress;
        private int processPriority;
        private int proceessMemory;
        private double avgBurstTime;
        private double avgWaitingTime;

        private bool resourceStarved;
        private List<SimulatorResource> allocatedResources;
        private List<SimulatorResource> requestedResources;
        private Register[] registers;
        private SpecialRegister[] specialRegisters;

        private int lifetimeMills;

        /// <summary>
        /// Default Constructor for process control block used when deserialising a process control block
        /// NOTE: DO NOT USE IN CODE:
        /// </summary>
        [JsonConstructor]
        public ProcessControlBlock()
        {
           
        }
        /// <summary>
        /// Constructor for process control block with construction flags
        /// </summary>
        /// <param name="flags"> flags to use to construct this PCB</param>
        public ProcessControlBlock(PCBFlags flags)
        {
            this.CPUID = flags.CPUID;
            this.OSID = flags.OSID;
            this.processID = flags.processID;
            this.ProcessName = flags.processName;
            this.processState = flags.processState;
            this.programName = flags.programName;
            this.baseAddress = flags.baseAddress;
            this.startAddress = flags.startAddress;
            this.processPriority = flags.processPriority;
            this.avgBurstTime = flags.avgBurstTime;
            this.avgWaitingTime = flags.avgWaitingTime;
            this.resourceStarved = flags.resourceStarved;
            if (flags.allocatedResources == null)
            {
                this.allocatedResources = new List<SimulatorResource>();
            }
            else
            {
                this.allocatedResources = flags.allocatedResources;
            }
            if (flags.requestedResources == null)
            {
                this.allocatedResources = new List<SimulatorResource>();
            }
            else
            {
                this.allocatedResources = flags.allocatedResources;
            }
            this.specialRegisters = flags.specialRegisters;
            this.registers = flags.registers;
            this.lifetimeMills = flags.lifetimeMills;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            string allocatedResourcesString = String.Empty;
            string requestedResourcesString = String.Empty;

            allocatedResourcesString = allocatedResources.Aggregate(allocatedResourcesString, (current, res) => current + (res.ResourceName + ", "));
            requestedResourcesString = requestedResources.Aggregate(requestedResourcesString, (current, res) => current + (res.ResourceName + ", "));

            string PCBString = "CPU ID \t\t\t\t\t : " + CPUID + "\n"
                               + "OS ID \t\t\t\t\t : " + OSID + "\n"
                               + "Process ID \t\t\t\t\t : " + processID + "\n"
                               + "Process Name \t\t\t\t\t : " + processName + "\n"
                               + "Process State \t\t\t\t\t : " + processState.ToString() + "\n\n"
                               + "Program Name \t\t\t\t\t : " + programName + "\n"
                               + "Base Address \t\t\t\t\t : " + baseAddress + "\n"
                               + "Start Address \t\t\t\t\t : " + startAddress + "\n"
                               + "Priority \t\t\t\t\t : " + processPriority + "\n"
                               + "Memory Size \t\t\t\t\t : " + proceessMemory + " pages " + "\n\n"
                               + "Avg. Burst Time \t\t\t\t\t : " + avgBurstTime + " ticks " + "\n"
                               + "Avg Waiting Time \t\t\t\t\t : " + avgWaitingTime + " ticks " + "\n\n"
                               + "Resource Starved  \t\t\t\t\t : " + resourceStarved + "\n"
                               + "Resources Allocated \t\t\t\t\t : " + allocatedResourcesString + "\n"
                               + "Resources Requested \t\t\t\t\t : " + requestedResourcesString + "\n"
                               + "\n";
            return PCBString;

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

        public Register[] Registers
        {
            get { return registers; }
            set { registers = value; }
        }

        public SpecialRegister[] SpecialRegisters
        {
            get { return specialRegisters; }
            set { specialRegisters = value; }
        }

        public int LifetimeMills
        {
            get { return lifetimeMills; }
            set { lifetimeMills = value; }
        }
    }
}
