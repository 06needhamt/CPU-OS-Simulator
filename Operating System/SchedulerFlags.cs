using System.Collections.Generic;

namespace CPU_OS_Simulator.Operating_System
{
    #pragma warning disable 1591
    /// <summary>
    /// This struct represents the flags passed to the scheduler
    /// </summary>
    public struct SchedulerFlags
    {
        public Queue<SimulatorProcess> readyQueue;
        public Queue<SimulatorProcess> waitingQueue;
        public SimulatorProcess runningProcess;
        public EnumSchedulingPolicies schedulingPolicies;
        public double RR_TimeSlice;
        public EnumTimeUnit TimeSliceUnit;
        public bool defaultScheduler;
        public EnumPriorityPolicy RR_Priority_Policy;
        public EnumRoundRobinType RR_Type;
        public bool usingSingleCPU;
        public bool allowCPUAffinity;
        public bool runningWithNoProcesses;
        public int cpuClockSpeed;
        public List<LotteryTicket> issuedLotteryTickets;
        public List<LotteryTicket> drawnLotteryTickets;
        public OSCore core;
    }
}