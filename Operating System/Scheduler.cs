using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Timers;
using CPU_OS_Simulator.CPU;

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
        private int cpuClockSpeed;
        private BackgroundWorker executionWorker;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

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
            cpuClockSpeed = flags.cpuClockSpeed;
            CreateBackgroundWorker();
        }

        private void CreateBackgroundWorker()
        {
            executionWorker = new BackgroundWorker();
            executionWorker.DoWork += CreateExecutionThread;
            executionWorker.WorkerSupportsCancellation = true;
            executionWorker.WorkerReportsProgress = true;
            executionWorker.ProgressChanged += UpdateInterface;
        }

        private void CreateExecutionThread(object sender, DoWorkEventArgs e)
        {
            var executionThread = new Thread(RunScheduler);
            executionThread.Start();
        }

        /// <summary>
        /// Bridge function used to call functions on the main thread from within the background thread
        /// </summary>
        /// <param name="FunctionPointer"> The function to call </param>
        /// <returns>A task to indicate to the main thread that the function has finished executing</returns>
        private async Task<int> CallFromMainThread(Func<Task<int>> FunctionPointer)
        {
            var invoke = dispatcher.Invoke(FunctionPointer);
            if (invoke != null) await invoke;
            return 0;
        }
        public bool Start()
        {
            if (readyQueue.Count == 0 && !runningWithNoProcesses)
            {
                MessageBox.Show("OS Execution complete");
                return true;
            }
            executionWorker.RunWorkerAsync();
            return true;
        }
        /// <summary>
        /// Asynchronous function called after every instruction is executed to update required values and user interface asynchronously
        /// </summary>
        /// <param name="sender"> the object that triggered this event</param>
        /// <param name="e">The parameters passed to this event ></param>
        private async void UpdateInterface(object sender, ProgressChangedEventArgs e)
        {
            dynamic OSWindow = GetOSWindow();
            OSWindow.lst_ReadyProcesses.ItemsSource = null;
            OSWindow.lst_RunningProcesses.ItemsSource = null;
            OSWindow.lst_WaitingProcesses.ItemsSource = null;
            OSWindow.lst_ReadyProcesses.Items.Clear();
            OSWindow.lst_RunningProcesses.Items.Clear();
            OSWindow.lst_WaitingProcesses.Items.Clear();
            List<SimulatorProcess> runningProcesses = new List<SimulatorProcess>();
            runningProcesses.Add(OSWindow.osCore.Scheduler.RunningProcess);
            OSWindow.lst_RunningProcesses.ItemsSource = runningProcesses;
            OSWindow.lst_ReadyProcesses.ItemsSource = OSWindow.OsCore.Scheduler.ReadyQueue;
            OSWindow.lst_WaitingProcesses.ItemsSource = OSWindow.OsCore.Scheduler.WaitingQueue;
            OSWindow.txtProcessName.Text = "P" + Convert.ToString(OSWindow.Processes.Count + 1);
        }
        /// <summary>
        /// Asynchronous function called after every instruction is executed to update required values and user interface asynchronously
        /// </summary>
        /// <returns> A task to indicate to the main thread that the function has finished executing</returns>
        private async Task<int> UpdateInterface()
        {
            dynamic OSWindow = GetOSWindow();
            OSWindow.lst_ReadyProcesses.ItemsSource = null;
            OSWindow.lst_RunningProcesses.ItemsSource = null;
            OSWindow.lst_WaitingProcesses.ItemsSource = null;
            OSWindow.lst_ReadyProcesses.Items.Clear();
            OSWindow.lst_RunningProcesses.Items.Clear();
            OSWindow.lst_WaitingProcesses.Items.Clear();
            List<SimulatorProcess> runningProcesses = new List<SimulatorProcess>();
            runningProcesses.Add(OSWindow.OsCore.Scheduler.RunningProcess);
            OSWindow.lst_RunningProcesses.ItemsSource = runningProcesses;
            OSWindow.lst_ReadyProcesses.ItemsSource = OSWindow.OsCore.Scheduler.ReadyQueue;
            OSWindow.lst_WaitingProcesses.ItemsSource = OSWindow.OsCore.Scheduler.WaitingQueue;
            OSWindow.txtProcessName.Text = "P" + Convert.ToString(OSWindow.Processes.Count + 1);
            return 0;
        }
        private async void RunScheduler()
        {
            while (readyQueue.Count > 0)
            {
                switch (schedulingPolicy)
                {
                    case EnumSchedulingPolicies.FIRST_COME_FIRST_SERVED:
                    {
                        runningProcess = readyQueue.Dequeue();
                        //ProcessExecutionUnit unit = runningProcess.Unit;
                        while (!runningProcess.Unit.Stop && !runningProcess.Unit.Done &&
                               !runningProcess.Unit.Process.Terminated && !runningProcess.Unit.TimedOut)
                        {
                            runningProcess.Unit.ExecuteInstruction();
                            await CallFromMainThread(UpdateInterface);
                        }
                        if (runningProcess.Unit.TimedOut)
                        {
                            runningProcess.ProcessState = EnumProcessState.READY;
                            readyQueue.Enqueue(runningProcess);
                            PCBFlags? flags = CreatePCBFlags(ref runningProcess);
                            if (flags != null)
                            {
                                runningProcess.ControlBlock = new ProcessControlBlock(flags.Value);
                            }
                            else
                            {
                                MessageBox.Show("There was an error while creating process control block flags");
                                return;
                            }
                                runningProcess = readyQueue.Dequeue();

                            }
                        if (runningProcess.Unit.Stop)
                        {
                            runningProcess.ProcessState = EnumProcessState.WAITING;
                            waitingQueue.Enqueue(runningProcess);
                            PCBFlags? flags = CreatePCBFlags(ref runningProcess);
                            if (flags != null)
                            {
                                runningProcess.ControlBlock = new ProcessControlBlock(flags.Value);
                            }
                            else
                            {
                                MessageBox.Show("There was an error while creating process control block flags");
                                return;
                            }
                            runningProcess = readyQueue.Dequeue();
                        }
                        break;
                    }
                    case EnumSchedulingPolicies.SHORTEST_JOB_FIRST:
                    {
                        readyQueue = new Queue<SimulatorProcess>(readyQueue.OrderBy(x => x.Program.Instructions.Count));
                        runningProcess = readyQueue.Dequeue();
                        await CallFromMainThread(UpdateInterface);
                          while (!runningProcess.Unit.Stop && !runningProcess.Unit.Done &&
                                 !runningProcess.Unit.Process.Terminated && !runningProcess.Unit.TimedOut)
                          {
                              runningProcess.Unit.ExecuteInstruction();
                              await CallFromMainThread(UpdateInterface);
                          }
                          if (runningProcess.Unit.TimedOut)
                          {
                             runningProcess.ProcessState = EnumProcessState.READY;
                              readyQueue.Enqueue(runningProcess);
                              PCBFlags? flags = CreatePCBFlags(ref runningProcess);
                              if (flags != null)
                              {
                                  runningProcess.ControlBlock = new ProcessControlBlock(flags.Value);
                              }
                              else
                              {
                                  MessageBox.Show("There was an error while creating process control block flags");
                                  return;
                              }
                              runningProcess = readyQueue.Dequeue();

                          }
                          if (runningProcess.Unit.Stop)
                          {
                              runningProcess.ProcessState = EnumProcessState.WAITING;
                              waitingQueue.Enqueue(runningProcess);
                              PCBFlags? flags = CreatePCBFlags(ref runningProcess);
                              if (flags != null)
                              {
                                  runningProcess.ControlBlock = new ProcessControlBlock(flags.Value);
                              }
                              else
                               {
                                  MessageBox.Show("There was an error while creating process control block flags");
                                  return;
                               }
                               runningProcess = readyQueue.Dequeue();
                            }
                            break;
                        }
                    case EnumSchedulingPolicies.ROUND_ROBIN:
                    {
                        if (RR_Priority_Policy == EnumPriorityPolicy.NON_PRE_EMPTIVE)
                        {
                            readyQueue = new Queue<SimulatorProcess>(readyQueue.OrderBy(x => x.ProcessPriority));
                            int timeOutMills = 0;
                            runningProcess = readyQueue.Dequeue();

                            if (timeSliceUnit == EnumTimeUnit.SECONDS)
                            {
                                timeOutMills = (int) (RR_TimeSlice*1000);
                            }
                            else if (timeSliceUnit == EnumTimeUnit.TICKS)
                            {
                                timeOutMills = (int) (RR_TimeSlice*runningProcess.Unit.ClockSpeed);
                            }
                            else
                            {
                                MessageBox.Show("Unknown time slice unit specified " + timeSliceUnit.ToString());
                            }

                            while (runningProcess != null)
                            {
                                if (runningProcess.ControlBlock != null)
                                {
                                    ProcessControlBlock p = runningProcess.ControlBlock;
                                    runningProcess.Unit.CurrentIndex = p.SpecialRegisters[0].Value / 4;
                                    for (int i = 0; i < runningProcess.ControlBlock.Registers.Length; i++)
                                    {
                                        string registerName = String.Format("R{0:00}",i);
                                        Register.FindRegister(registerName).SetRegisterValue(p.Registers[i].Value,p.Registers[i].Type);
                                    }
                                    for (int i = 0; i < p.SpecialRegisters.Length; i++)
                                    {
                                        int v = 0;
                                        if (int.TryParse(p.SpecialRegisters[i].ValueString, out v))
                                            SpecialRegister.FindSpecialRegister(p.SpecialRegisters[i].Name).SetRegisterValue(p.SpecialRegisters[i].Value,p.SpecialRegisters[i].Type);
                                        else
                                            SpecialRegister.FindSpecialRegister(p.SpecialRegisters[i].Name).SetRegisterValue(p.SpecialRegisters[i].ValueString, p.SpecialRegisters[i].Type);
                                    }

                                }
                                    System.Timers.Timer t = new System.Timers.Timer((double)timeOutMills);
                                    t.Elapsed += TOnElapsed;
                                    t.Start();
                                    
                                while (!runningProcess.Unit.Stop && !runningProcess.Unit.Done &&
                                        !runningProcess.Unit.Process.Terminated && !runningProcess.Unit.TimedOut)
                                {
                                    runningProcess.Unit.ExecuteInstruction();
                                    await CallFromMainThread(UpdateInterface);
                                }
                            }
                        }
                        break;
                    }
                    case EnumSchedulingPolicies.LOTTERY_SCHEDULING:
                    {
                        break;
                    }
                    case EnumSchedulingPolicies.FAIR_SHARE_SCEDULING:
                    {
                        break;
                    }
                    case EnumSchedulingPolicies.UNKNOWN:
                    {
                        MessageBox.Show("Please select a scheduling policy");
                        return;
                    }
                    default:
                    {
                        MessageBox.Show("Unknown scheduling policy selected");
                        return;
                    }
                }
            }
            MessageBox.Show("OS Execution Complete");
            runningProcess = null;
            await CallFromMainThread(UpdateInterface);
            return;
        }

        private async void TOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (runningProcess != null)
            {
                runningProcess.Unit.TimedOut = true;
                runningProcess.ProcessState = EnumProcessState.READY;
                readyQueue.Enqueue(runningProcess);
                PCBFlags? flags = CreatePCBFlags(ref runningProcess);
                if (flags != null)
                {
                    runningProcess.ControlBlock = new ProcessControlBlock(flags.Value);
                }
                else
                {
                    MessageBox.Show("There was an error while creating process control block flags");
                    return;
                }
                if (readyQueue.Count > 0)
                {
                    runningProcess = readyQueue.Dequeue();
                    runningProcess.ProcessState = EnumProcessState.RUNNING;
                }
            }
        }

        //private void OnProcessTimeout(object state)
        //{
        //    runningProcess.ProcessState = EnumProcessState.READY;
        //    readyQueue.Enqueue(runningProcess);
        //    if (readyQueue.Count > 0)
        //    {
        //        runningProcess = readyQueue.Dequeue();
        //    }
        //    else
        //    {
        //        runningProcess = null;
        //    }
        //}
        private PCBFlags? CreatePCBFlags(ref SimulatorProcess currentProcess)
        {
            //SimulatorProcess currentProcess = waitingQueue.Peek();
            PCBFlags flags = new PCBFlags();
            flags.CPUID = currentProcess.CPUID;
            flags.OSID = currentProcess.OSID;
            flags.allocatedResources = currentProcess.AllocatedResources;
            flags.avgBurstTime = 0;
            flags.avgWaitingTime = 0; //TODO calculate correct values
            flags.baseAddress = currentProcess.Program.BaseAddress;
            flags.proceessMemory = currentProcess.ProcessMemory;
            flags.processID = currentProcess.ProcessID;
            flags.processName = currentProcess.ProcessName;
            flags.processPriority = currentProcess.ProcessPriority;
            flags.CPUID = 0; //TODO Change this once multiple CPU's are implemented
            flags.processState = currentProcess.ProcessState;
            flags.programName = currentProcess.ProgramName;
            flags.requestedResources = currentProcess.RequestedResources;
            flags.resourceStarved = currentProcess.ResourceStarved;
            flags.startAddress = currentProcess.Program.BaseAddress; // TODO Implement program start address
            flags.registers = new Register[21];
            for (int i = 0; i < flags.registers.Length; i++)
            {
                string registerName = String.Empty;
                if (i < 10)
                    registerName = "R0" + i;
                else
                    registerName = "R" + i;
                flags.registers[i] = Register.FindRegister(registerName);
            }
            flags.specialRegisters = new SpecialRegister[7];
            flags.specialRegisters[0] = SpecialRegister.PC;
            flags.specialRegisters[1] = SpecialRegister.BR;
            flags.specialRegisters[2] = SpecialRegister.SP;
            flags.specialRegisters[3] = SpecialRegister.IR;
            flags.specialRegisters[4] = SpecialRegister.SR;
            flags.specialRegisters[5] = SpecialRegister.MAR;
            flags.specialRegisters[6] = SpecialRegister.MDR;
            return flags;
        }

        private dynamic GetOSWindow()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("OperatingSystemMainWindowInstance").GetValue(null); // get the value of the static OperatingSystemMainWindowInstance field
            return window;
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