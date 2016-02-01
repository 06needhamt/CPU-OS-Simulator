using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using CPU_OS_Simulator.Operating_System;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for ProcessControlBlockWindow.xaml
    /// </summary>
    public partial class ProcessControlBlockWindow : Window
    {
        private MainWindow main_Parent;
        private OperatingSystemMainWindow os_Parent;
        private ProcessListWindow procList_Parent;
        private LinkedListNode<ProcessControlBlock> currentControlBlock;
        private LinkedList<ProcessControlBlock> currentControlBlocks;
        /// <summary>
        /// This variable holds the current active instance of this window
        /// </summary>
        public static ProcessControlBlockWindow currentInstance;

        /// <summary>
        /// Default Constructor for process control block window
        /// </summary>
        public ProcessControlBlockWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetPCBWindowInstance();
        }
        /// <summary>
        /// Constructor for process control block window when constructing from MainWindow
        /// </summary>
        /// <param name="main_Parent"> the window that is creating this window</param>
        /// <param name="currentControlBlock"> the control block to be loaded</param>
        public ProcessControlBlockWindow(MainWindow main_Parent, LinkedListNode<ProcessControlBlock> currentControlBlock)
        {
            this.main_Parent = main_Parent;
            this.os_Parent = null;
            this.procList_Parent = null;
            InitializeComponent();
            currentInstance = this;
            this.currentControlBlock = currentControlBlock;
            currentControlBlocks = new LinkedList<ProcessControlBlock>();
            if (currentControlBlock != null && currentControlBlock.Value != null)
            {
                currentControlBlocks.AddLast(currentControlBlock);
            }
            SetPCBWindowInstance();
        }
        /// <summary>
        /// Constructor for process control block window when constructing from OperatingSystemMainWindow
        /// </summary>
        /// <param name="os_Parent"> the window that is creating this window</param>
        /// <param name="currentControlBlock"> the control block to be loaded</param>
        public ProcessControlBlockWindow(OperatingSystemMainWindow os_Parent, LinkedListNode<ProcessControlBlock> currentControlBlock)
        {
            this.os_Parent = os_Parent;
            this.main_Parent = null;
            this.procList_Parent = null;
            InitializeComponent();
            currentInstance = this;
            this.currentControlBlock = currentControlBlock;
            currentControlBlocks = new LinkedList<ProcessControlBlock>();
            if (currentControlBlock != null && currentControlBlock.Value != null)
            {
                currentControlBlocks.AddLast(currentControlBlock);
            }
            SetPCBWindowInstance();
        }
        /// <summary>
        /// Constructor for process control block window when constructing from ProcessListWindow
        /// </summary>
        /// <param name="procList_Parent"> the window that is creating this window</param>
        /// <param name="currentControlBlock"> the control block to be loaded</param>
        public ProcessControlBlockWindow(ProcessListWindow procList_Parent,
            LinkedListNode<ProcessControlBlock> currentControlBlock)
        {
            this.procList_Parent = procList_Parent;
            this.main_Parent = null;
            this.os_Parent = null;
            InitializeComponent();
            currentInstance = this;
            this.currentControlBlock = currentControlBlock;
            currentControlBlocks = new LinkedList<ProcessControlBlock>();
            if (currentControlBlock != null && currentControlBlock.Value != null)
            {
                currentControlBlocks.AddLast(currentControlBlock);
            }
            SetPCBWindowInstance();
        }

        private void ProcessControlBlockWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            txt_PCB_Info.Foreground = new SolidColorBrush(Colors.DarkBlue);
            if (currentControlBlocks.First != null)
            {
                currentControlBlock = currentControlBlocks.First;
            }
            UpdateText();
        }

        /// <summary>
        /// This function updates the text block with information from the current PCB
        /// </summary>
        private void UpdateText()
        {
            if (currentControlBlock != null && currentControlBlock.Value != null)
            {
                txt_PCB_Info.Text = currentControlBlock.Value.ToString();
            }
            else
            {
                txt_PCB_Info.Text = "No PCB Loaded";
            }
        }

        /// <summary>
        /// This method sets the current instance of PCB window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetPCBWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("ProcessControlBlockWindowInstance").SetValue(null, currentInstance);
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_Previous_PCB_Click(object sender, RoutedEventArgs e)
        {
            if (currentControlBlock.Previous != null)
            {
                currentControlBlock = currentControlBlock.Previous;
                UpdateText();
            }
        }

        private void btn_NextPCB_Click(object sender, RoutedEventArgs e)
        {
            if (currentControlBlock.Next != null)
            {
                currentControlBlock = currentControlBlock.Next;
                UpdateText();
            }
        }

        private void ProcessControlBlockWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetPCBWindowInstance();
        }
    }
}
