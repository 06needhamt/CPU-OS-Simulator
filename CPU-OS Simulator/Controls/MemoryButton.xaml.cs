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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator.Controls
{
    /// <summary>
    /// Interaction logic for MemoryButton.xaml
    /// </summary>
    public partial class MemoryButton : Button
    {
        private MemoryPage page;
        public MemoryButton()
        {
            InitializeComponent();
            Click += new RoutedEventHandler(button_Click);
        }

        public MemoryPage GetMemoryPage(int frameNumber)
        {
            return MainWindow.currentInstance.Memory.RequestMemoryPage(frameNumber);
        }

        public void ShowMemoryWindow()
        {
            if (MemoryWindow.currentInstance != null)
            {
                MemoryWindow.currentInstance.Close();
            }
            MemoryWindow m = new MemoryWindow(MainWindow.currentInstance, page);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
