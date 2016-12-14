﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfCluster
{
    /// <summary>
    /// Class for drawing 3D cluster (cube)
    /// </summary>
    public class DrawCluster3D : FindClustersAlgorithm3D
    {
        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="size">Size of cube (3d-matrix)</param>
        /// <param name="probability">Probabilti which used in random filling cells (0 or 1) at drawing start</param>      
        public DrawCluster3D(int size, double probability)
            : base(size, probability)
        {
        }

        /// <summary>
        /// Cube color property
        /// </summary>
        public Color CubeColor { get; set; }

        /// <summary>
        /// Draw cube on viewport3D object
        /// </summary>
        /// <param name="viewportField">Viewport3D object for drawing shapes</param>
        /// <param name="mainCamera">PerspectiveCamera object for setting camera coordinates</param>
        /// <param name="cubeCellSize">Size of ONE cell if cube (for calculating distanse from main cube)</param>
        /// <param name="cubeModel3D">ModelVisual3D object for drawing cube</param>
        public void DrawCube(Viewport3D viewportField, PerspectiveCamera mainCamera, int cubeCellSize, ref ModelVisual3D cubeModel3D)
        {
            // calculate matrix before drawing
            HoshenKopelmanAlgorithm3D();

            double opacity = 0.2;            

            CubeColor = Color.FromRgb(210, 10, 10);
            CubeBuilder cubeBuilder = new CubeBuilder(cubeCellSize);
            Model3DGroup cubeModelGroup = new Model3DGroup();
            cubeCellSize++;

            for (int i = 0; i < grid3D.GetLength(0); i++)
                for (int j = 0; j < grid3D.GetLength(1); j++)
                    for (int k = 0; k < grid3D.GetLength(2); k++)
                    {
                        int zCenter = i * cubeCellSize - (int)((grid3D.GetLength(0) / 2) * cubeCellSize);
                        int yCenter = j * cubeCellSize - (int)((grid3D.GetLength(1) / 2) * cubeCellSize);
                        int xCenter = k * cubeCellSize - (int)((grid3D.GetLength(2) / 2) * cubeCellSize);

                        cubeBuilder.Create(ref cubeModelGroup, xCenter, -yCenter, zCenter, CubeColor, opacity);

                        //MessageBox.Show(i.ToString() + "," + j.ToString() + "," + k.ToString(), "Yo");
                    }
            cubeModel3D.Content = cubeModelGroup;

            this.SetLight(viewportField);
            this.SetCamera(mainCamera, cubeCellSize);
        }

        /// <summary>
        /// Fill existing cube (change brush opacity) to show percolation effect
        /// </summary>
        /// <param name="viewportField">Viewport3D object for drawing shapes</param>
        /// <param name="mainCamera">PerspectiveCamera object for setting camera coordinates</param>
        /// <param name="cubeCellSize">Size of ONE cell if cube</param>
        /// <param name="cubeModel3D">ModelVisual3D object for drawing cube</param>
        public void FillCube(Viewport3D viewportField, PerspectiveCamera mainCamera, int cubeCellSize, ref ModelVisual3D cubeModel3D)
        {
            // enumerator, which supports a simple iteration over a collection of a specified type
            //IEnumerable<ModelVisual3D> cubes = viewportField.Children.OfType<ModelVisual3D>();

            cubeModel3D.Children.Clear();

            double opacity;

            CubeColor = Color.FromRgb(210, 10, 10);
            CubeBuilder cubeBuilder = new CubeBuilder(cubeCellSize);
            Model3DGroup cubeModelGroup = new Model3DGroup();
            cubeCellSize++;

            for (int i = 0; i < grid3D.GetLength(0); i++)
                for (int j = 0; j < grid3D.GetLength(1); j++)
                    for (int k = 0; k < grid3D.GetLength(2); k++)
                    {
                        int zCenter = i * cubeCellSize - (int)((grid3D.GetLength(0) / 2) * cubeCellSize);
                        int yCenter = j * cubeCellSize - (int)((grid3D.GetLength(1) / 2) * cubeCellSize);
                        int xCenter = k * cubeCellSize - (int)((grid3D.GetLength(2) / 2) * cubeCellSize);

                        // display any cluster
                        opacity = (grid3D[i, j, k] != 0) ? 0.7 : 0.2;

                        // change color of percolation clusters
                        CubeColor = (grid3D[i, j, k] != 0 && foundClusters3D.Contains(grid3D[i, j, k])) ? Color.FromRgb(10, 10, 210) : Color.FromRgb(210, 10, 10);

                        // redraw cube parts
                        cubeBuilder.Create(ref cubeModelGroup, xCenter, -yCenter, zCenter, CubeColor, opacity);  

                        //MessageBox.Show(i.ToString() + "," + j.ToString() + "," + k.ToString(), "Yo");
                    }
            cubeModel3D.Content = cubeModelGroup;

            this.SetLight(viewportField);
            this.SetCamera(mainCamera, cubeCellSize);
        }

        /// <summary>
        /// Set lightning effects
        /// </summary>
        /// <param name="viewportField">Viewport3D object for adding light objects</param>
        private void SetLight(Viewport3D viewportField)
        {
        }

        /// <summary>
        /// Set camera coordinates
        /// </summary>
        /// <param name="mainCamera">PerspectiveCamera object that have to would be changed</param>
        /// <param name="cubeSize">Size of ONE cell if cube (for calculating distanse from main cube)</param>
        private void SetCamera(PerspectiveCamera mainCamera, int cubeSize)
        {
            int xCameraCoor = grid3D.GetLength(0) * cubeSize + 10;
            int yCameraCoor = (grid3D.GetLength(1) / 2) * cubeSize;

            mainCamera.Position = new Point3D(-xCameraCoor, yCameraCoor, 0);
            mainCamera.LookDirection = new Vector3D(0 + xCameraCoor, 0 - yCameraCoor, 0);
        }
    }
}
