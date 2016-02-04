using System.ComponentModel;
using System.Runtime.CompilerServices;
using CPU_OS_Simulator.Annotations;
using OxyPlot; // See Third Party Libs/Credits.txt for licensing information for OxyPlot

namespace CPU_OS_Simulator.Controls.Graphs.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }

        public MainViewModel()
        {
            PlotModel = new PlotModel();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}