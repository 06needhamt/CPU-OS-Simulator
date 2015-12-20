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
    /// Interaction logic for OperatingSystemMainWindow.xaml
    /// </summary>
    public partial class OperatingSystemMainWindow : Window
    {
        private MainWindow parent;
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
        /// This method sets the current instance of memory window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetMemoryWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("OperatingSystemMainWindowInstance").SetValue(null, currentInstance);
        }
    }
}
