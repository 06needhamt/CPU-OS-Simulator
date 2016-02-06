using System.ComponentModel;
using System.Runtime.CompilerServices;
using CPU_OS_Simulator.Annotations;
using OxyPlot; // See Third Party Libs/Credits.txt for licensing information for OxyPlot

namespace CPU_OS_Simulator.Controls.Graphs.ViewModels
{
    /// <summary>
    /// The Class is the base class for all Oxyplot ViewModels
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This event is fired when a graph property is updated
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private PlotModel plotModel;
        /// <summary>
        /// Property for the XAML PlotModel object 
        /// </summary>
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }
        /// <summary>
        /// Constructor for the view model
        /// </summary>
        public MainViewModel()
        {
            PlotModel = new PlotModel();
            Draw();
        }
        /// <summary>
        /// This function is called whenever the PropertyChangedEventHandler is fired
        /// </summary>
        /// <param name="propertyName">the property that was updated</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
            System.Console.WriteLine("Property " + propertyName + " Has Been Updated");
        }
        /// <summary>
        /// This function is called every frame to update the graph's values
        /// </summary>
        public virtual void Draw()
        {
        }
    }
}