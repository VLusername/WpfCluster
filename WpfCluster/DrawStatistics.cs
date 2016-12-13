using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfCluster
{
    public class DrawStatistics
    {
        private static int margin = 50;
        private static int stepsCount = 6;
        private static int markSize = 10;

        public DrawStatistics()
        { 
        }

        public void DrawCoordinates(Canvas canvas)
        {
            canvas.Children.Clear();

            GeometryGroup linesGroup = new GeometryGroup();          

            Point xLineStart = new Point(0, canvas.Height - margin);
            Point xLineEnd = new Point(canvas.Width, canvas.Height - margin);           
            linesGroup.Children.Add(new LineGeometry(xLineStart, xLineEnd));

            int xStepSize = (int)canvas.Width / stepsCount;
            for (int xCoord = margin + xStepSize; xCoord < (int)canvas.Width; xCoord += xStepSize)
            {
                Point xMarkStart = new Point(xCoord, canvas.Height - margin);
                Point xMarkEnd = new Point(xCoord, canvas.Height - margin + markSize);
                linesGroup.Children.Add(new LineGeometry(xMarkStart, xMarkEnd));
            }

            Point yLineStart = new Point(margin, 0);
            Point yLineEnd = new Point(margin, canvas.Height);
            linesGroup.Children.Add(new LineGeometry(yLineStart, yLineEnd));

            int yStepSize = (int)canvas.Height / stepsCount;
            for (int yCoord = (int)canvas.Height - margin - yStepSize; yCoord > 0; yCoord -= yStepSize)
            {
                Point yMarkStart = new Point(0 + margin - markSize, yCoord);
                Point yMarkEnd = new Point(0 + margin, yCoord);
                linesGroup.Children.Add(new LineGeometry(yMarkStart, yMarkEnd));
            }

            Path linePath = new Path();
            linePath.StrokeThickness = 3;
            linePath.Stroke = Brushes.Black;
            linePath.Data = linesGroup;

            canvas.Children.Add(linePath);
        }
    }
}
