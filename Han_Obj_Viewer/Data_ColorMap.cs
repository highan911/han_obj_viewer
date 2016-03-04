using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{

    public class ColorMap : Dictionary<int, float[]>
    {
        public static float[] R = { 1, 0, 0 };
        public static float[] G = { 0, 1, 0 };
        public static float[] B = { 0, 0, 1 };
        public static float[] D = { 0.5f, 0.5f, 0.5f };

        public void SetData(int key, double value)
        {
            if (this.ContainsKey(key))
            {
                float[] ColorFloats = ToColorFloats(value);
                this[key] = ColorFloats;
            }
        }

        public static float[] ToColorFloats(double data)
        {
            float[] ColorFloats = new float[3];
            if (data < 0) data = 0;
            if (data > 1) data = 1;

            if (data < 0.25)
            {
                ColorFloats[0] = 0;
                ColorFloats[1] = (float)(data / 0.25);
                ColorFloats[2] = 1;
            }
            else if (data < 0.5)
            {
                ColorFloats[0] = 0;
                ColorFloats[1] = 1;
                ColorFloats[2] = (float)((0.5 - data) / 0.25);
            }
            else if (data < 0.75)
            {
                ColorFloats[0] = (float)((data - 0.5) / 0.25);
                ColorFloats[1] = 1;
                ColorFloats[2] = 0;
            }
            else
            {
                ColorFloats[0] = 1;
                ColorFloats[1] = (float)((1 - data) / 0.25);
                ColorFloats[2] = 0;
            }
            return ColorFloats;
        }

        public bool SetDataArray(List<double> array)
        {
            if (array.Count != this.Count) return false;
            double max = array.Max();
            double min = array.Min();
            for (int i = 0; i < this.Count; i++)
            {
                this.SetData(i, (array[i] - min) / (max - min));
            }
            return true;
        }

        public ColorMap(Points points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.Add(i, D);
            }
        }

        public ColorMap(Triangles triangles, List<int> tids = null)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                if (tids != null && tids.Count > 0)
                {
                    if (i == tids[0])
                    {
                        this.Add(i, R);
                    }
                    else if (tids.Contains(i))
                    {
                        this.Add(i, G);
                    }
                    else
                    {
                        this.Add(i, B);
                    }
                }
                else
                {
                    this.Add(i, B);
                }
            }
        }
    }

}
