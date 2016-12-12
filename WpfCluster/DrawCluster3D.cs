using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfCluster
{
    public class DrawCluster3D : FindClustersAlgorithm3D
    {
        public DrawCluster3D(int size, double probability)
            : base(size, probability)
        {
        }

        public Color CubeColor { get; set; }

        public void DrawCube(Viewport3D viewportField, PerspectiveCamera mainCamera, int cubeSize)
        {
            // calculate matrix before drawing
            HoshenKopelmanAlgorithm3D();
           
            double opacity;            

            CubeColor = Color.FromRgb(210, 10, 10);
            CubeBuilder cubeBuilder = new CubeBuilder(cubeSize);
            cubeSize++;

            for (int i = 0; i < grid3D.GetLength(0); i++)
                for (int j = 0; j < grid3D.GetLength(1); j++)
                    for (int k = 0; k < grid3D.GetLength(2); k++)
                    {
                        //if (i == 0)
                        //{
                        //    CubeColor = Color.FromRgb(10, 10, 210);
                        //}
                        //if (j == 0)
                        //{
                        //    CubeColor = Color.FromRgb(10, 210, 10);
                        //}

                        int zCenter = i * cubeSize - (int)(grid3D.GetLength(0) / 2 * cubeSize);
                        int yCenter = j * cubeSize - (int)(grid3D.GetLength(1) / 2 * cubeSize);
                        int xCenter = k * cubeSize - (int)(grid3D.GetLength(2) / 2 * cubeSize);

                        opacity = (grid3D[i, j, k] == 3) ? 1 : 0.2;
                        viewportField.Children.Add(cubeBuilder.Create(zCenter, -yCenter, xCenter, CubeColor, opacity));

                        CubeColor = Color.FromRgb(210, 10, 10);

                        //MessageBox.Show(i.ToString() + "," + j.ToString() + "," + k.ToString(), "Yo");
                    }
            this.SetLight(viewportField);
            this.SetCamera(mainCamera, cubeSize);
        }

        public void FillCube(Viewport3D viewportField)
        {
            // enumerator, which supports a simple iteration over a collection of a specified type
            IEnumerable<Visual3D> cubes = viewportField.Children.OfType<Visual3D>();

            for (int i = 0; i < grid3D.GetLength(0); i++)
                for (int j = 0; j < grid3D.GetLength(1); j++)
                    for (int k = 0; k < grid3D.GetLength(2); k++)
                    {
                        if (grid3D[i, j, k] == 3)
                        {
                            Visual3D cube = cubes.ElementAt(k + j * grid3D.GetLength(1) + i * grid3D.GetLength(0));
                            //
                            //System.Windows.MessageBox.Show(i.ToString() + "," + j.ToString() + "," + k.ToString(), "Yo");
                        }
                    } 
        }

        private void SetLight(Viewport3D viewportField)
        {
            ModelVisual3D firstLightResourse = new ModelVisual3D();
            firstLightResourse.Content = new DirectionalLight(Colors.Transparent, new Vector3D(15, 15, 15));
            viewportField.Children.Add(firstLightResourse);

            ModelVisual3D secondLightResourse = new ModelVisual3D();
            secondLightResourse.Content = new DirectionalLight(Colors.Transparent, new Vector3D(-15, -15, -15));
            viewportField.Children.Add(secondLightResourse);
        }

        private void SetCamera(PerspectiveCamera mainCamera, int cubeSize)
        {
            int cameraCoor = grid3D.GetLength(0) * cubeSize + 10;
            mainCamera.Position = new Point3D(-cameraCoor, 0, 0);
            mainCamera.LookDirection = new Vector3D(0 + cameraCoor, 0, 0);
        }
    }
}
