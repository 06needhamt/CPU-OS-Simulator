using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;

namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This class represents the core of the operating system
    /// </summary>
    [Serializable]
    public class OSCore : INotifyCollectionChanged
    {
        private EnumSchedulingPolicies schedulingPolicy = EnumSchedulingPolicies.UNKNOWN;
        private double RR_Time_Slice;
        private EnumTimeUnit RR_Time_Slice_Unit = EnumTimeUnit.UNKNOWN;
        private EnumPriorityPolicy priorityPolicy = EnumPriorityPolicy.UNKNOWN;
        private EnumRoundRobinType roundRobinType = EnumRoundRobinType.UNKNOWN;
        private bool useDefaultScheduler;
        private bool useSingleCPU;
        private bool allowCPUAffinity;
        private bool runWithNoProcesses;
        private int CPUClockSpeed;
        private bool suspendOnStateChange_Ready;
        private bool suspendOnPreEmption;
        private bool suspendOnStateChange_Running;
        private bool suspendOnStateChange_Waiting;
        private bool forceKill;
        private bool faultKill;
        private EnumOSState osState = EnumOSState.UNKNOWN;
        private EnumErrorCodes errorCode = EnumErrorCodes.UNKNOWN;
        private Scheduler scheduler;
        private Queue<SimulatorProcess> readyQueue;
        private Queue<SimulatorProcess> waitingQueue;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        /// <summary>
        /// Default Constructor for the OS Core
        /// </summary>
        public OSCore()
        {
            
        }
        /// <summary>
        /// Constructor for OS Core that takes flags which control OS behaviour
        /// </summary>
        /// <param name="flags"> the flags to be passed to this operating system</param>
        public OSCore(OSFlags flags)
        {
            schedulingPolicy = flags.schedulingPolicy;
            RR_Time_Slice = flags.RR_Time_Slice;
            RR_Time_Slice_Unit = flags.RR_Time_Slice_Unit;
            priorityPolicy = flags.priorityPolicy;
            roundRobinType = flags.roundRobinType;
            useDefaultScheduler = flags.useDefaultScheduler;
            useSingleCPU = flags.useSingleCPU;
            allowCPUAffinity = flags.allowCPUAffinity;
            runWithNoProcesses = flags.runWithNoprocesses;
            CPUClockSpeed = flags.CPUClockSpeed;
            suspendOnStateChange_Ready = flags.suspendOnStateChange_Ready;
            suspendOnPreEmption = flags.suspendOnPreEmption;
            suspendOnStateChange_Running = flags.suspendOnStateChange_Running;
            suspendOnStateChange_Waiting = flags.suspendOnStateChange_Waiting;
            forceKill = flags.forceKill;
            faultKill = flags.faultKill;
            osState = flags.osState;
            scheduler = flags.scheduler;
            readyQueue = new Queue<SimulatorProcess>();
            waitingQueue = new Queue<SimulatorProcess>();
            CollectionChanged += OnCollectionChanged;

        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            System.Console.WriteLine("Queue contents have changed" + sender.ToString());
        }

        /// <summary>
        /// This function is called to start the operating system
        /// </summary>
        /// <returns> whether any errors occurred during execution</returns>
        public bool Start()
        {
            errorCode = EnumErrorCodes.NO_ERROR;
            if (scheduler == null)
            {
                SchedulerFlags? flags = CreateSchedulerFlags();
                if (flags == null)
                {
                    MessageBox.Show("Could not start scheduler invalid flags");
                    return false;
                }
                scheduler = new Scheduler(flags.Value);
            }
            SetClockSpeed();
            if (!scheduler.Start())
            {
                MessageBox.Show("An error occurred while running the scheduler");
                return false;
            }
            return true;
        }

        private void SetClockSpeed()
        {
            dynamic window = GetOSWindowInstance();
            foreach (SimulatorProcess process in window.Processes)
            {
                process.Unit.ClockSpeed = (int) window.sld_ClockSpeed.Value;
            }
        }

        /// <summary>
        /// This function creates a simulator process
        /// </summary>
        /// <param name="flags"> the flags to use to create the simulator process</param>
        /// <returns> the created simulator process</returns>
        public SimulatorProcess CreateProcess(ProcessFlags flags)
        {
            SimulatorProcess proc = new SimulatorProcess(flags);
            return proc;
        }
        /// <summary>
        /// this method creates flags for the operating system scheduler based on selected UI options
        /// </summary>
        /// <returns> a struct containing the selected options or null if an error occurred</returns>
        private SchedulerFlags? CreateSchedulerFlags()
        {
            SchedulerFlags temp = new SchedulerFlags();
            temp.schedulingPolicies = schedulingPolicy;
            if (temp.schedulingPolicies == EnumSchedulingPolicies.ROUND_ROBIN) // if the user selected round robin scheduling
            {
                temp.RR_Priority_Policy = priorityPolicy;
                temp.RR_TimeSlice = RR_Time_Slice;
                temp.RR_Type = roundRobinType;
                temp.TimeSliceUnit = RR_Time_Slice_Unit;
            }
            temp.allowCPUAffinity = allowCPUAffinity;
            temp.defaultScheduler = useDefaultScheduler;
            temp.runningWithNoProcesses = runWithNoProcesses;
            if (!temp.runningWithNoProcesses) // if the user selected to run the scheduler no processes
            {
                temp.readyQueue = readyQueue;
                temp.waitingQueue = waitingQueue;
                SimulatorProcess proc = readyQueue.Dequeue();
                if (proc != null)
                {
                    proc.ProcessState = EnumProcessState.RUNNING;
                }
                temp.runningProcess = proc; // populate the queues and set the first processes state to running
            }
            else  // otherwise create a blank queue and set the running process to null
            {
                readyQueue = new Queue<SimulatorProcess>();
                waitingQueue = new Queue<SimulatorProcess>();
                temp.runningProcess = null;
            }
            temp.cpuClockSpeed = CPUClockSpeed;
            return temp;
        }

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetOSWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("OperatingSystemMainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        /// <summary>
        /// Property for the length of the round robin time slice being used by this scheduler
        /// </summary>
        public double Rr_Time_Slice
        {
            get { return RR_Time_Slice; }
            set { RR_Time_Slice = value; }
        }
        /// <summary>
        /// Property for the unit of time used for time slices
        /// i.e ticks or seconds
        /// </summary>
        public EnumTimeUnit Rr_Time_Slice_Unit
        {
            get { return RR_Time_Slice_Unit; }
            set { RR_Time_Slice_Unit = value; }
        }
        /// <summary>
        /// Property for the Round Robin priority policy being used by this scheduler
        /// </summary>
        public EnumPriorityPolicy PriorityPolicy
        {
            get { return priorityPolicy; }
            set { priorityPolicy = value; }
        }
        /// <summary>
        /// Property for the type of round robin being used by the scheduler
        /// </summary>
        public EnumRoundRobinType RoundRobinType
        {
            get { return roundRobinType; }
            set { roundRobinType = value; }
        }
        /// <summary>
        /// Property for whether we are using the default scheduler
        /// </summary>
        public bool UseDefaultScheduler
        {
            get { return useDefaultScheduler; }
            set { useDefaultScheduler = value; }
        }
        /// <summary>
        /// Property for the clock speed of the CPU
        /// </summary>
        public int CpuClockSpeed
        {
            get { return CPUClockSpeed; }
            set { CPUClockSpeed = value; }
        }
        /// <summary>
        /// Property for whether the process should suspend when it enters the ready state
        /// </summary>
        public bool SuspendOnStateChange_Ready
        {
            get { return suspendOnStateChange_Ready; }
            set { suspendOnStateChange_Ready = value; }
        }
        /// <summary>
        /// Property for whether the process should suspend when it is pre-empted by another process
        /// </summary>
        public bool SuspendOnPreEmption
        {
            get { return suspendOnPreEmption; }
            set { suspendOnPreEmption = value; }
        }
        /// <summary>
        /// Property for whether the process should suspend when it enters the running state
        /// </summary>
        public bool SuspendOnStateChange_Running
        {
            get { return suspendOnStateChange_Running; }
            set { suspendOnStateChange_Running = value; }
        }
        /// <summary>
        /// Property for whether the process should suspend when it enters the waiting state
        /// </summary>
        public bool SuspendOnStateChange_Waiting
        {
            get { return suspendOnStateChange_Waiting; }
            set { suspendOnStateChange_Waiting = value; }
        }
        /// <summary>
        /// Property for the current state of the OS
        /// </summary>
        public EnumOSState OsState
        {
            get { return osState; }
            set { osState = value; }
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
        /// Property for whether the scheduler is running on a single CPU
        /// </summary>
        public bool UsingSingleCpu
        {
            get { return useSingleCPU; }
            set { useSingleCPU = value; }
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
            get { return runWithNoProcesses; }
            set { runWithNoProcesses = value; }
        }
        /// <summary>
        /// Property for an error code that describes any error that occurred
        /// </summary>
        public EnumErrorCodes ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        /// <summary>
        /// Property for the current scheduler
        /// </summary>
        public Scheduler Scheduler
        {
            get { return scheduler; }
            set { scheduler = value; }
        }

    }
}