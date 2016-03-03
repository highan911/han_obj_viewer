using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public class ColorMap : Dictionary<int, float[]>//Point, Color
    {
        public static float[] Zero = { 0, 1, 0 };
        public ColorMap(Points points)
        {
            foreach (int key in points.Keys)
            {
                this.Add(key, Zero);
            }
        }
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
    }
}
