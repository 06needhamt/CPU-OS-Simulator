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

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for OperatingSystemMainWindow.xaml
    /// </summary>
    public partial class OperatingSystemMainWindow : Window
    {
        private MainWindow parent;
        public OperatingSystemMainWindow()
        {
            InitializeComponent();
        }

        public OperatingSystemMainWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
