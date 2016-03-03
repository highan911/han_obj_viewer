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
        public GeometryObject GeometryObject;
    }

    public class Obj_Loader : Model_Loader
    {
        public Obj_Loader(string path)
        {
            Path = path;
            GeometryObject = new GeometryObject();
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
                            GeometryObject.Points.Insert(tokens[1].ToDouble(), tokens[2].ToDouble(), tokens[3].ToDouble());
                        }
                    }
                    else if (tokens[0] == "f")
                    {
                        if (tokens.Length == 4)
                        {
                            GeometryObject.Triangles.Insert(tokens[1].ToInt() - 1, tokens[2].ToInt() - 1, tokens[3].ToInt() - 1);
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
            GeometryObject = new GeometryObject();
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
                        GeometryObject.Points.Insert(tokens[0].ToDouble(), tokens[1].ToDouble(), tokens[2].ToDouble());
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
                        GeometryObject.Triangles.Insert(tokens[1].ToInt(), tokens[2].ToInt(), tokens[3].ToInt());
                    }
                }
                catch
                {
                }
            }
        }
    }

}
