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
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for PhysicalMemoryWindow.xaml
    /// </summary>
    public partial class PhysicalMemoryWindow : Window
    {
        public static PhysicalMemoryWindow currentInstance;
        private MainWindow mainParent;
        private OperatingSystemMainWindow osParent;

        public PhysicalMemoryWindow()
        {
            osParent = null;
            mainParent = null;
            InitializeComponent();
            currentInstance = this;
        }

        public PhysicalMemoryWindow(MainWindow mainParent)
        {
            this.mainParent = mainParent;
            this.osParent = null;
            InitializeComponent();
            currentInstance = this;
        }

        public PhysicalMemoryWindow(OperatingSystemMainWindow osParent)
        {
            this.osParent = osParent;
            this.mainParent = null;
            InitializeComponent();
            currentInstance = this;
        }

        private void PhysicalMemoryWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            foreach ()
            {
                
            }
        }
    }
}
