using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public class GeometryRoot : Dictionary<string, GeometryObject>
    {}

    public class GeometryObject
    {
        public Points Points;
        public Edges Edges;
        public Triangles Triangles;

        public GeometryObject()
        {
            Points = new Points();
            Edges = new Edges();
            Triangles = new Triangles();
            Triangles.Points = Points;
            Triangles.Edges = Edges;
        }

        public void Show(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.5f, 0.5f, 0.5f);
            foreach (Triangle triangle in Triangles)
            {
                gl.Vertex(triangle.P0.XYZ.ToArray());
                gl.Vertex(triangle.P1.XYZ.ToArray());
                gl.Vertex(triangle.P2.XYZ.ToArray());
            }
            gl.End();
        }

        public void ShowEdge(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0, 0, 0);
            foreach (Edge edge in Edges.Values)
            {
                gl.Vertex(edge.P0.XYZ.ToArray());
                gl.Vertex(edge.P1.XYZ.ToArray());
            }
            gl.End();
        }

        public void ShowPointColorMap(OpenGL gl, ColorMap colorMap)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (Triangle triangle in Triangles)
            {
                gl.Color(colorMap[triangle.P0.Id]);
                gl.Vertex(triangle.P0.XYZ.ToArray());
                gl.Color(colorMap[triangle.P1.Id]);
                gl.Vertex(triangle.P1.XYZ.ToArray());
                gl.Color(colorMap[triangle.P2.Id]);
                gl.Vertex(triangle.P2.XYZ.ToArray());
            }
            gl.End();
        }

        public void ShowFaceColorMap(OpenGL gl, ColorMap colorMap)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (Triangle triangle in Triangles)
            {
                gl.Color(colorMap[triangle.Id]);
                gl.Vertex(triangle.P0.XYZ.ToArray());
                gl.Vertex(triangle.P1.XYZ.ToArray());
                gl.Vertex(triangle.P2.XYZ.ToArray());
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
    }


}
