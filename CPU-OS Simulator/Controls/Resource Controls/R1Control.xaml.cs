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
        private R1Shape shape;

        public R1Control()
        {
            InitializeComponent();
            shape = new R1Shape(RectangleWidth,RectangleHeight,RectangleColour);
            Canvas.SetLeft(shape, 0);
            Canvas.SetTop(shape, 0);
            R1Canvas.Children.Add(shape);
        }

        public R1Shape Shape
        {
            get { return shape; }
            set { shape = value; }
        }
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register
        (
            "RectangleWidth",
            typeof(double),
            typeof(R1Control),
            new PropertyMetadata(75.0)
        );

        public double RectangleWidth
        {
            get { return (double) GetValue(WidthProperty); }
            set { SetValue(WidthProperty,value);}
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register
        (
            "RectangleHeight",
            typeof(double),
            typeof(R1Control),
            new PropertyMetadata(50.0)
        );

        public double RectangleHeight
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static readonly DependencyProperty ColourProperty = DependencyProperty.Register
        (
            "RectangleColour",
            typeof(Color),
            typeof(R1Control),
            new PropertyMetadata(Colors.LawnGreen)
        );

        public Color RectangleColour
        {
            get { return (Color)GetValue(ColourProperty); }
            set { SetValue(ColourProperty, value); }
        }
    }
}
