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

        private double mousePositionX = 0, mousePositionY = 0;

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
            canvasField.Children.Clear();
            fillButton.IsEnabled = true;
            clearButton.IsEnabled = true;

            // TODO: validate input

            int gridSize = Convert.ToInt32(this.gridSize.Text);
            double probability = Convert.ToDouble(this.probability.Text);

            drawClusterObj = new DrawCluster(gridSize, probability);
            drawClusterObj.DrawGrid(canvasField);
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
        }

        private void drawCubeButton_Click(object sender, RoutedEventArgs e)
        {
            fillCubeButton.IsEnabled = true;
            clearCubeButton.IsEnabled = true;

            // TODO: input validation

            int cubeSize = Convert.ToInt32(this.cubeSize.Text);
            double cubeProbability = Convert.ToDouble(this.cubeProbability.Text);

            drawClusterObj3D = new DrawCluster3D(cubeSize, cubeProbability);
            drawClusterObj3D.DrawCube(viewportField, mainCamera, 4);
        }

        private void fillCubeButton_Click(object sender, RoutedEventArgs e)
        {
            drawClusterObj3D.FillCube(viewportField);
        }

        private void clearCubeButton_Click(object sender, RoutedEventArgs e)
        {
            fillCubeButton.IsEnabled = false;
            clearCubeButton.IsEnabled = false;
            viewportField.Children.Clear();
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
                            rotZ.Angle += 1;
                            break;
                        }
                    case -1:
                        {
                            rotZ.Angle -= 1;
                            break;
                        }
                }
                this.mousePositionY = e.GetPosition(this).Y;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mainCamera.Position = new Point3D(
                mainCamera.Position.X + e.Delta / 150D,
                mainCamera.Position.Y,
                mainCamera.Position.Z);
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
