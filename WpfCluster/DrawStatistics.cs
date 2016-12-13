﻿using System;
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
            
        public DrawStatistics(int operationsPerPoint)
        {
            this.operationsPerPoint = operationsPerPoint;
        }

        public void DrawCoordinates(Canvas canvas)
        {
            canvas.Children.Clear();

            GeometryGroup linesGroup = new GeometryGroup();
            TextBlock textBlock = new TextBlock();
            double yText = 0, xText = 0.4;

            Point xLineStart = new Point(0, canvas.Height - margin);
            Point xLineEnd = new Point(canvas.Width, canvas.Height - margin);           
            linesGroup.Children.Add(new LineGeometry(xLineStart, xLineEnd));

            double xStepSize = (canvas.Width - 2 * margin) / stepsCount;
            for (double xCoord = margin + xStepSize; xCoord < canvas.Width; xCoord += xStepSize)
            {
                Point xMarkStart = new Point(xCoord, canvas.Height - margin);
                Point xMarkEnd = new Point(xCoord, canvas.Height - margin + markSize);
                linesGroup.Children.Add(new LineGeometry(xMarkStart, xMarkEnd));

                xText += 0.05;
                canvas.Children.Add(this.DrawTextMark(xText.ToString(), xCoord - markSize, canvas.Height - 25));
            }
            canvas.Children.Add(this.DrawTextMark("P", canvas.Width, canvas.Height - margin));


            Point yLineStart = new Point(margin, 0);
            Point yLineEnd = new Point(margin, canvas.Height);
            linesGroup.Children.Add(new LineGeometry(yLineStart, yLineEnd));

            double yStepSize = (canvas.Height - 2 * margin) / stepsCount;
            for (double yCoord = canvas.Height - margin - yStepSize; yCoord > 0; yCoord -= yStepSize)
            {
                Point yMarkStart = new Point(0 + margin - markSize, yCoord);
                Point yMarkEnd = new Point(0 + margin, yCoord);
                linesGroup.Children.Add(new LineGeometry(yMarkStart, yMarkEnd));

                if (yText < 1)
                {
                    yText += 0.2;
                    canvas.Children.Add(this.DrawTextMark(yText.ToString(), 0, yCoord - 15));
                }
            }
            canvas.Children.Add(this.DrawTextMark("M/N", 0, 0));

            Path linePath = new Path();
            linePath.StrokeThickness = 3;
            linePath.Stroke = Brushes.Black;
            linePath.Data = linesGroup;

            canvas.Children.Add(linePath);         
        }

        public void DrawExperimentData(Canvas canvas)
        {
            this.DrawCoordinates(canvas);

            FindClustersAlgorithm findClusterObj;
            PointCollection points = new PointCollection();
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            int currentBrush = 0;

            // TODO: input scale step, remove hardcode

            double probabilityStep = 0.05;

            for (int gridSize = 30; gridSize <= 70; gridSize += 20)
            {
                points = new PointCollection();
                for (double probability = 0.4; probability < 0.8; probability += probabilityStep)
                {
                    int countPercolationClusters = 0;
                    for (int j = 0; j < this.operationsPerPoint; j++)
                    {
                        findClusterObj = new FindClustersAlgorithm(gridSize, probability);
                        findClusterObj.HoshenKopelmanAlgorithm(true);

                        if (findClusterObj.lightCheckResult)
                            countPercolationClusters++;
                    }

                    double xPixelStepSize = (canvas.Width - 2 * margin) / ((0.8 - 0.4) / probabilityStep);
                    double yPixelStepSize = (canvas.Height - 2 * margin) / 100;

                    double x = margin + ((probability - 0.4) / probabilityStep) * xPixelStepSize;
                    double y = canvas.Height - margin - yPixelStepSize * (((double)countPercolationClusters / this.operationsPerPoint) * 100);

                    points.Add(new Point(x, y));
                }
                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 3;
                polyline.Stroke = brushes[currentBrush];
                polyline.Points = points;

                canvas.Children.Add(polyline);
                currentBrush++;
            }           
        }

        private TextBlock DrawTextMark(string text, double x, double y)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 18;
            textBlock.Text = text;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);

            return textBlock;
        }
    }
}
