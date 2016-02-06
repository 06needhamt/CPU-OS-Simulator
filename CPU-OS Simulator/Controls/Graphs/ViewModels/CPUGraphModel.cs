using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CPU_OS_Simulator.Annotations;
using OxyPlot; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using OxyPlot.Series; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using OxyPlot.Wpf; // See Third Party Libs/Credits.txt for licensing information for OxyPlot
using CategoryAxis = OxyPlot.Axes.CategoryAxis;
using ColumnSeries = OxyPlot.Series.ColumnSeries;

namespace CPU_OS_Simulator.Controls.Graphs.ViewModels
{
    /// <summary>
    /// This class represents the view model for the CPU utilisation graph
    /// </summary>
    public class CPUGraphModel : MainViewModel
    {
        private int utilisationPercentage;
        private int graphScale;
        /// <summary>
        /// Constructor for CPU graph view model
        /// </summary>
        public CPUGraphModel() : base()
        {
            this.PlotModel = new PlotModel();
            utilisationPercentage = 0;
            Draw();
        }
        /// <summary>
        /// Property for the UtilisationPercentage column value
        /// </summary>
        public int UtilisationPercentage
        {
            get { return utilisationPercentage; }
            set { utilisationPercentage = value; OnPropertyChanged("UtilisationPercentage"); }
        }
        /// <summary>
        /// Property for the increment of the graph scale
        /// </summary>
        public int GraphScale
        {
            get { return graphScale; }
            set { graphScale = value; OnPropertyChanged("GraphScale");}
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
            PlotModel.Title = "%CPU Utilisation";
            CategoryAxis X = new CategoryAxis();
            CategoryAxis Y = new CategoryAxis(); // Create the X and Y axes
            X.Title = "% Utilised";
            Y.IntervalLength = 1.0;
            PlotModel.Axes.Add(Y);
            PlotModel.Axes.Add(X); // add them to the model
            ColumnSeries col = new ColumnSeries(); // create a column
            col.Items.Add(new ColumnItem(100)); // set the value of the column
            col.ColumnWidth = 25; 
            col.FillColor = Colors.CornflowerBlue.ToOxyColor();
            PlotModel.Series.Add(col); // add the column to the graph
            
        }
    }
}
