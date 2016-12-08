using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfCluster
{
    public class VectorHelper
    {
        public static Vector3D CalcNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }
    }

    public class ModelBuilder
    {
        private Color _color;

        public ModelBuilder(Color color)
        {
            _color = color;
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2, double opacity = 1)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            Vector3D normal = VectorHelper.CalcNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            SolidColorBrush brush = new SolidColorBrush(_color);
            brush.Opacity = opacity;
            Material material = new DiffuseMaterial(brush);
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }
    }

    public class CubeBuilder : ModelBuilder
    {
        public CubeBuilder(Color color) : base(color)
        {
        }

        public ModelVisual3D Create()
        {
            return Create(0, 0, 0, 1);
        }

        public ModelVisual3D Create(int x, int y, int z, double opacity)
        {
            Model3DGroup cube = new Model3DGroup();

            Point3D p0 = new Point3D(0 + x, 0 + y, 0 + z);
            Point3D p1 = new Point3D(2 + x, 0 + y, 0 + z);
            Point3D p2 = new Point3D(2 + x, 0 + y, 2 + z);
            Point3D p3 = new Point3D(0 + x, 0 + y, 2 + z);
            Point3D p4 = new Point3D(0 + x, 2 + y, 0 + z);
            Point3D p5 = new Point3D(2 + x, 2 + y, 0 + z);
            Point3D p6 = new Point3D(2 + x, 2 + y, 2 + z);
            Point3D p7 = new Point3D(0 + x, 2 + y, 2 + z);

            //front
            cube.Children.Add(CreateTriangle(p3, p2, p6, opacity));
            cube.Children.Add(CreateTriangle(p3, p6, p7, opacity));

            //right
            cube.Children.Add(CreateTriangle(p2, p1, p5, opacity));
            cube.Children.Add(CreateTriangle(p2, p5, p6, opacity));

            //back
            cube.Children.Add(CreateTriangle(p1, p0, p4, opacity));
            cube.Children.Add(CreateTriangle(p1, p4, p5, opacity));

            //left
            cube.Children.Add(CreateTriangle(p0, p3, p7, opacity));
            cube.Children.Add(CreateTriangle(p0, p7, p4, opacity));

            //top
            cube.Children.Add(CreateTriangle(p7, p6, p5, opacity));
            cube.Children.Add(CreateTriangle(p7, p5, p4, opacity));

            //bottom
            cube.Children.Add(CreateTriangle(p2, p3, p0, opacity));
            cube.Children.Add(CreateTriangle(p2, p0, p1, opacity));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = cube;
            return model;
        }
    }
}
