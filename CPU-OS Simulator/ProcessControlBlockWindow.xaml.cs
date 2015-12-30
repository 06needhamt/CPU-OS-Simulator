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
        private MainWindow main_Parent = null;
        private OperatingSystemMainWindow os_Parent = null;
        private LinkedListNode<ProcessControlBlock> currentControlBlock = null;
        private LinkedList<ProcessControlBlock> currentControlBlocks = null;
        public static ProcessControlBlockWindow currentInstance;

        public ProcessControlBlockWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetPCBWindowInstance();
        }

        public ProcessControlBlockWindow(MainWindow main_Parent, LinkedListNode<ProcessControlBlock> currentControlBlock)
        {
            this.main_Parent = main_Parent;
            InitializeComponent();
            currentInstance = this;
            this.currentControlBlock = currentControlBlock;
            currentControlBlocks = new LinkedList<ProcessControlBlock>();
            if (currentControlBlock != null)
            {
                currentControlBlocks.AddLast(currentControlBlock);
            }
            SetPCBWindowInstance();
        }

        public ProcessControlBlockWindow(OperatingSystemMainWindow os_Parent, LinkedListNode<ProcessControlBlock> currentControlBlock)
        {
            this.os_Parent = os_Parent;
            InitializeComponent();
            currentInstance = this;
            this.currentControlBlock = currentControlBlock;
            currentControlBlocks = new LinkedList<ProcessControlBlock>();
            if (currentControlBlock != null)
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

        private void UpdateText()
        {
            if (currentControlBlock != null)
            {
                txt_PCB_Info.Text = currentControlBlock.ToString();
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
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
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
    }
}
