using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Han_Obj_Viewer
{
    public class GeometryRoot : Dictionary<string, GeometryObject>
    {}

    public class GeometryObject
    {
        public Points Points;
        public Edges Edges;
        public Triangles Triangles;

        public Transform Transform;


        public GeometryObject()
        {
            Points = new Points();
            Edges = new Edges();
            Triangles = new Triangles();
            Triangles.Points = Points;
            Triangles.Edges = Edges;
            Transform = new Transform();
        }

        public void Show(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.5f, 0.5f, 0.5f);
            foreach (Triangle triangle in Triangles)
            {
                gl.Vertex(triangle.P0.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P1.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P2.XYZ.Transform(Transform).ToArray());
            }
            gl.End();
        }

        public void Show(OpenGL gl, float[] color)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(color[0], color[1], color[2]);
            foreach (Triangle triangle in Triangles)
            {
                gl.Vertex(triangle.P0.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P1.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P2.XYZ.Transform(Transform).ToArray());
            }
            gl.End();
        }

        public void ShowEdge(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0, 0, 0);
            foreach (Edge edge in Edges.Values)
            {
                gl.Vertex(edge.P0.XYZ.Transform(Transform).ToArray());
                gl.Vertex(edge.P1.XYZ.Transform(Transform).ToArray());
            }
            gl.End();
        }

        public void ShowPointColorMap(OpenGL gl, ColorMap colorMap)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (Triangle triangle in Triangles)
            {
                gl.Color(colorMap[triangle.P0.Id]);
                gl.Vertex(triangle.P0.XYZ.Transform(Transform).ToArray());
                gl.Color(colorMap[triangle.P1.Id]);
                gl.Vertex(triangle.P1.XYZ.Transform(Transform).ToArray());
                gl.Color(colorMap[triangle.P2.Id]);
                gl.Vertex(triangle.P2.XYZ.Transform(Transform).ToArray());
            }
            gl.End();
        }

        public void ShowFaceColorMap(OpenGL gl, ColorMap colorMap)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (Triangle triangle in Triangles)
            {
                gl.Color(colorMap[triangle.Id]);
                gl.Vertex(triangle.P0.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P1.XYZ.Transform(Transform).ToArray());
                gl.Vertex(triangle.P2.XYZ.Transform(Transform).ToArray());
            }
            gl.End();
        }

        internal void DrawMarkedPoints(OpenGL gl, List<int> MarkedPoints)
        {
            if (MarkedPoints.Count > 0)
            {
                PointDraw3D P;
                gl.Begin(OpenGL.GL_TRIANGLES);
                gl.Color(1.0f, 0f, 0);
                if (MarkedPoints[0] >= 0)
                {
                    P = new PointDraw3D(Points[MarkedPoints[0]].XYZ);
                    P.Draw(gl);
                }
                gl.Color(1.0f, 1.0f, 0);
                for (int i = 1; i < MarkedPoints.Count; i++)
                {
                    P = new PointDraw3D(Points[MarkedPoints[i]].XYZ);
                    P.Draw(gl);
                }
                gl.End();
            }
        }

        //public double[][] ToDataArrays()
        //{
        //    double[][] data = new double[Points.Count][];
        //    int i = 0;
        //    foreach (Point P in Points)
        //    {
        //        data[i] = new double[3];
        //        data[i][0] = P.XYZ.X;
        //        data[i][1] = P.XYZ.Y;
        //        data[i][2] = P.XYZ.Z;
        //        i++;
        //    }
        //    return data;
        //}


        // 4 rows, NSamples columns
        public DenseMatrix ToSampledDataMat(int NSamples)
        {
            DenseMatrix mat = new DenseMatrix(4, NSamples);
            Random rnd = new Random();
            int[] rec = new int[Points.Count];

            int index = 0;

            for (int i = 0; i < NSamples; i++)
            {
                while (true)
                {
                    index = rnd.Next(Points.Count);
                    if (rec[index] == 0)
                    {
                        rec[index] = 1;
                        break;
                    }
                }

                mat[0, i] = Points[index].XYZ.X;
                mat[1, i] = Points[index].XYZ.Y;
                mat[2, i] = Points[index].XYZ.Z;
                mat[3, i] = 1;
            }
            return mat;
        }

        // 4 rows, NPoint columns
        public DenseMatrix ToDataMat()
        {
            DenseMatrix mat = new DenseMatrix(4, Points.Count);
            Random rnd = new Random();
            int[] rec = new int[Points.Count];

            for (int i = 0; i < Points.Count; i++)
            {
                mat[0, i] = Points[i].XYZ.X;
                mat[1, i] = Points[i].XYZ.Y;
                mat[2, i] = Points[i].XYZ.Z;
                mat[3, i] = 1;
            }
            return mat;
        }


    }


}
