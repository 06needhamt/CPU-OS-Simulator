using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using OxyPlot; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using OxyPlot.Series; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using OxyPlot.Wpf; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using CPU_OS_Simulator.Annotations;
using CategoryAxis = OxyPlot.Axes.CategoryAxis;
using ColumnSeries = OxyPlot.Series.ColumnSeries;


namespace CPU_OS_Simulator.Controls.Graphs.ViewModels
{
    public class MemoryGraphModel : MainViewModel
    {
        public int AllocBytes
        {
            get { return allocBytes; }
            set { allocBytes = value; OnPropertyChanged("AllocBytes"); }
        }

        public int FreeBytes
        {
            get { return freeBytes; }
            set { freeBytes = value; OnPropertyChanged("FreeBytes"); }
        }

        public int SwappedBytes
        {
            get { return swappedBytes; }
            set { swappedBytes = value; OnPropertyChanged("SwappedBytes"); }
        }

        private int allocBytes;
        private int freeBytes;
        private int swappedBytes;

        public MemoryGraphModel()
        {
            PlotModel = new PlotModel();
            allocBytes = 0;
            freeBytes = 0;
            swappedBytes = 0;
            Draw();
        }

        [NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        public override void Draw()
        {
            base.Draw();
            PlotModel.Title = "Memory Usage";
            CategoryAxis X = new CategoryAxis();
            CategoryAxis Y = new CategoryAxis();
            X.Title = "Usage";
            PlotModel.Axes.Add(Y);
            PlotModel.Axes.Add(X);
            ColumnSeries alloc = new ColumnSeries();
            alloc.Items.Add(new ColumnItem(256));
            alloc.ColumnWidth = 25;
            alloc.FillColor = Colors.CornflowerBlue.ToOxyColor();
            alloc.Title = "Allocated";
            PlotModel.Series.Add(alloc);
            ColumnSeries free = new ColumnSeries();
            free.Items.Add(new ColumnItem(512));
            free.ColumnWidth = 25;
            free.FillColor = Colors.GreenYellow.ToOxyColor();
            free.Title = "Free";
            PlotModel.Series.Add(free);
            ColumnSeries swap = new ColumnSeries();
            swap.Items.Add(new ColumnItem(768));
            swap.ColumnWidth = 25;
            swap.FillColor = Colors.Red.ToOxyColor();
            swap.Title = "Swap";
            PlotModel.Series.Add(swap);

        }
    }
}
