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
    public class CPUGraphModel : MainViewModel
    {
        private int utilisationPercentage;

        public CPUGraphModel() : base()
        {
            this.PlotModel = new PlotModel();
            utilisationPercentage = 0;
            Draw();
        }

        public int UtilisationPercentage
        {
            get { return utilisationPercentage; }
            set { utilisationPercentage = value; OnPropertyChanged("UtilisationPercentage"); }
        }

        [NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        public override void Draw()
        {
            base.Draw();
            PlotModel.Title = "%CPU Utilisation";
            CategoryAxis X = new CategoryAxis();
            CategoryAxis Y = new CategoryAxis();
            X.Title = "% Utilised";
            PlotModel.Axes.Add(Y);
            PlotModel.Axes.Add(X);
            ColumnSeries col = new ColumnSeries();
            col.Items.Add(new ColumnItem(100));
            col.ColumnWidth = 25;
            col.FillColor = Colors.CornflowerBlue.ToOxyColor();
            PlotModel.Series.Add(col);
            
        }
    }
}
