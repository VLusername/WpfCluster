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
    /// <summary>
    /// Class for drawing 2D cluster
    /// </summary>
    public class DrawCluster : FindClustersAlgorithm
    {
        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="size">Size of square grid (matrix)</param>
        /// <param name="probability">Probabilti which used in random filling cells at drawing start</param>
        public DrawCluster(int size, double probability)
            : base(size, probability)
        {
        }

        /// <summary>
        /// Draw grid on canvas object
        /// </summary>
        /// <param name="canvas">Canvas object for drawing</param>
        /// <param name="percolationClustersText">Text about found percolation clusters</param>
        /// <param name="showText">Display or not clustres label</param>
        /// <param name="diffColors">Mark clusters by different colors or not</param>
        public void DrawGrid(Canvas canvas, ref string percolationClustersText, bool showText = false, bool diffColors = false)
        {
            // calculate matrix before drawing
            HoshenKopelmanAlgorithm();

            SolidColorBrush rectBrush = new SolidColorBrush(Colors.Black);
            Rectangle cellRect = new Rectangle();
            Random rand = new Random();
            Dictionary<int, Color> clusterColors = new Dictionary<int, Color>();

            int squareSizeX = (int)(canvas.Width / grid.GetLength(0));
            int squareSizeY = (int)(canvas.Height / grid.GetLength(1));

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != 0)
                        if (diffColors)
                        {
                            if (clusterColors.ContainsKey(grid[i, j]))
                            {
                                rectBrush = new SolidColorBrush(clusterColors[grid[i, j]]);
                            }
                            else 
                            {
                                int red = rand.Next(0, byte.MaxValue + 1);
                                int green = rand.Next(0, byte.MaxValue + 1);
                                int blue = rand.Next(0, byte.MaxValue + 1);
                                Color color = Color.FromRgb((byte)red, (byte)green, (byte)blue);

                                clusterColors.Add(grid[i, j], color);

                                rectBrush = new SolidColorBrush(color);
                            }                           
                        }
                        else 
                        {
                            rectBrush = new SolidColorBrush(Colors.WhiteSmoke);
                        }

                    cellRect = new Rectangle();
                    cellRect.Width = squareSizeY - 1;
                    cellRect.Height = squareSizeX - 1;
                    cellRect.Fill = rectBrush;
                    cellRect.Name = "box" + i.ToString() + j.ToString();
                    Canvas.SetLeft(cellRect, j * squareSizeX);
                    Canvas.SetTop(cellRect, i * squareSizeY);

                    canvas.Children.Add(cellRect);
                    rectBrush = new SolidColorBrush(Colors.Black);

                    if (showText)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.FontSize = squareSizeY / 2;
                        textBlock.Text = grid[i, j].ToString();
                        Canvas.SetLeft(textBlock, j * squareSizeX);
                        Canvas.SetTop(textBlock, i * squareSizeY);
                        canvas.Children.Add(textBlock);
                    }          
                }

            if (showText)
            {
                percolationClustersText += "Per. clusers:";
                foreach (var item in percolationClusters)
                    percolationClustersText += " #" + item.ToString();
            }
        }

        /// <summary>
        /// Fill existing grid to show percolation effect
        /// </summary>
        /// <param name="canvas">Canvas object for drawing</param>
        /// <param name="showText">Display or not clustres label</param>
        /// <param name="diffColors">Mark clusters by different colors or not</param>
        public void FillGrid(Canvas canvas)
        {
            // enumerator, which supports a simple iteration over a collection of a specified type
            IEnumerable<Rectangle> rectangles = canvas.Children.OfType<Rectangle>();
         
            int squareSizeX = (int)(canvas.Width / grid.GetLength(0));
            int squareSizeY = (int)(canvas.Height / grid.GetLength(1));

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
