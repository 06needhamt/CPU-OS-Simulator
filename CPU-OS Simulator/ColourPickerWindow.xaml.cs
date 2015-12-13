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
    /// Interaction logic for ColourPickerWindow.xaml
    /// </summary>
    public partial class ColourPickerWindow : Window
    {
        private ConsoleWindow parent;
        private Color selectedColor;

        public ColourPickerWindow()
        {
            InitializeComponent();
        }

        public ColourPickerWindow(ConsoleWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }
        /// <summary>
        /// Property for the selected colour
        /// </summary>
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
    }
}
