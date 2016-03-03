using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public static class DataParser
    {
        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }

    public class XYZ
    {
        public double X;
        public double Y;
        public double Z;
        public XYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static XYZ GetXProduct(XYZ p1, XYZ p2)
        {
            double x = p1.Y * p2.Z - p1.Z * p2.Y;
            double y = p1.Z * p2.X - p1.X * p2.Z;
            double z = p1.X * p2.Y - p1.Y * p2.X;
            return new XYZ(x, y, z);
        }

        public void Scale(double s)
        {
            this.X = this.X * s;
            this.Y = this.Y * s;
            this.Z = this.Z * s;
        }

        public static double GetDotProduct(XYZ p1, XYZ p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        public XYZ Normalize()
        {
            double d = this.GetLength();
            if (d > 0)
            {
                double s = 1 / d;
                this.Scale(s);
            }
            return this;
        }

        public double GetLength()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static XYZ Minus(XYZ p1, XYZ p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            double z = p1.Z - p2.Z;
            XYZ p = new XYZ(x, y, z);
            return p;
        }

        public static XYZ GetNormal(XYZ p0, XYZ p1, XYZ p2)
        {
            XYZ a1 = Minus(p1, p0);
            XYZ a2 = Minus(p2, p0);
            return GetXProduct(a1, a2).Normalize();
        }

        public double[] ToArray()
        {
            double[] array = new double[3];
            array[0] = X;
            array[1] = Y;
            array[2] = Z;
            return array;
        }
    }

    public class Point
    {
        public int Id;
        public XYZ XYZ;
        public List<Edge> Edges;
        public Point(int id, XYZ p)
        {
            XYZ = p;
            Id = id;
            Edges = new List<Edge>();
        }
    }

    public class Edge
    {
        public int Id;
        public Point P0;
        public Point P1;
        public List<Triangle> Triangles;
        public Edge(int id, Point p0, Point p1)
        {
            Id = id;
            if (p0.Id < p1.Id)
            {
                P0 = p0;
                P1 = p1;
            }
            else
            {
                P1 = p0;
                P0 = p1;
            }
            Triangles = new List<Triangle>();
        }
    }

    public class Triangle
    {
        public int Id;
        public Point P0;
        public Point P1;
        public Point P2;
        public Edge E0;//01
        public Edge E1;//12
        public Edge E2;//02
        public XYZ Normal()
        {
            return XYZ.GetNormal(P0.XYZ, P1.XYZ, P2.XYZ);
        }
        public Triangle(int id)
        {
            Id = id;
        }
    }

    public class Points : List<Point>
    {
        public Point Insert(double x, double y, double z)
        {
            XYZ xyz = new XYZ(x, y, z);
            Point p = new Point(this.Count + 1, xyz);
            this.Add(p);
            return p;
        }
    }

    public class Edges : Dictionary<string, Edge>
    {
        public Edge Insert(Point p0, Point p1)
        {
            Point P0, P1;
            if (p0.Id < p1.Id)
            {
                P0 = p0;
                P1 = p1;
            }
            else
            {
                P1 = p0;
                P0 = p1;
            }
            string index = P0.Id.ToString() + "," + P1.Id.ToString();
            Edge edge;
            if (this.ContainsKey(index))
            {
                edge = this[index];
            }
            else
            {
                edge = new Edge(this.Count + 1, P0, P1);
                this.Add(index, edge);
            }
            P0.Edges.Add(edge);
            P1.Edges.Add(edge);
            return edge;
        }
    }

    public class Triangles : List<Triangle>
    {
        public Points Points;
        public Edges Edges;
        public Triangle Insert(int p0, int p1, int p2)
        {
            try
            {
                Point P0 = Points[p0];
                Point P1 = Points[p1];
                Point P2 = Points[p2];
                Edge E0 = Edges.Insert(P0, P1);
                Edge E1 = Edges.Insert(P1, P2);
                Edge E2 = Edges.Insert(P2, P0);

                Triangle triangle = new Triangle(this.Count + 1);
                triangle.P0 = P0;
                triangle.P1 = P1;
                triangle.P2 = P2;
                triangle.E0 = E0;
                triangle.E1 = E1;
                triangle.E2 = E2;
                //triangle.Normal = XYZ.GetNormal(P0.XYZ, P1.XYZ, P2.XYZ);

                E0.Triangles.Add(triangle);
                E1.Triangles.Add(triangle);
                E2.Triangles.Add(triangle);

                this.Add(triangle);
                return triangle;
            }
            catch
            {
                return null;
            }
            
        }

    }
}
