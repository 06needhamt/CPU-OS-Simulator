using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CPU_OS_Simulator.CPU;
using CPU_OS_Simulator.Operating_System;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for OperatingSystemMainWindow.xaml
    /// </summary>
    public partial class OperatingSystemMainWindow : Window
    {
        private MainWindow parent;
        private List<SimulatorProgram> programList;
        private OSCore osCore = null;
        private string missingFlag = String.Empty;
        private List<SimulatorProcess> processes;
        private OSFlags globalFlags;
        /// <summary>
        /// Variable to hold the current instance of this window so it can be accessed by other modules
        /// </summary>
        public static OperatingSystemMainWindow currentInstance;
        /// <summary>
        /// Default constructor for OperatingSystemMainWindow
        /// </summary>
        public OperatingSystemMainWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetOSWindowInstance();
            processes = new List<SimulatorProcess>();
        }
        /// <summary>
        /// Constructor for operating system window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        /// </summary>
        /// <param name="parentWindow">The window that is creating this window </param>
        public OperatingSystemMainWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetOSWindowInstance();
            processes = new List<SimulatorProcess>();
        }

        public List<SimulatorProcess> Processes
        {
            get { return processes; }
            set { processes = value; }
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
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
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

            }
        }

        private void CreateOsCore()
        {
            OSFlags? flags = CreateOsFlags();
            if (flags == null)
            {
                return;
            }
            osCore = new OSCore(flags.Value);
            globalFlags = flags.Value;
        }

        private OSFlags? CreateOsFlags()
        {
            OSFlags temp = new OSFlags();
            if (rdb_Round_Robin.IsChecked != null && rdb_Round_Robin.IsChecked.Value)
            {
                if (rdb_RR_Seconds.IsChecked != null && rdb_RR_Seconds.IsChecked.Value)
                {
                    temp.RR_Time_Slice_Unit = EnumTimeUnit.SECONDS;
                    temp.RR_Time_Slice = Double.Parse(cmb_RRSeconds.SelectedValue.ToString());
                }
                else if (rdb_RR_Ticks.IsChecked != null && rdb_RR_Ticks.IsChecked.Value)
                {
                    temp.RR_Time_Slice_Unit = EnumTimeUnit.TICKS;
                    temp.RR_Time_Slice = Double.Parse(cmb_RRTicks.SelectedValue.ToString());
                }

                else
                {
                    MessageBox.Show("Please Select a time unit");
                    return null;
                }

                if (rdb_No_Priority.IsChecked != null && rdb_No_Priority.IsChecked.Value)
                {
                    temp.priorityPolicy = EnumPriorityPolicy.NO_PRIORITY;
                }
                else if (rdb_Non_Preemptive.IsChecked != null && rdb_Non_Preemptive.IsChecked.Value)
                {
                    temp.priorityPolicy = EnumPriorityPolicy.NON_PRE_EMPTIVE;
                }
                else if (rdb_Preemptive.IsChecked != null && rdb_Preemptive.IsChecked.Value)
                {
                    temp.priorityPolicy = EnumPriorityPolicy.PRE_EMPTIVE;
                }
                else
                {
                    MessageBox.Show("Please Select a priority policy");
                    return null;
                }
                if (rdb_Static.IsChecked != null && rdb_Static.IsChecked.Value)
                {
                    temp.roundRobinType = EnumRoundRobinType.STATIC;
                }
                else if (rdb_Dynamic.IsChecked != null && rdb_Dynamic.IsChecked.Value)
                {
                    temp.roundRobinType = EnumRoundRobinType.DYNAMIC;
                }
                else
                {
                    MessageBox.Show("Please Select a Round Robin Type");
                    return null;
                }

            }
            else if(rdb_FirstCome_FirstServed.IsChecked != null && rdb_FirstCome_FirstServed.IsChecked.Value)
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.FIRST_COME_FIRST_SERVED;
            }
            else if (rdb_Shortest_Job_First.IsChecked != null && rdb_Shortest_Job_First.IsChecked.Value)
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.SHORTEST_JOB_FIRST;
            }
            else if (rdb_Lottery.IsChecked != null && rdb_Lottery.IsChecked.Value)
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.LOTTERY_SCHEDULING;
            }
            else if (rdb_FairShare.IsChecked != null && rdb_FairShare.IsChecked.Value)
            {
                temp.schedulingPolicy = EnumSchedulingPolicies.FAIR_SHARE_SCEDULING;
            }
            else
            {
                MessageBox.Show("Please select a scheduling policy");
                return null;
            }

            temp.CPUClockSpeed = (int)sld_ClockSpeed.Value;
            if (chk_CPU_Affinity.IsChecked != null && chk_CPU_Affinity.IsChecked.Value)
            {
                temp.allowCPUAffinity = true;
            }
            else
            {
                temp.allowCPUAffinity = false;
            }
            temp.osState = EnumOSState.STOPPED;
            if (chk_No_Processes.IsChecked != null && chk_No_Processes.IsChecked.Value)
            {
                temp.runWithNoprocesses = true;
            }
            else
            {
                temp.runWithNoprocesses = false;
            }
            if (chk_Suspend_On_Pre_emption.IsChecked != null && chk_Suspend_On_Pre_emption.IsChecked.Value)
            {
                temp.suspendOnPreEmption = true;
            }
            else
            {
                temp.suspendOnPreEmption = false;
            }
            if (chk_Suspend_On_State_Change_Running.IsChecked != null &&
                chk_Suspend_On_State_Change_Running.IsChecked.Value)
            {
                temp.suspendOnStateChange_Running = true;
            }
            else
            {
                temp.suspendOnStateChange_Running = false;
            }
            if (chk_Suspend_On_State_Change_Ready.IsChecked != null && chk_Suspend_On_State_Change_Ready.IsChecked.Value)
            {
                temp.suspendOnStateChange_Ready = true;
            }
            else
            {
                temp.suspendOnStateChange_Ready = false;
            }
            if (chk_Suspend_On_State_Change_Waiting.IsChecked != null &&
                chk_Suspend_On_State_Change_Waiting.IsChecked.Value)
            {
                temp.suspendOnStateChange_Waiting = true;
            }
            else
            {
                temp.suspendOnStateChange_Waiting = false;
            }
            if (chk_Use_Default_Scheduler.IsChecked != null && chk_Use_Default_Scheduler.IsChecked.Value)
            {
                temp.useDefaultScheduler = true;
            }
            else
            {
                temp.useDefaultScheduler = false;
            }
            if (chk_FaultKill.IsChecked != null && chk_FaultKill.IsChecked.Value)
            {
                temp.faultKill = true;
            }
            else
            {
                temp.faultKill = false;
            }
            if (chk_ForceKill.IsChecked != null && chk_ForceKill.IsChecked.Value)
            {
                temp.forceKill = true;
            }
            else
            {
                temp.forceKill = false;
            }
            SchedulerFlags schedulerFlags = CreateSchedulerFlags(temp).Value;
            temp.scheduler = new Scheduler();
            return temp;
        }
        private SchedulerFlags? CreateSchedulerFlags(OSFlags flags)
        {
            SchedulerFlags temp = new SchedulerFlags();
            temp.schedulingPolicies = flags.schedulingPolicy;
            if (temp.schedulingPolicies == EnumSchedulingPolicies.ROUND_ROBIN)
            {
                temp.RR_Priority_Policy = flags.priorityPolicy;
                temp.RR_TimeSlice = flags.RR_Time_Slice;
                temp.RR_Type = flags.roundRobinType;
                temp.TimeSliceUnit = flags.RR_Time_Slice_Unit;
            }
            temp.allowCPUAffinity = flags.allowCPUAffinity;
            temp.defaultScheduler = flags.useDefaultScheduler;
            temp.runningWithNoProcesses = flags.runWithNoprocesses;
            return temp;
        }

        private void btn_CreateNewProcess_Click(object sender, RoutedEventArgs e)
        {
            if (osCore == null)
            {
                CreateOsCore();
                if (osCore == null)
                {
                    MessageBox.Show("Failed To create OS Core");
                    return;
                }
            }
            ProcessFlags? processFlags = CreateProcessFlags();
            if (processFlags == null)
            {
                MessageBox.Show("Failed To create process invalid flags");
                return;
            }
            SimulatorProcess proc = osCore.CreateProcess(processFlags.Value);
            processes.Add(proc);
            if (osCore.Scheduler == null)
            {
                var schedulerFlags = CreateSchedulerFlags(globalFlags);
                if (schedulerFlags != null)
                    osCore.Scheduler = new Scheduler(schedulerFlags.Value);
                else
                    throw new NullReferenceException("Scheduler flags is NULL");
                if (osCore.Scheduler == null)
                {
                    throw new NullReferenceException("Scheduler is NULL");
                }
            }
            osCore.Scheduler.ReadyQueue.Enqueue(proc);
        }

        private ProcessFlags? CreateProcessFlags()
        {
            ProcessFlags temp = new ProcessFlags();
            temp.program = (SimulatorProgram) lst_Programs.SelectedItem;
            temp.burstTime = 0;
            temp.childProcesses = new List<SimulatorProcess>();
            if (chk_Use_Default_Scheduler_Process.IsChecked != null && chk_Use_Default_Scheduler_Process.IsChecked.Value)
            {
                temp.defaultScheduler = true;
            }
            else
            {
                temp.defaultScheduler = false;
            }
            if (chk_DelayedProcess.IsChecked != null && chk_DelayedProcess.IsChecked.Value)
            {
                temp.delayedProcess = true;
                temp.delayedProcessTime = (int) cmb_Arrival_Delay.SelectedValue;
                if (rdb_DelaySecs.IsChecked != null && rdb_DelaySecs.IsChecked.Value)
                {
                    temp.delayTimeUnit = EnumTimeUnit.SECONDS;
                }
                else if (rdb_DelayTicks.IsChecked != null && rdb_DelayTicks.IsChecked.Value)
                {
                    temp.delayTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a delay time unit");
                    return null;
                }
            }
            if (chk_Display_Profile.IsChecked != null && chk_Display_Profile.IsChecked.Value)
            {
                temp.displayProfile = true;
            }
            else
            {
                temp.displayProfile = false;
            }
            if (chk_Children_Die.IsChecked != null && chk_Children_Die.IsChecked.Value)
            {
                temp.parentDiesChildrenDie = true;
            }
            else
            {
                temp.parentDiesChildrenDie = false;
            }
            temp.parentProcess = null;
            if (Int32.Parse(txt_ProcessLifetime.Text) == 0)
            {
                temp.processLifetime = Int32.MaxValue;
                temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
            }
            else
            {
                temp.processLifetime = Int32.Parse(txt_ProcessLifetime.Text);
                if (rdb_LifetimeSecs.IsChecked != null && rdb_LifetimeSecs.IsChecked.Value)
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
                }
                else if (rdb_LifetimeTicks.IsChecked != null && rdb_LifetimeTicks.IsChecked.Value)
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a Process Lifetime Time Unit");
                    return null;
                }
            }
            temp.processMemory = (int) cmb_Pages.SelectedValue;
            temp.processName = txtProcessName.Text;
            temp.processPriority = (int) cmb_Priority.SelectedValue;
            temp.processState = EnumProcessState.READY;
            temp.processSwapped = false;
            if (chk_Use_Default_Scheduler_Process.IsChecked != null && chk_Use_Default_Scheduler_Process.IsChecked.Value)
            {
                temp.defaultScheduler = true;
            }
            else
            {
                temp.defaultScheduler = false;
            }
            return temp;
        }
    }
}
