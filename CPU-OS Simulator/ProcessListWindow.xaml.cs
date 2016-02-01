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
using CPU_OS_Simulator.Operating_System;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for ProcessListWindow.xaml
    /// </summary>
    public partial class ProcessListWindow : Window
    {
        private List<SimulatorProcess> processes;
        private OperatingSystemMainWindow parent;
        /// <summary>
        /// This variable holds the current active instance of this window
        /// </summary>
        public static ProcessListWindow currentInstance;

        /// <summary>
        /// Default Constructor of ProcessListWindow
        /// </summary>
        public ProcessListWindow()
        {
            processes = new List<SimulatorProcess>();
            parent = null;
            InitializeComponent();
            currentInstance = this;
        }
        /// <summary>
        /// Constructor for ProcessListWindow when constructing from OperatingSystemMainWindow
        /// </summary>
        /// <param name="parent">the window that is creating this window</param>
        /// <param name="processes"> the list of processes to display</param>
        public ProcessListWindow(OperatingSystemMainWindow parent, List<SimulatorProcess> processes)
        {
            this.parent = parent;
            this.processes = processes;
            InitializeComponent();
            currentInstance = this;

        }

        private void ProcessListWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            lst_Processes.Items.Clear();
            lst_Processes.ItemsSource = null;
            lst_Processes.ItemsSource = processes;
        }

        private void btn_ProcessTree_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Process Tree is not currently implemented");
        }

        private void btn_Pcb_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lst_Processes.SelectedIndex;
            if (selectedIndex < 0)
            {
                MessageBox.Show("Please Select a process to view its PCB");
                return;
            }
            ProcessControlBlockWindow window = new ProcessControlBlockWindow(this,
                new LinkedListNode<ProcessControlBlock>(processes[selectedIndex].ControlBlock));
            window.Show();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProcessListWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetProcessListWindowInstance();
        }

        private void chk_StayOnTop_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void chk_StayOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
        private void btn_Profile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Profile is not currently implemented");
        }

        /// <summary>
        /// This method sets the current instance of Process List window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetProcessListWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("ProcessListWindowInstance").SetValue(null, currentInstance);
        }

    }
}
