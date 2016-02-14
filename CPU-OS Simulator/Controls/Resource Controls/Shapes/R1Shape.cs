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
    public class R1Shape : Shape
    {
        private Geometry definingGeometry;
        public const double SHAPE_SIZE = 75.0;

        public R1Shape()
        {
            Fill = new SolidColorBrush(Colors.LawnGreen);
            Stroke = Brushes.Black;
            StrokeThickness = 1;
        }

        /// <summary>
        /// Gets a value that represents the <see cref="T:System.Windows.Media.Geometry"/> of the <see cref="T:System.Windows.Shapes.Shape"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Windows.Media.Geometry"/> of the <see cref="T:System.Windows.Shapes.Shape"/>.
        /// </returns>
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext streamGeometryContext)
        {
            
            Point topLeft = new Point(0.0,0.0);
            Point topRight = new Point(SHAPE_SIZE,0.0);
            Point bottomLeft = new Point(0.0,SHAPE_SIZE);
            Point bottomRight = new Point(SHAPE_SIZE,SHAPE_SIZE);
            Line topLine = new Line();
            Line rightLine = new Line();
            Line bottomLine = new Line();
            Line leftLine = new Line();
            streamGeometryContext.BeginFigure(topLeft, true, false);
            topLine.X1 = topLeft.X;
            topLine.X2 = topRight.X;
            topLine.Y1 = topLeft.Y;
            topLine.Y2 = topRight.Y;
            rightLine.X1 = topRight.X;
            rightLine.X2 = topRight.X;
            rightLine.Y1 = topRight.Y;
            rightLine.Y2 = bottomRight.Y;
            bottomLine.X1 = bottomLeft.X;
            bottomLine.X2 = bottomRight.X;
            bottomLine.Y1 = bottomLeft.Y;
            bottomLine.Y2 = bottomLeft.Y;
            leftLine.X1 = topLeft.X;
            leftLine.X2 = topLeft.X;
            leftLine.Y1 = topLeft.Y;
            leftLine.Y2 = bottomLeft.Y;
            //streamGeometryContext.LineTo(topLeft, true, false);
            //streamGeometryContext.LineTo(topRight,true,false);
        }
    }
}
