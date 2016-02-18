using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CPU_OS_Simulator.Controls.Resource_Controls.Shapes
{
    public class R1Shape : UIElement
    {
        private Rectangle rect;
        private readonly double width;
        private readonly double height;
        private Color colour;


        public R1Shape() { }

        public R1Shape(double width, double height, Color colour)
        {
            this.width = width;
            this.height = height;
            this.colour = colour;
            rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1.0;
            rect.Fill = new SolidColorBrush(colour);

        }

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public double Width
        {
            get { return width; }
        }

        public double Height
        {
            get { return height; }
        }

        public Color Colour
        {
            get { return colour; }
            set { colour = value; rect.Fill = new SolidColorBrush(value); }
        }

    }
}
