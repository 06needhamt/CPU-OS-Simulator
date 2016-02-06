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

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for UtilisationWindow.xaml
    /// </summary>
    public partial class UtilisationWindow : Window
    {
        public static UtilisationWindow currentInstance;
        private OperatingSystemMainWindow parent;

        public UtilisationWindow()
        {
            InitializeComponent();
        }

        public UtilisationWindow(OperatingSystemMainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetUtilisationWindowInstance();

        }
        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            cmb_CPUGraphScale.SelectedIndex = 3;
            //TODO Find out how to update graph values
        }

        /// <summary>
        /// This method sets the current instance of UtilisationWindow in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetUtilisationWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("UtilisationWindowInstance").SetValue(null, currentInstance);
        }

        private void btn_SetSwap_Click(object sender, RoutedEventArgs e)
        {
            //TODO Find out how to update graph values
        }

        private void btn_MemReset_Click(object sender, RoutedEventArgs e)
        {
            txt_MaxSwap.Text = Convert.ToString(2560);
            //TODO Find out how to update graph values
        }

        private void UtilisationWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetUtilisationWindowInstance();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chk_StayOnTop_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void chk_StayOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
    }
}
