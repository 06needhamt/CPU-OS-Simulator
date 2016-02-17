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
    /// Interaction logic for ResourceWindow.xaml
    /// </summary>
    public partial class ResourceWindow : Window
    {
        public static ResourceWindow currentInstance;
        private OperatingSystemMainWindow parent;

        public ResourceWindow()
        {
            this.parent = null;
            InitializeComponent();
            currentInstance = this;
            SetResourceWindowInstance();
        }

        public ResourceWindow(OperatingSystemMainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetResourceWindowInstance();
        }

        /// <summary>
        /// This method sets the current instance of Resource window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetResourceWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("ResourceWindowInstance").SetValue(null, currentInstance);
        }
    }
}
