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

namespace WpfCluster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FindClustersAlgorithm findObj;
        private List<int> percolationClusters;

        public MainWindow()
        {
            InitializeComponent();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {
            canvasField.Children.Clear();
            fillButton.IsEnabled = true;
            clearButton.IsEnabled = true;

            // TODO: validate input

            int gridSize = Convert.ToInt32(this.gridSize.Text);
            double probability = Convert.ToDouble(this.probability.Text);

            findObj = new FindClustersAlgorithm(gridSize, probability);
            findObj.HoshenKopelmanAlgorithm();
            int totalClusters = findObj.RelabledGrid();
            this.percolationClusters = findObj.FindPercolationClusters();

            SolidColorBrush rectBrush = new SolidColorBrush(Colors.Black);

            int[,] grid = this.findObj.grid;

            int squareSizeX = (int)(this.canvasField.Width / grid.GetLength(0));
            int squareSizeY = (int)(this.canvasField.Height / grid.GetLength(1));         

            for (int i = 0; i < grid.GetLength(0); i++)
            {
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
                    Canvas.SetBottom(cellRect, i * squareSizeY);

                    rectBrush = new SolidColorBrush(Colors.Black);
                }               
                if (this.slowMotion.IsChecked == true)
                    System.Threading.Thread.Sleep(50);
            } 
        }

        private void fillButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            canvasField.Children.Clear();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }

    }
}
