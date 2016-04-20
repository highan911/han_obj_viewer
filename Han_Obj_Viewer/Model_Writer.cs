using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Han_Obj_Viewer
{
    public static class DecimalStrFromat
    {
        public static double Eps = 0.00001;
        public static string ToDecimalStr(this double val)
        {
            if ((val > 0 && val < Eps) || (val < 0 && val > -Eps)) return "0";
            string str = String.Format("{0:#.#####}", val);
            return str;
        }

    }

    public class Model_Writer
    {
        public string Path;
        public GeometryObject GeometryObject;
    }

    class Obj_Writer : Model_Writer
    {
        public Obj_Writer(string path, GeometryObject geometryObject)
        {
            Path = path;
            GeometryObject = geometryObject;
            Write();
        }
        private void Write()
        {
            Transform Trans = GeometryObject.Transform;
            Matrix dataMat = GeometryObject.ToDataMat();
            dataMat = Trans.GetMatrix() * dataMat;

            StreamWriter writer = new StreamWriter(Path);
            for (int i = 0; i < dataMat.Columns; i++)
            {
                writer.WriteLine(String.Format("v {0} {1} {2}", dataMat[0, i].ToDecimalStr(), dataMat[1, i].ToDecimalStr(), dataMat[2, i].ToDecimalStr()));
            }
            for (int i = 0; i < GeometryObject.Triangles.Count; i++)
            {
                writer.WriteLine(String.Format("f {0} {1} {2}", GeometryObject.Triangles[i].P0.Id + 1, GeometryObject.Triangles[i].P1.Id + 1, GeometryObject.Triangles[i].P2.Id + 1));
            }
            writer.Close();
        }

    }

    class Off_Writer : Model_Writer
    {
        public Off_Writer(string path, GeometryObject geometryObject)
        {
            Path = path;
            GeometryObject = geometryObject;
            Write();
        }
        private void Write()
        {
            Transform Trans = GeometryObject.Transform;
            Matrix dataMat = GeometryObject.ToDataMat();
            dataMat = Trans.GetMatrix() * dataMat;

            StreamWriter writer = new StreamWriter(Path);
            writer.WriteLine("OFF");
            writer.WriteLine(String.Format("{0} {1} {2}", GeometryObject.Points.Count, GeometryObject.Triangles.Count, GeometryObject.Edges.Count));

            for (int i = 0; i < dataMat.Columns; i++)
            {
                writer.WriteLine(String.Format("{0} {1} {2}", dataMat[0, i].ToDecimalStr(), dataMat[1, i].ToDecimalStr(), dataMat[2, i].ToDecimalStr()));
            }
            for (int i = 0; i < GeometryObject.Triangles.Count; i++)
            {
                writer.WriteLine(String.Format("3 {0} {1} {2}", GeometryObject.Triangles[i].P0.Id, GeometryObject.Triangles[i].P1.Id, GeometryObject.Triangles[i].P2.Id));
            }
            writer.Close();

        }

    }
}
