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
        private FindClustersAlgorithm findClustersObj;
        private FindClustersAlgorithm3D findClustersObj3D;

        private List<int> percolationClusters;
        private double mousePositionX = 0, mousePositionY = 0;

        public Color CubeColor { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            clearCubeButton.IsEnabled = false;
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {           
            canvasField.Children.Clear();
            fillButton.IsEnabled = true;
            clearButton.IsEnabled = true;

            // TODO: validate input

            int gridSize = Convert.ToInt32(this.gridSize.Text);
            double probability = Convert.ToDouble(this.probability.Text);

            findClustersObj = new FindClustersAlgorithm(gridSize, probability);
            findClustersObj.HoshenKopelmanAlgorithm();
            int totalClusters = findClustersObj.RelabledGrid();
            this.percolationClusters = findClustersObj.FindPercolationClusters();

            SolidColorBrush rectBrush = new SolidColorBrush(Colors.Black);

            int[,] grid = this.findClustersObj.grid;

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
            SolidColorBrush rectBrush = new SolidColorBrush(Colors.CornflowerBlue);

            int[,] grid = this.findClustersObj.grid;

            int squareSizeX = (int)(this.canvasField.Width / grid.GetLength(0));
            int squareSizeY = (int)(this.canvasField.Height / grid.GetLength(1));

            int filledRowCells = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                filledRowCells = 0;
                for (int j = 0; j < grid.GetLength(1); j++)
                    for (int k = 0; k < grid.GetLength(1); k++)
                        if (grid[0, k] != 0)
                            if (grid[i, j] == grid[0, k])
                            {
                                filledRowCells++;
                                Rectangle cellRect = new Rectangle();
                                cellRect.Width = squareSizeY - 1;
                                cellRect.Height = squareSizeX - 1;
                                cellRect.Fill = rectBrush;
                                cellRect.Name = "box" + i.ToString() + j.ToString();

                                canvasField.Children.Add(cellRect);

                                Canvas.SetLeft(cellRect, j * squareSizeX);
                                Canvas.SetBottom(cellRect, i * squareSizeY);         
                            }
                //System.Threading.Thread.Sleep(50);

                /**
                 * if entire row was not filled - stop filling
                 */
                if (filledRowCells == 0)
                    break;
            }          
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            canvasField.Children.Clear();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }

        private void drawCubeButton_Click(object sender, RoutedEventArgs e)
        {
            RenderCube();
        }

        private void clearCubeButton_Click(object sender, RoutedEventArgs e)
        {
            clearCubeButton.IsEnabled = false;
            viewportField.Children.Clear();
        }

        public void RenderCube()
        {
            clearCubeButton.IsEnabled = true;

            //ModelVisual3D light = new ModelVisual3D();
            //light.Content = new DirectionalLight(Colors.Transparent, new Vector3D(0, 0, 5));
            //viewportField.Children.Add(light);

            CubeColor = Color.FromRgb(210,10,10);
            CubeBuilder cubeBuilder = new CubeBuilder();
            double opacity = 1;

            int cubeSize = Convert.ToInt32(this.cubeSize.Text);
            double cubeProbability = Convert.ToDouble(this.cubeProbability.Text);

            findClustersObj3D = new FindClustersAlgorithm3D(cubeSize, cubeProbability);
            findClustersObj3D.HoshenKopelmanAlgorithm3D();
            findClustersObj3D.Relabled3DGrid();
            int[, ,] grid = this.findClustersObj3D.threeDgrid;

            int zCameraCoor = grid.GetLength(0) * 3 + 10;

            mainCamera.Position = new Point3D(0, 0, zCameraCoor);
            mainCamera.LookDirection = new Vector3D(0, 0, 0 - zCameraCoor);      

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    for (int k = 0; k < grid.GetLength(2); k++)
                    {
                        if (i == 0)
                        {
                            CubeColor = Color.FromRgb(10, 10, 210);
                        }
                        if (j == 0)
                        {
                            CubeColor = Color.FromRgb(10, 210, 10);
                        }

                        int zCenter = i * 3 - (int)(grid.GetLength(0) / 2 * 3);
                        int yCenter = j * 3 - (int)(grid.GetLength(1) / 2 * 3);
                        int xCenter = k * 3 - (int)(grid.GetLength(2) / 2 * 3);

                        opacity = (grid[i, j, k] != 0) ? 1 : 0.3;
                        viewportField.Children.Add(cubeBuilder.Create(zCenter, yCenter, xCenter, CubeColor, opacity));
                        
                        CubeColor = Color.FromRgb(210,10,10);

                        //MessageBox.Show(i.ToString() + "," + j.ToString() + "," + k.ToString(), "Yo");
                    }             

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            int xDirection, yDirection;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double deltaXDirection = this.mousePositionX - e.GetPosition(this).X;
                xDirection = deltaXDirection > 0 ? 1 : -1;

                switch (xDirection)
                {
                    case 1:
                        {
                            rotY.Angle += 1;
                            break;
                        }
                    case -1:
                        {
                            rotY.Angle -= 1;
                            break;
                        }                     
                }
                this.mousePositionX = e.GetPosition(this).X;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                double deltaYDirection = this.mousePositionY - e.GetPosition(this).Y;
                yDirection = deltaYDirection > 0 ? 1 : -1;

                switch (yDirection)
                {
                    case 1:
                        {
                            rotX.Angle += 1;
                            break;
                        }
                    case -1:
                        {
                            rotX.Angle -= 1;
                            break;
                        }
                }
                this.mousePositionY = e.GetPosition(this).Y;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mainCamera.Position = new Point3D(
                mainCamera.Position.X,
                mainCamera.Position.Y,
                mainCamera.Position.Z - e.Delta / 150D);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mousePositionX = e.GetPosition(this).X;        
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mousePositionY = e.GetPosition(this).Y;
        }

    }
}
