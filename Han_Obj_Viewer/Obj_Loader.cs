using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpGL;

namespace Han_Obj_Viewer
{
    public class Model_Loader
    {
        public string Path;
        public Points Points;
        public Edges Edges;
        public Triangles Triangles;

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
            gl.Color(1.0f, 1.0f, 1.0f);
            foreach (Edge edge in Edges.Values)
            {
                gl.Vertex(edge.P0.XYZ.ToArray());
                gl.Vertex(edge.P1.XYZ.ToArray());
            }
            gl.End();
        }

        public void ShowColorMap(OpenGL gl, ColorMap colorMap)
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

    }

    public class Obj_Loader : Model_Loader
    {
        public Obj_Loader(string path)
        {
            Path = path;
            Points = new Points();
            Edges = new Edges();
            Triangles = new Triangles();
            Triangles.Points = Points;
            Triangles.Edges = Edges;
            Load();
        }

        void Load()
        {
            StreamReader reader = new StreamReader(Path);
            while (!reader.EndOfStream)
            {
                try{
                    string line = reader.ReadLine();
                    line.Trim();
                    string[] tokens = line.Split(' ');
                    if (tokens[0] == "v")
                    {
                        if(tokens.Length==4)
                        {
                            Points.Insert(tokens[1].ToDouble(), tokens[2].ToDouble(), tokens[3].ToDouble());
                        }
                    }
                    else if (tokens[0] == "f")
                    {
                        if (tokens.Length == 4)
                        {
                            Triangles.Insert(tokens[1].ToInt(), tokens[2].ToInt(), tokens[3].ToInt());
                        }
                    }
                    else
                    {
                    }
                }catch{
                }
            }
        }
    }

    public class Off_Loader : Model_Loader
    {
        public Off_Loader(string path)
        {
            Path = path;
            Points = new Points();
            Edges = new Edges();
            Triangles = new Triangles();
            Triangles.Points = Points;
            Triangles.Edges = Edges;
            Load();
        }

        void Load()
        {
            StreamReader reader = new StreamReader(Path);
            reader.ReadLine();
            string[] meta = reader.ReadLine().Trim().Split(' ');
            int VCount = meta[0].ToInt();
            int FCount = meta[1].ToInt();
            for (int i = 0; i < VCount; i++)
            {
                try
                {
                    string[] tokens = reader.ReadLine().Trim().Split(' ');
                    if (tokens.Length == 3)
                    {
                        Points.Insert(tokens[0].ToDouble(), tokens[1].ToDouble(), tokens[2].ToDouble());
                    }
                }
                catch
                {
                }
            }
            for (int i = 0; i < FCount; i++)
            {
                try
                {
                    string[] tokens = reader.ReadLine().Trim().Split(' ');
                    if (tokens.Length == 4 && tokens[0] == "3")
                    {
                        Triangles.Insert(tokens[1].ToInt() + 1, tokens[2].ToInt() + 1, tokens[3].ToInt() + 1);
                    }
                }
                catch
                {
                }
            }
        }
    }

}
