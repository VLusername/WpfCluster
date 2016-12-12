using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfCluster
{
    public class DrawCluster : FindClustersAlgorithm
    {
        public DrawCluster(int size, double probability)
            : base(size, probability)
        {
        }

        public void DrawGrid(Canvas canvasField)
        {
            // calculate matrix before drawing
            HoshenKopelmanAlgorithm();

            SolidColorBrush rectBrush = new SolidColorBrush(Colors.Black);

            int squareSizeX = (int)(canvasField.Width / grid.GetLength(0));
            int squareSizeY = (int)(canvasField.Height / grid.GetLength(1));

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != 0)
                        rectBrush = new SolidColorBrush(Colors.White);

                    Rectangle cellRect = new Rectangle();
                    cellRect.Width = squareSizeY - 1;
                    cellRect.Height = squareSizeX - 1;
                    cellRect.Fill = rectBrush;
                    cellRect.Name = "box" + i.ToString() + j.ToString();

                    canvasField.Children.Add(cellRect);

                    Canvas.SetLeft(cellRect, j * squareSizeX);
                    Canvas.SetTop(cellRect, i * squareSizeY);

                    rectBrush = new SolidColorBrush(Colors.Black);
                }
        }

        public void FillGrid(Canvas canvasField)
        {
            // enumerator, which supports a simple iteration over a collection of a specified type
            IEnumerable<Rectangle> rectangles = canvasField.Children.OfType<Rectangle>();
         
            int squareSizeX = (int)(canvasField.Width / grid.GetLength(0));
            int squareSizeY = (int)(canvasField.Height / grid.GetLength(1));

            // the list of clusters that start in first row
            List<int> firstRowClusters = new List<int>();
            for (int i = 0; i < grid.GetLength(0); i++)
                if (grid[0, i] != 0 && !firstRowClusters.Contains(grid[0, i]))
                    firstRowClusters.Add(grid[0, i]);

            ColorAnimation colorAnimation = new ColorAnimation(
                Colors.White,
                Colors.CornflowerBlue,
                TimeSpan.FromMilliseconds(1000));

            SolidColorBrush rectBrush = new SolidColorBrush(Colors.CornflowerBlue);

            int filledRowCells = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                filledRowCells = 0;
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != 0 && firstRowClusters.Contains(grid[i, j]))
                    {
                        // fill only that cluster which start on first row
                        filledRowCells++;
                        Rectangle rect = rectangles.ElementAt(j + i * grid.GetLength(0));
                        rect.Fill = rectBrush;
                        rectBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                    }
                }           
                // if entire row was not filled - stop filling. Percolation does not exist
                if (filledRowCells == 0)
                    break;
            }     
        }
    }
}
