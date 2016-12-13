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
        private int operationsPerPoint;
        private int gridSize;
            
        public DrawStatistics(int operationsPerPoint, int gridSize)
        {
            this.operationsPerPoint = operationsPerPoint;
            this.gridSize = gridSize;
        }

        public void DrawCoordinates(Canvas canvas)
        {
            canvas.Children.Clear();

            GeometryGroup linesGroup = new GeometryGroup();          

            Point xLineStart = new Point(0, canvas.Height - margin);
            Point xLineEnd = new Point(canvas.Width, canvas.Height - margin);           
            linesGroup.Children.Add(new LineGeometry(xLineStart, xLineEnd));

            double xStepSize = (canvas.Width - 2 * margin) / stepsCount;
            for (double xCoord = margin + xStepSize; xCoord < canvas.Width; xCoord += xStepSize)
            {
                Point xMarkStart = new Point(xCoord, canvas.Height - margin);
                Point xMarkEnd = new Point(xCoord, canvas.Height - margin + markSize);
                linesGroup.Children.Add(new LineGeometry(xMarkStart, xMarkEnd));
            }

            Point yLineStart = new Point(margin, 0);
            Point yLineEnd = new Point(margin, canvas.Height);
            linesGroup.Children.Add(new LineGeometry(yLineStart, yLineEnd));

            double yStepSize = (canvas.Height - 2 * margin) / stepsCount;
            for (double yCoord = canvas.Height - margin - yStepSize; yCoord > 0; yCoord -= yStepSize)
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

        public void DrawExperimentData(Canvas canvas)
        {
            FindClustersAlgorithm findClusterObj;
            PointCollection points = new PointCollection();
            
            // TODO: input scale step, remove hardcode

            double probabilityStep = 0.025;

            for (double probability = 0.4; probability < 0.8; probability += probabilityStep)
            {
                int countPercolationClusters = 0;
                for (int j = 0; j < this.operationsPerPoint; j++)
                {
                    findClusterObj = new FindClustersAlgorithm(this.gridSize, probability);
                    findClusterObj.HoshenKopelmanAlgorithm();

                    if (findClusterObj.percolationClusters.Count > 0)
                        countPercolationClusters++;
                }

                double xPixelStepSize = (canvas.Width - 2 * margin) / ((0.8 - 0.4) / probabilityStep);
                double yPixelStepSize = (canvas.Height - 2 * margin) / 100;

                double x = margin + ((probability - 0.4) / probabilityStep) * xPixelStepSize;
                double y = canvas.Height - margin - yPixelStepSize * (((double)countPercolationClusters / this.operationsPerPoint) * 100);
                
                points.Add(new Point(x, y));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 5;
            polyline.Stroke = Brushes.Green;
            polyline.Points = points;

            canvas.Children.Add(polyline);
        }
    }
}
