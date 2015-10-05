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
using System.Reflection;
using System.Diagnostics;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindowGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += " " + GetProgramVersion();
            #if DEBUG
            this.Title += " DEBUG BUILD ";
            #endif
        }
        /// <summary>
        /// Geta the build number of the running program
        /// </summary>
        /// <returns> The build number of the running program</returns>
        private string GetProgramVersion()
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(ExecutingAssembly.Location);
            return VersionInfo.FileVersion;
        }
    }
}
