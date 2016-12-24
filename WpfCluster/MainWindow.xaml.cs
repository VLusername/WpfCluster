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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCluster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DrawCluster drawClusterObj;
        public DrawCluster3D drawClusterObj3D;
        public DrawStatistics drawStatObj;

        public MainWindow()
        {
            InitializeComponent();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            fillCubeButton.IsEnabled = false;
            clearCubeButton.IsEnabled = false;
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {           
            int gridSize;
            double probability;

            if (!int.TryParse(this.gridSize.Text, out gridSize) || gridSize < 2 || gridSize > 250)
            {
                MessageBox.Show(
                    "Grid size must be positive integer greater from 2 to 250!",
                    "Parse error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );             
            }
            else if (!double.TryParse(this.probability.Text, out probability) || probability < 0 || probability > 1)
            {
                MessageBox.Show(
                    "Probability must be positive double from 0 to 1!",
                    "Parse error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            else
            {
                canvasField.Children.Clear();
                fillButton.IsEnabled = true;
                clearButton.IsEnabled = true;
                string percolationClustersText = "";

                drawClusterObj = new DrawCluster(gridSize, probability);
                drawClusterObj.DrawGrid(canvasField, ref percolationClustersText, (bool)showClusterCount.IsChecked, (bool)differentColors.IsChecked);

                foundClustersLabel.Content = percolationClustersText;
            }
        }

        private void fillButton_Click(object sender, RoutedEventArgs e)
        {
            drawClusterObj.FillGrid(canvasField);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            canvasField.Children.Clear();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            foundClustersLabel.Content = "";
        }

        private void drawCubeButton_Click(object sender, RoutedEventArgs e)
        {
            int cubeSize;
            double cubeProbability;
            if (!int.TryParse(this.cubeSize.Text, out cubeSize) || cubeSize < 2 || cubeSize > 16)
            {
                MessageBox.Show(
                    "Cube size must be positive integer greater from 2 to 16!",
                    "Parse error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );             
            }
            else if (!double.TryParse(this.cubeProbability.Text, out cubeProbability) || cubeProbability < 0 || cubeProbability > 1)
            {
                MessageBox.Show(
                    "Cube probability must be positive double from 0 to 1!",
                    "Parse error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            else
            {
                fillCubeButton.IsEnabled = true;
                clearCubeButton.IsEnabled = true;
                cubeModel.Children.Clear();

                drawClusterObj3D = new DrawCluster3D(cubeSize, cubeProbability);
                drawClusterObj3D.DrawCube(viewportField, mainCamera, 5, ref cubeModel);
            }
        }

        private void fillCubeButton_Click(object sender, RoutedEventArgs e)
        {
            drawClusterObj3D.FillCube(viewportField, mainCamera, 5, ref cubeModel);
        }

        private void clearCubeButton_Click(object sender, RoutedEventArgs e)
        {
            fillCubeButton.IsEnabled = false;
            clearCubeButton.IsEnabled = false;
            cubeModel.Children.Clear();
            cubeModel.Content = null;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mainCamera.Position = new Point3D(
                mainCamera.Position.X + e.Delta / 150D,
                mainCamera.Position.Y,
                mainCamera.Position.Z);
        }

        private void drawGraphic_Click(object sender, RoutedEventArgs e)
        {
            int operationCount;
            if (!int.TryParse(this.operationCount.Text, out operationCount) || operationCount < 1 || operationCount > 150)
            {
                MessageBox.Show(
                    "Operation count must be positive integer from 1 to 150!",
                    "Parse error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            else
            {
                drawStatObj.DrawExperimentData(graphicCanvasField, operationCount);
            }         
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (secondTab.IsSelected)
            {
                drawStatObj = new DrawStatistics();
                graphicCanvasField.Children.Clear();
                drawStatObj.DrawCoordinates(graphicCanvasField);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
                rotateCubeY.Angle += 5;

            if (e.Key == Key.D)
                rotateCubeY.Angle -= 5;

            if (e.Key == Key.W)
                rotateCubeX.Angle += 5;

            if (e.Key == Key.S)
                rotateCubeX.Angle -= 5;

            if (e.Key == Key.Q)
                rotateCubeZ.Angle += 5;

            if (e.Key == Key.E)
                rotateCubeZ.Angle -= 5;
        }

    }
}
