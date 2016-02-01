using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Reflection;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        private OperatingSystemMainWindow parent;
        public static LogWindow currentInstance;
        public LogWindow()
        {
            parent = null;
            InitializeComponent();
            currentInstance = this;
            SetLogWindowInstance();
        }
        public LogWindow(OperatingSystemMainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetLogWindowInstance();
        }

        /// <summary>
        /// This method sets the current instance of Log window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetLogWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("LogWindowInstance").SetValue(null, currentInstance);
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;

        }
    }
}
