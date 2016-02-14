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
using CPU_OS_Simulator.Controls.Resource_Controls.Shapes;

namespace CPU_OS_Simulator.Controls.Resource_Controls
{
    /// <summary>
    /// Interaction logic for R1Control.xaml
    /// </summary>
    public partial class R1Control : UserControl
    {
        public R1Control()
        {
            InitializeComponent();
            R1Shape shape = new R1Shape();
            DrawCanvas.Children.Add(shape);
        }
    }
}
