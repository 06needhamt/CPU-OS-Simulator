using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace CPU_OS_Simulator.Controls.Graphs.ViewModels
{
    public class CPUGraphModel : MainViewModel
    {
        public CPUGraphModel()
        {
            this.PlotModel = new PlotModel();
            PlotModel.Title = "%CPU Utilisation";
        }
    }
}
