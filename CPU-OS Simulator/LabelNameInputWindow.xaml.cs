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
    /// Interaction logic for LabelNameInputWindow.xaml
    /// </summary>
    public partial class LabelNameInputWindow : Window
    {
        private InstructionsWindow parent;

        public LabelNameInputWindow()
        {
            parent = null;
            InitializeComponent();
        }

        public LabelNameInputWindow(InstructionsWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            parent.LabelName = txtName.Text;
        }
    }
}
