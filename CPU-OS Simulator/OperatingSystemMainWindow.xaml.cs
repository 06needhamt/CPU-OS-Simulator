using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CPU_OS_Simulator.CPU;
using CPU_OS_Simulator.Operating_System;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for OperatingSystemMainWindow.xaml
    /// </summary>
    public partial class OperatingSystemMainWindow : Window , INotifyCollectionChanged
    {
        private MainWindow parent;
        private List<SimulatorProgram> programList;
        private OSCore osCore = null;
        private string missingFlag = String.Empty;
        private List<SimulatorProcess> processes;
        private OSFlags globalFlags;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        /// <summary>
        /// Variable to hold the current instance of this window so it can be accessed by other modules
        /// </summary>
        public static OperatingSystemMainWindow currentInstance;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Default constructor for OperatingSystemMainWindow
        /// </summary>
        public OperatingSystemMainWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetOSWindowInstance();
            processes = new List<SimulatorProcess>();
            CollectionChanged += OnCollectionChanged;
        }
        /// <summary>
        /// Constructor for operating system window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        /// </summary>
        /// <param name="parent">The window that is creating this window </param>
        public OperatingSystemMainWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetOSWindowInstance();
            processes = new List<SimulatorProcess>();
            CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            System.Console.WriteLine("Queue contents have changed" + sender.ToString());
            System.Console.WriteLine("Queue contents have changed" + sender.ToString());
        }

        public List<SimulatorProcess> Processes
        {
            get { return processes; }
            set { processes = value; }
        }

        public OSCore OsCore
        {
            get { return osCore; }
            set { osCore = value; }
        }
        /// <summary>
        /// This method is called when the close button is clicked
        /// </summary>
        /// <param name="sender"> the object that triggered this event</param>
        /// <param name="e"> the event args for this event</param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This method sets the current instance of OS window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetOSWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("OperatingSystemMainWindowInstance").SetValue(null, currentInstance);
        }

        private void OperatingSystemWindow_Loaded(object sender, RoutedEventArgs e)
        {
            programList = parent.ProgramList ?? new List<SimulatorProgram>();
            lst_Programs.Items.Clear();
            lst_Programs.ItemsSource = programList;

        }

        private void OperatingSystemWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetOSWindowInstance();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            
            if (osCore == null)
            {
                CreateOsCore();
            }
            if (!osCore.Start())
            {
                MessageBox.Show("An error occurred while executing the Operating System");
                return;
            }
        }
        /// <summary>
        /// This function creates the Operating System core
        /// </summary>
        private void CreateOsCore()
        {
            OSFlags? flags = CreateOsFlags(); // create the operating system flags from selected UI options 
            if (flags == null) // if the flags are invalid
            {
                MessageBox.Show("An error occurred while creating the operating system core : Invalid flags");
                return;
            }
            osCore = new OSCore(flags.Value); //create the operating system core
            globalFlags = flags.Value; // save the creation flags
        }

        /// <summary>
        /// This function creates the flags for creating the OS core based on selected UI options
        /// </summary>
        /// <returns> the selected OS flags or null if an error occurred</returns>
        private OSFlags? CreateOsFlags()
        {
            OSFlags temp = new OSFlags();
            if (rdb_Round_Robin.IsChecked != null && rdb_Round_Robin.IsChecked.Value) // if round robin is selected
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.ROUND_ROBIN;

                if (rdb_RR_Seconds.IsChecked != null && rdb_RR_Seconds.IsChecked.Value) // if the seconds time unit is selected
                {
                    temp.RR_Time_Slice_Unit = EnumTimeUnit.SECONDS;
                    temp.RR_Time_Slice = Double.Parse(cmb_RRSeconds.SelectedValue.ToString());
                }
                else if (rdb_RR_Ticks.IsChecked != null && rdb_RR_Ticks.IsChecked.Value) // if the ticks time unit is selected
                {
                    temp.RR_Time_Slice_Unit = EnumTimeUnit.TICKS;
                    temp.RR_Time_Slice = Double.Parse(cmb_RRTicks.SelectedValue.ToString());
                }

                else
                {
                    MessageBox.Show("Please Select a time unit"); // no time unit was selected
                    return null;
                }

                if (rdb_No_Priority.IsChecked != null && rdb_No_Priority.IsChecked.Value) // if the no priority option is selected
                {
                    temp.priorityPolicy = EnumPriorityPolicy.NO_PRIORITY;
                }
                else if (rdb_Non_Preemptive.IsChecked != null && rdb_Non_Preemptive.IsChecked.Value) // if the non pre-emptive priority option is selected
                {
                    temp.priorityPolicy = EnumPriorityPolicy.NON_PRE_EMPTIVE;
                }
                else if (rdb_Preemptive.IsChecked != null && rdb_Preemptive.IsChecked.Value) // if the pre-emptive priority option is selected
                {
                    temp.priorityPolicy = EnumPriorityPolicy.PRE_EMPTIVE;
                }
                else
                {
                    MessageBox.Show("Please Select a priority policy"); // if no priority option is selected
                    return null;
                }
                if (rdb_Static.IsChecked != null && rdb_Static.IsChecked.Value) // if the static option is selected
                {
                    temp.roundRobinType = EnumRoundRobinType.STATIC;
                }
                else if (rdb_Dynamic.IsChecked != null && rdb_Dynamic.IsChecked.Value) // if the dynamic option is selected
                {
                    temp.roundRobinType = EnumRoundRobinType.DYNAMIC;
                }
                else
                {
                    MessageBox.Show("Please Select a Round Robin Type"); // if no option was selected
                    return null;
                }

            }
            else if(rdb_FirstCome_FirstServed.IsChecked != null && rdb_FirstCome_FirstServed.IsChecked.Value) // if first come first served is selected
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.FIRST_COME_FIRST_SERVED;
            }
            else if (rdb_Shortest_Job_First.IsChecked != null && rdb_Shortest_Job_First.IsChecked.Value) // if shortest job first is selected
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.SHORTEST_JOB_FIRST;
            }
            else if (rdb_Lottery.IsChecked != null && rdb_Lottery.IsChecked.Value) // if lottery is selected
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.LOTTERY_SCHEDULING;
            }
            else if (rdb_FairShare.IsChecked != null && rdb_FairShare.IsChecked.Value) // if fair share is selected
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.FAIR_SHARE_SCEDULING;
            }
            else
            {
                MessageBox.Show("Please select a scheduling policy"); // if no policy is selected
                return null;
            }

            temp.CPUClockSpeed = (int)sld_ClockSpeed.Value; // clock speed
            if (chk_CPU_Affinity.IsChecked != null && chk_CPU_Affinity.IsChecked.Value) // is CPU affinity allowed?
            {
                temp.allowCPUAffinity = true;
            }
            else
            {
                temp.allowCPUAffinity = false;
            }
            temp.osState = EnumOSState.STOPPED; // the OS starts in the stopped state
            if (chk_No_Processes.IsChecked != null && chk_No_Processes.IsChecked.Value) // re we running with no processes?
            {
                temp.runWithNoprocesses = true;
            }
            else
            {
                temp.runWithNoprocesses = false;
            }
            if (chk_Suspend_On_Pre_emption.IsChecked != null && chk_Suspend_On_Pre_emption.IsChecked.Value) //Should processes suspend on pre-emption 
            {
                temp.suspendOnPreEmption = true;
            }
            else
            {
                temp.suspendOnPreEmption = false;
            }
            if (chk_Suspend_On_State_Change_Running.IsChecked != null &&
                chk_Suspend_On_State_Change_Running.IsChecked.Value) // should processes suspend when entering the running state
            {
                temp.suspendOnStateChange_Running = true;
            }
            else
            {
                temp.suspendOnStateChange_Running = false;
            }
            if (chk_Suspend_On_State_Change_Ready.IsChecked != null 
                && chk_Suspend_On_State_Change_Ready.IsChecked.Value) // should processes suspend when entering the ready state
            {
                temp.suspendOnStateChange_Ready = true;
            }
            else
            {
                temp.suspendOnStateChange_Ready = false;
            }
            if (chk_Suspend_On_State_Change_Waiting.IsChecked != null &&
                chk_Suspend_On_State_Change_Waiting.IsChecked.Value) // should processes suspend when entering the running state
            {
                temp.suspendOnStateChange_Waiting = true;
            }
            else
            {
                temp.suspendOnStateChange_Waiting = false;
            }
            if (chk_Use_Default_Scheduler.IsChecked != null && chk_Use_Default_Scheduler.IsChecked.Value) // are we using the default scheduler
            {
                temp.useDefaultScheduler = true;
            }
            else
            {
                temp.useDefaultScheduler = false;
            }
            if (chk_FaultKill.IsChecked != null && chk_FaultKill.IsChecked.Value) // should processes be killed upon having a page fault
            {
                temp.faultKill = true;
            }
            else
            {
                temp.faultKill = false;
            }
            if (chk_ForceKill.IsChecked != null && chk_ForceKill.IsChecked.Value) // should processes be force killed
            {
                temp.forceKill = true;
            }
            else
            {
                temp.forceKill = false;
            }
            SchedulerFlags? schedulerFlags = CreateSchedulerFlags(temp);
            if (schedulerFlags == null)
            {
                MessageBox.Show("An error occurred while creating the scheduler flags");
                return null;
            }
            temp.scheduler = new Scheduler(schedulerFlags.Value);  // create a scheduler
            return temp; // return the OS flags
        }
        /// <summary>
        /// This function creates scheduler flags
        /// </summary>
        /// <param name="flags"> the OS flags to use to create the scheduler flags</param>
        /// <returns> the created scheduler flags or null if an error occurred</returns>
        private SchedulerFlags? CreateSchedulerFlags(OSFlags flags)
        {
            SchedulerFlags temp = new SchedulerFlags();
            temp.schedulingPolicies = flags.schedulingPolicy;
            if (temp.schedulingPolicies == EnumSchedulingPolicies.ROUND_ROBIN) // if we are using round robin scheduling
            {
                temp.RR_Priority_Policy = flags.priorityPolicy;
                temp.RR_TimeSlice = flags.RR_Time_Slice;
                temp.RR_Type = flags.roundRobinType;
                temp.TimeSliceUnit = flags.RR_Time_Slice_Unit;
            }
            temp.allowCPUAffinity = flags.allowCPUAffinity;
            temp.defaultScheduler = flags.useDefaultScheduler;
            temp.runningWithNoProcesses = flags.runWithNoprocesses;
            temp.cpuClockSpeed = (int) sld_ClockSpeed.Value;
            return temp;
        }

        private async void btn_CreateNewProcess_Click(object sender, RoutedEventArgs e)
        {
            long delayMills = 0;
            if (chk_DelayedProcess.IsChecked != null && chk_DelayedProcess.IsChecked.Value)
            {
                if (rdb_DelaySecs.IsChecked != null && rdb_DelaySecs.IsChecked.Value)
                {
                    delayMills = ((Convert.ToInt64(cmb_Arrival_Delay.SelectedValue) * 1000));
                    await Delay(delayMills);
                }
                else if (rdb_DelayTicks.IsChecked != null && rdb_DelayTicks.IsChecked.Value)
                {
                    delayMills = (long)(Convert.ToInt64(cmb_Arrival_Delay.SelectedValue) * sld_ClockSpeed.Value);
                    await Delay(delayMills);
                }
                else
                {
                    MessageBox.Show("Invalid Delay Time Unit Selected");
                }
            }
            if (osCore == null)
            {
                CreateOsCore(); // try to create the OS Core
                if (osCore == null)
                {
                    MessageBox.Show("Failed To create OS Core");
                    return;
                }
                
            }
            // if the OS Core was successfully created

            ProcessFlags? processFlags = CreateProcessFlags(); // create the process flags
            if (processFlags == null)
            {
                MessageBox.Show("Failed To create process invalid flags");
                return;
            }
            SimulatorProcess proc = osCore.CreateProcess(processFlags.Value); // create the process
            processes.Add(proc); // add the process to the OS
            if (osCore.Scheduler == null) // if the scheduler is null try to create it
            {
                SchedulerFlags? schedulerFlags = CreateSchedulerFlags(globalFlags); // create scheduler flags
                if (schedulerFlags != null) // if the flags are valid
                    osCore.Scheduler = new Scheduler(schedulerFlags.Value); // create a new Scheduler
                else
                    MessageBox.Show("Scheduler flags are invalid");

                if (osCore.Scheduler == null)
                {
                    MessageBox.Show("Scheduler is NULL");
                    return;
                }
            }
            osCore.Scheduler.ReadyQueue.Enqueue(proc); // add the process to the ready queue
            await dispatcher.InvokeAsync(UpdateInterface);
        }

        private async Task<long> Delay(long delayMills)
        {
            Thread DelayThread = new Thread(delegate(object o)
            {
                long i = 0; 
                Stopwatch DelayStopwatch = new Stopwatch();
                DelayStopwatch.Start();
                while (DelayStopwatch.ElapsedMilliseconds < delayMills)
                {
                    i++;
                }
                DelayStopwatch.Reset();
            });
            DelayThread.Start(null);
            return delayMills;
        }

        private async void UpdateInterface()
        {
            lst_ReadyProcesses.ItemsSource = null;
            lst_RunningProcesses.ItemsSource = null;
            lst_WaitingProcesses.ItemsSource = null;
            lst_ReadyProcesses.Items.Clear();
            lst_RunningProcesses.Items.Clear();
            lst_WaitingProcesses.Items.Clear();
            List<SimulatorProcess> runningProcesses = new List<SimulatorProcess>();
            runningProcesses.Add(osCore.Scheduler.RunningProcess);
            lst_RunningProcesses.ItemsSource = runningProcesses;
            lst_ReadyProcesses.ItemsSource = osCore.Scheduler.ReadyQueue;
            lst_WaitingProcesses.ItemsSource = osCore.Scheduler.WaitingQueue;
            txtProcessName.Text = "P" + Convert.ToString(processes.Count + 1);
        }

        /// <summary>
        /// This function creates the process flags
        /// </summary>
        /// <returns> the created process flags ore null if an error occurred</returns>
        private ProcessFlags? CreateProcessFlags()
        {
            ProcessFlags temp = new ProcessFlags();
            temp.program = (SimulatorProgram) lst_Programs.SelectedItem;
            if (temp.program == null)
            {
                MessageBox.Show("Please select a program before trying to create a process");
                return null;
            }
            temp.programName = temp.program.Name;
            temp.processName = txtProcessName.Text;
            //ComboBoxItem priority = (ComboBoxItem)cmb_Priority.SelectedItem;
            temp.processPriority = (int) cmb_Priority.SelectedItem;
           // ComboBoxItem memory = (ComboBoxItem) cmb_Pages.SelectedItem;
            temp.processMemory = (int) cmb_Pages.SelectedItem;
            if (Int32.Parse(txt_ProcessLifetime.Text) == 0) // if the lifetime was 0
            {
                temp.processLifetime = Int32.MaxValue; // make it INT.MAXVALUE seconds
                temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
            }
            else // if the lifetime was non 0
            {
                temp.processLifetime = Int32.Parse(txt_ProcessLifetime.Text); // set the lifetime to the in-putted value
                if (rdb_LifetimeSecs.IsChecked != null && rdb_LifetimeSecs.IsChecked.Value) // if the seconds unit was selected  
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
                }
                else if (rdb_LifetimeTicks.IsChecked != null && rdb_LifetimeTicks.IsChecked.Value) //if the ticks time unit was selected
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a Process Lifetime Time Unit"); // if no time unit is selected
                    return null;
                }
            }
            temp.processID = processes.Count;
            temp.CPUid = 0;
            temp.burstTime = 0;
            if (chk_Display_Profile.IsChecked != null && chk_Display_Profile.IsChecked.Value) // if we are displaying a performance profile
            {
                temp.displayProfile = true;
            }
            else
            {
                temp.displayProfile = false;
            }
            if (chk_Children_Die.IsChecked != null && chk_Children_Die.IsChecked.Value) // should the processes children die when it dies
            {
                temp.parentDiesChildrenDie = true;
            }
            else
            {
                temp.parentDiesChildrenDie = false;
            }
            if (chk_Use_Default_Scheduler_Process.IsChecked != null
               && chk_Use_Default_Scheduler_Process.IsChecked.Value) // should this process use the default scheduler
            {
                temp.defaultScheduler = true;
            }
            else
            {
                temp.defaultScheduler = false;
            }
            if (chk_DelayedProcess.IsChecked != null && chk_DelayedProcess.IsChecked.Value) // does this process have a delayed start 
            {
                temp.delayedProcess = true;
                temp.delayedProcessTime = (int)cmb_Arrival_Delay.SelectedValue;
                if (rdb_DelaySecs.IsChecked != null && rdb_DelaySecs.IsChecked.Value) // if the seconds time unit was selected
                {
                    temp.delayTimeUnit = EnumTimeUnit.SECONDS;
                }
                else if (rdb_DelayTicks.IsChecked != null && rdb_DelayTicks.IsChecked.Value) // if the ticks time unit was selected
                {
                    temp.delayTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a delay time unit"); // if no time unit was selected
                    return null;
                }
            }
            temp.parentProcess = null;
            if (temp.parentProcess == null)
            {
                temp.parentProcessID = -1;
            }
            else
            {
                temp.parentProcessID = temp.parentProcess.ProcessID;
            }
            
            temp.childProcesses = new List<SimulatorProcess>();
            temp.processSwapped = false;
            temp.processState = EnumProcessState.READY;
            temp.resourceStarved = false;
            temp.allocatedResources = new List<SystemResource>();
            temp.requestedResources = new List<SystemResource>();
            temp.terminated = false;
            temp.processControlBlock = null;
            temp.OSid = 0;
            temp.unit = null;
            temp.clockSpeed = (int) sld_ClockSpeed.Value;
            return temp;
            #region OLD
            //temp.burstTime = 0;
            //temp.childProcesses = new List<SimulatorProcess>();
            //if (chk_Use_Default_Scheduler_Process.IsChecked != null
            //    && chk_Use_Default_Scheduler_Process.IsChecked.Value) // are we using the default scheduler
            //{
            //    temp.defaultScheduler = true;
            //}
            //else
            //{
            //    temp.defaultScheduler = false;
            //}
            //if (chk_DelayedProcess.IsChecked != null && chk_DelayedProcess.IsChecked.Value) // does this process have a delayed start 
            //{
            //    temp.delayedProcess = true;
            //    temp.delayedProcessTime = (int)cmb_Arrival_Delay.SelectedValue;
            //    if (rdb_DelaySecs.IsChecked != null && rdb_DelaySecs.IsChecked.Value) // if the seconds time unit was selected
            //    {
            //        temp.delayTimeUnit = EnumTimeUnit.SECONDS;
            //    }
            //    else if (rdb_DelayTicks.IsChecked != null && rdb_DelayTicks.IsChecked.Value) // if the ticks time unit was selected
            //    {
            //        temp.delayTimeUnit = EnumTimeUnit.TICKS;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please Select a delay time unit"); // if no time unit was selected
            //        return null;
            //    }
            //}
            //if (chk_Display_Profile.IsChecked != null && chk_Display_Profile.IsChecked.Value) // if we are displaying a performance profile
            //{
            //    temp.displayProfile = true;
            //}
            //else
            //{
            //    temp.displayProfile = false;
            //}
            //if (chk_Children_Die.IsChecked != null && chk_Children_Die.IsChecked.Value) // should the processes children die when it dies
            //{
            //    temp.parentDiesChildrenDie = true;
            //}
            //else
            //{
            //    temp.parentDiesChildrenDie = false;
            //}
            //temp.parentProcess = null;
            //if (Int32.Parse(txt_ProcessLifetime.Text) == 0) // if the lifetime was 0
            //{
            //    temp.processLifetime = Int32.MaxValue; // make it INT.MAXVALUE seconds
            //    temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
            //}
            //else // if the lifetime was non 0
            //{
            //    temp.processLifetime = Int32.Parse(txt_ProcessLifetime.Text); // set the lifetime to the in-putted value
            //    if (rdb_LifetimeSecs.IsChecked != null && rdb_LifetimeSecs.IsChecked.Value) // if the seconds unit was selected  
            //    {
            //        temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
            //    }
            //    else if (rdb_LifetimeTicks.IsChecked != null && rdb_LifetimeTicks.IsChecked.Value) //if the ticks time unit was selected
            //    {
            //        temp.processLifetimeTimeUnit = EnumTimeUnit.TICKS;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please Select a Process Lifetime Time Unit"); // if no time unit is selected
            //        return null;
            //    }
            //}
            //temp.processMemory = (int)cmb_Pages.SelectedValue;
            //temp.processName = txtProcessName.Text;
            //temp.processPriority = (int)cmb_Priority.SelectedValue;
            //temp.processState = EnumProcessState.READY;
            //temp.processSwapped = false;
            //temp.processID = processes.Count;
            //if (chk_Use_Default_Scheduler_Process.IsChecked != null
            //    && chk_Use_Default_Scheduler_Process.IsChecked.Value) // should this process use the default scheduler
            //{
            //    temp.defaultScheduler = true;
            //}
            //else
            //{
            //    temp.defaultScheduler = false;
            //}
            //return temp; // return the created flags
#endregion
        }

        private PCBFlags? CreatePCBFlags()
        {
            PCBFlags flags = new PCBFlags();
            return flags;
        }
    }
}
