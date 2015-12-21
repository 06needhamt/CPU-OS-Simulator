using System;
using System.Collections.Generic;
using System.Windows;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This class represents the process scheduler / manager for the virtual operating system
    /// </summary>
    [Serializable]
    public class Scheduler
    {
        private Queue<SimulatorProcess> readyQueue;
        private Queue<SimulatorProcess> waitingQueue;
        private SimulatorProcess runningProcess;
        private EnumSchedulingPolicies schedulingPolicy = EnumSchedulingPolicies.UNKNOWN;
        private double RR_TimeSlice;
        private EnumTimeUnit timeSliceUnit = EnumTimeUnit.UNKNOWN;
        private bool defaultScheduler;
        private EnumPriorityPolicy RR_Priority_Policy = EnumPriorityPolicy.UNKNOWN;
        private EnumRoundRobinType RR_Type = EnumRoundRobinType.UNKNOWN;
        private bool usingSingleCPU;
        private bool allowCPUAffinity;
        private bool runningWithNoProcesses;

        /// <summary>
        /// Default Constructor for scheduler used when deserialising the scheduler
        /// NOTE: DO NOT USE IN CODE
        /// </summary>
        public Scheduler()
        {
            
        }
        /// <summary>
        /// Constructor for process Scheduler
        /// </summary>
        /// <param name="flags"> the flags to use to create this scheduler</param>
        public Scheduler(SchedulerFlags flags)
        {
            readyQueue = flags.readyQueue ?? new Queue<SimulatorProcess>();
            waitingQueue = flags.waitingQueue ?? new Queue<SimulatorProcess>();
            runningProcess = flags.runningProcess;
            schedulingPolicy = flags.schedulingPolicies;
            RR_TimeSlice = flags.RR_TimeSlice;
            timeSliceUnit = flags.TimeSliceUnit;
            defaultScheduler = flags.defaultScheduler;
            RR_Priority_Policy = flags.RR_Priority_Policy;
            RR_Type = flags.RR_Type;
            usingSingleCPU = flags.usingSingleCPU;
            allowCPUAffinity = flags.allowCPUAffinity;
            runningWithNoProcesses = flags.runningWithNoProcesses;
        }

        public bool Start()
        {
            if (readyQueue.Count == 0 && !runningWithNoProcesses)
            {
                MessageBox.Show("OS Execution complete");
                return true;
            }
            bool finished = RunScheduler();
            return finished;
        }

        private bool RunScheduler()
        {
            while (readyQueue.Count > 0 && runningProcess == null)
            {
                switch (schedulingPolicy)
                {
                    case EnumSchedulingPolicies.FIRST_COME_FIRST_SERVED:
                        runningProcess = readyQueue.Dequeue();
                        

                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Property for the queue of processes that are ready to be executed
        /// </summary>
        public Queue<SimulatorProcess> ReadyQueue
        {
            get { return readyQueue; }
            set { readyQueue = value; }
        }
        /// <summary>
        /// Property for the queue of processes that are waiting to be serviced by the operating system
        /// </summary>
        public Queue<SimulatorProcess> WaitingQueue
        {
            get { return waitingQueue; }
            set { waitingQueue = value; }
        }
        /// <summary>
        /// Property for the currently running process
        /// </summary>
        public SimulatorProcess RunningProcess
        {
            get { return runningProcess; }
            set { runningProcess = value; }
        }
        /// <summary>
        /// Property for the scheduling policy currently being used by this scheduler 
        /// </summary>
        public EnumSchedulingPolicies SchedulingPolicy
        {
            get { return schedulingPolicy; }
            set { schedulingPolicy = value; }
        }
        /// <summary>
        /// Property for the length of the round robin time slice being used by this scheduler
        /// </summary>
        public double Rr_TimeSlice
        {
            get { return RR_TimeSlice; }
            set { RR_TimeSlice = value; }
        }
        /// <summary>
        /// Property for the unit of time used for time slices
        /// i.e ticks or seconds
        /// </summary>
        public EnumTimeUnit TimeSliceUnit
        {
            get { return timeSliceUnit; }
            set { timeSliceUnit = value; }
        }
        /// <summary>
        /// Property for whether we are using the default scheduler
        /// </summary>
        public bool DefaultScheduler
        {
            get { return defaultScheduler; }
            set { defaultScheduler = value; }
        }
        /// <summary>
        /// Property for the Round Robin priority policy being used by this scheduler
        /// </summary>
        public EnumPriorityPolicy Rr_Priority_Policy
        {
            get { return RR_Priority_Policy; }
            set { RR_Priority_Policy = value; }
        }
        /// <summary>
        /// Property for the type of round robin being used by the scheduler
        /// </summary>
        public EnumRoundRobinType Rr_Type
        {
            get { return RR_Type; }
            set { RR_Type = value; }
        }
        /// <summary>
        /// Property for whether the scheduler is running on a single CPU
        /// </summary>
        public bool UsingSingleCpu
        {
            get { return usingSingleCPU; }
            set { usingSingleCPU = value; }
        }
        /// <summary>
        /// Property for whether CPU affinity is allowed on this scheduler
        /// </summary>
        public bool AllowCpuAffinity
        {
            get { return allowCPUAffinity; }
            set { allowCPUAffinity = value; }
        }
        /// <summary>
        /// Property for whether the scheduler is running with no processes
        /// </summary>
        public bool RunningWithNoProcesses
        {
            get { return runningWithNoProcesses; }
            set { runningWithNoProcesses = value; }
        }
    }
}