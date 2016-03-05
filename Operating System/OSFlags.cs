
using CPU_OS_Simulator.CPU.Interrupts;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This struct represents the flags passed to the operating system
    /// </summary>
    public struct OSFlags
    {
        #pragma warning disable 1591
        public EnumSchedulingPolicies schedulingPolicy;
        public double RR_Time_Slice;
        public EnumTimeUnit RR_Time_Slice_Unit;
        public EnumPriorityPolicy priorityPolicy;
        public EnumRoundRobinType roundRobinType;
        public bool useDefaultScheduler;
        public bool useSingleCPU;
        public bool allowCPUAffinity;
        public bool runWithNoprocesses;
        public int CPUClockSpeed;
        public bool suspendOnStateChange_Ready;
        public bool suspendOnPreEmption;
        public bool suspendOnStateChange_Running;
        public bool suspendOnStateChange_Waiting;
        public bool forceKill;
        public bool faultKill;
        public EnumOSState osState;
        public Scheduler scheduler;
        public InterruptHandles interruptHandles;
    }
}