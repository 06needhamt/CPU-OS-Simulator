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
    /// Interaction logic for InterruptsWindow.xaml
    /// </summary>
    public partial class InterruptsWindow : Window
    {
        public static InterruptsWindow currentInstance;
        private MainWindow mainParent;
        private OperatingSystemMainWindow osParent;

        public InterruptsWindow()
        {
            this.mainParent = null;
            this.osParent = null;
            InitializeComponent();
            currentInstance = this;
            SetInterruptWindowInstance();
        }

        public InterruptsWindow(MainWindow mainParent)
        {
            this.mainParent = mainParent;
            this.osParent = null;
            InitializeComponent();
            currentInstance = this;
            SetInterruptWindowInstance();
        }

        public InterruptsWindow(OperatingSystemMainWindow osParent)
        {
            this.mainParent = null;
            this.osParent = osParent;
            InitializeComponent();
            currentInstance = this;
            SetInterruptWindowInstance();
        }

        /// <summary>
        /// This method sets the current instance of Interrupt window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetInterruptWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("InterruptWindowInstance").SetValue(null, currentInstance);
        }

        /// <summary>
        /// This function gets the operating system main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of operating system main window </returns>
        private dynamic GetOSMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("OperatingSystemMainWindowInstance").GetValue(null); // get the value of the static OperatingSystemMainWindowInstance field
            return window;
        }

        private void txt_Int1Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(1,(int)address);
                core.Handles.SetVectoredInterrupt(1, (int)address);
            }
        }

        private void txt_Int2Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(2, (int)address);
                core.Handles.SetVectoredInterrupt(2, (int)address);
            }
        }

        private void txt_Int3Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(3, (int)address);
                core.Handles.SetVectoredInterrupt(3, (int)address);
            }
        }

        private void txt_Int4Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(4, (int)address);
                core.Handles.SetVectoredInterrupt(4, (int)address);
            }
        }

        private void txt_Int5Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(5, (int)address);
                core.Handles.SetVectoredInterrupt(5, (int)address);
            }
        }

        private void txt_Int6Location_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            uint address = 0;
            if (uint.TryParse(txt_Int1Location.Text, out address))
            {
                core.Handles.SetPolledInterrupt(6, (int)address);
                core.Handles.SetVectoredInterrupt(6, (int)address);
            }
        }

        private void btn_Int1Trigger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Int2Trigger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Int3Trigger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Int5Trigger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Int6Trigger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int1Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int2Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int3Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int4Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int5Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int6Suspend_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int1Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int2Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int3Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int4Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int5Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Int6Suspend_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_StayOnTop_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_StayOnTop_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (osParent == null && GetOSMainWindowInstance() == null)
            {
                MessageBox.Show("Please open the os before clearing interrupts");
                return;
            }
            OperatingSystemMainWindow os = GetOSMainWindowInstance();
            OSCore core = os.OsCore;
            txt_Int1Location.Text = "";
            txt_Int2Location.Text = "";
            txt_Int3Location.Text = "";
            txt_Int4Location.Text = "";
            txt_Int5Location.Text = "";
            txt_Int6Location.Text = "";

            core.Handles.PINT1 = null;
            core.Handles.VINT1 = null;
            core.Handles.PINT2 = null;
            core.Handles.VINT2 = null;
            core.Handles.PINT3 = null;
            core.Handles.VINT3 = null;
            core.Handles.PINT4 = null;
            core.Handles.VINT4 = null;
            core.Handles.PINT5 = null;
            core.Handles.VINT5 = null;
            core.Handles.PINT6 = null;
            core.Handles.VINT6 = null;

        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetInterruptWindowInstance();
        }
    }
}
