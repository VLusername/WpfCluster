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
        private FindClustersAlgorithm findObj;
        private List<int> percolationClusters;
        private double mousePositionX = 0, mousePositionY = 0;

        public Color CubeColor { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            fillButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {
            RenderCube();
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
            viewportField.Children.Clear();
        }

        public void RenderCube()
        {
            CubeColor = Color.FromRgb(210,10,10);

            CubeBuilder cubeBuilder = new CubeBuilder(CubeColor);
            double opacity = 1;
            Random rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        opacity = (rand.NextDouble() < 0.4) ? 1 : 0.3;
                        viewportField.Children.Add(cubeBuilder.Create(i * 3, j * 3, k * 3, opacity));
                    }             
                }
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
                //            mCamera.Position = new Point3D(
                //mCamera.Position.X - deltaXDirection / 10D,
                //mCamera.Position.Y,
                //mCamera.Position.Z);
                            rotY.Angle += 1;
                            break;
                        }
                    case -1:
                        {
                //            mCamera.Position = new Point3D(
                //mCamera.Position.X - deltaXDirection / 10D,
                //mCamera.Position.Y,
                //mCamera.Position.Z);
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
               //             mCamera.Position = new Point3D(
               //mCamera.Position.X,
               //mCamera.Position.Y - deltaYDirection / 10D,
               //mCamera.Position.Z);
                            rotX.Angle += 1;
                            break;
                        }
                    case -1:
                        {
               //             mCamera.Position = new Point3D(
               //mCamera.Position.X,
               //mCamera.Position.Y - deltaYDirection / 10D,
               //mCamera.Position.Z);
                            rotX.Angle -= 1;
                            break;
                        }
                }
                this.mousePositionY = e.GetPosition(this).Y;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mCamera.Position = new Point3D(
                mCamera.Position.X,
                mCamera.Position.Y,
                mCamera.Position.Z - e.Delta / 150D);
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
