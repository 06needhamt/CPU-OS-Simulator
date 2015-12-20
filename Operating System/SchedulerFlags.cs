﻿using System.Collections.Generic;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This struct represents the flags passed to the scheduler
    /// </summary>
    public struct SchedulerFlags
    {
        public Queue<SimulatorProcess> readyQueue;
        public Queue<SimulatorProcess> waitingQueue;
        public SimulatorProcess runningProcess;
        public EnumSchedulingPolicies schedulingPolicies;
        public int RR_TimeSlice;
        public EnumTimeUnit TimeSliceUnit;
        public bool defaultScheduler;
        public EnumPriorityPolicy RR_Priority_Policy;
        public EnumRoundRobinType RR_Type;
        public bool usingSingleCPU;
        public bool allowCPUAffinity;
        public bool runningWithNoProcesses;
    }
}