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
    /// <summary>
    /// This class represents the Memory Graph ViewModel
    /// </summary>
    public class MemoryGraphModel : MainViewModel
    {
        /// <summary>
        /// Property for the Allocated bytes column value
        /// </summary>
        public int AllocBytes
        {
            get { return allocBytes; }
            set { allocBytes = value; OnPropertyChanged("AllocBytes"); }
        }
        /// <summary>
        /// Property for the Free bytes column value
        /// </summary>
        public int FreeBytes
        {
            get { return freeBytes; }
            set { freeBytes = value; OnPropertyChanged("FreeBytes"); }
        }
        /// <summary>
        /// Property for the Swapped bytes column value
        /// </summary>
        public int SwappedBytes
        {
            get { return swappedBytes; }
            set { swappedBytes = value; OnPropertyChanged("SwappedBytes"); }
        }

        private int allocBytes;
        private int freeBytes;
        private int swappedBytes;

        /// <summary>
        /// Constructor for the memory graph view model
        /// </summary>
        public MemoryGraphModel()
        {
            PlotModel = new PlotModel();
            allocBytes = 0;
            freeBytes = 0;
            swappedBytes = 0;
            Draw();
        }
        /// <summary>
        /// This function is called whenever the PropertyChangedEventHandler is fired
        /// </summary>
        /// <param name="propertyName">the property that was updated</param>
        [NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// This function is called every frame to update the graph's values
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            PlotModel.Title = "Memory Usage";
            CategoryAxis X = new CategoryAxis();
            CategoryAxis Y = new CategoryAxis(); // Create the X and Y axes
            X.Title = "Usage";
            PlotModel.Axes.Add(Y);
            PlotModel.Axes.Add(X); // Add them to the model
            ColumnSeries alloc = new ColumnSeries();
            alloc.Items.Add(new ColumnItem(256)); // create a column and set its value
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
            PlotModel.Series.Add(swap); // add the columns to the plot model

        }
    }
}
