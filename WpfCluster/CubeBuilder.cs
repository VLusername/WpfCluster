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
        public ModelBuilder()
        {
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Color color, double opacity = 1)
        {
            MeshGeometry3D triangleMesh = new MeshGeometry3D();
            triangleMesh.Positions.Add(p0);
            triangleMesh.Positions.Add(p1);
            triangleMesh.Positions.Add(p2);
            triangleMesh.TriangleIndices.Add(0);
            triangleMesh.TriangleIndices.Add(2);
            triangleMesh.TriangleIndices.Add(1);

            Vector3D normal = VectorHelper.CalcNormal(p0, p1, p2);
            triangleMesh.Normals.Add(normal);
            triangleMesh.Normals.Add(normal);
            triangleMesh.Normals.Add(normal);

            SolidColorBrush brush = new SolidColorBrush(color);
            brush.Opacity = opacity;
            Material material = new DiffuseMaterial(brush);

            GeometryModel3D model = new GeometryModel3D(triangleMesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }
    }

    public class CubeBuilder : ModelBuilder
    {
        public int cubeSize;

        public CubeBuilder(int size)
        {
            this.cubeSize = size;
        }

        public void Create(ref Model3DGroup cubeModelGroup, int x, int y, int z, Color color, double opacity)
        {
            Point3D p0 = new Point3D(0 + x, 0 + y, 0 + z);
            Point3D p1 = new Point3D(this.cubeSize + x, 0 + y, 0 + z);
            Point3D p2 = new Point3D(this.cubeSize + x, 0 + y, this.cubeSize + z);
            Point3D p3 = new Point3D(0 + x, 0 + y, this.cubeSize + z);
            Point3D p4 = new Point3D(0 + x, this.cubeSize + y, 0 + z);
            Point3D p5 = new Point3D(this.cubeSize + x, this.cubeSize + y, 0 + z);
            Point3D p6 = new Point3D(this.cubeSize + x, this.cubeSize + y, this.cubeSize + z);
            Point3D p7 = new Point3D(0 + x, this.cubeSize + y, this.cubeSize + z);

            // front cube side
            cubeModelGroup.Children.Add(CreateTriangle(p3, p2, p6, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p3, p6, p7, color, opacity));
            //back cube side
            cubeModelGroup.Children.Add(CreateTriangle(p1, p0, p4, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p1, p4, p5, color, opacity));

            //left cube side
            cubeModelGroup.Children.Add(CreateTriangle(p0, p3, p7, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p0, p7, p4, color, opacity));
            //right cube side
            cubeModelGroup.Children.Add(CreateTriangle(p2, p1, p5, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p2, p5, p6, color, opacity));

            //top cube side
            cubeModelGroup.Children.Add(CreateTriangle(p7, p6, p5, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p7, p5, p4, color, opacity));
            //bottom cube side
            cubeModelGroup.Children.Add(CreateTriangle(p2, p3, p0, color, opacity));
            cubeModelGroup.Children.Add(CreateTriangle(p2, p0, p1, color, opacity));
        }
    }
}
