using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public class CellIndex : List<List<double[]>>
    {
        double CellSize;
        int[] CellsCount = new int[3];
        double[] Size = new double[3];
        double[] Min = new double[3];
        double eps = 0.001;


        public CellIndex(double[] Min, double[] Max, int minCellCount)
        {
            this.Min = Min;
            this.Size[0] = Max[0] - Min[0];
            this.Size[1] = Max[1] - Min[1];
            this.Size[2] = Max[2] - Min[2];

            double minSize = Math.Min(Math.Min(Size[0], Size[1]), Size[2]);

            this.CellSize = (minSize + eps) / minCellCount;
            this.CellsCount[0] = (int)Math.Ceiling((Size[0] + eps) / CellSize);
            this.CellsCount[1] = (int)Math.Ceiling((Size[1] + eps) / CellSize);
            this.CellsCount[2] = (int)Math.Ceiling((Size[2] + eps) / CellSize);

            for (int i = 0; i < CellsCount[0] * CellsCount[1] * CellsCount[2]; i++)
            {
                this.Add(new List<double[]>());
            }

        }

        void InsertPoint(double[] p)
        {
            List<double[]> Cell = GetCell(p);
            Cell.Add(p);
        }


        List<double[]> GetCell(int xCell, int yCell, int zCell)
        {
            if (xCell < 0 || yCell < 0 || zCell < 0) return null;
            if (xCell >= CellsCount[0] || yCell >= CellsCount[1] || zCell >= CellsCount[2]) return null;
            int index = xCell * CellsCount[1] * CellsCount[2] + yCell * CellsCount[2] + zCell;
            return this[index];
        }

        List<double[]> GetCell(double[] p)
        {
            if (p[0] < Min[0] || p[1] < Min[1] || p[2] < Min[2]) return null;
            int xCell = (int)Math.Floor((p[0] - Min[0]) / CellSize);
            int yCell = (int)Math.Floor((p[1] - Min[1]) / CellSize);
            int zCell = (int)Math.Floor((p[2] - Min[2]) / CellSize);
            return GetCell(xCell, yCell, zCell);
        }



        List<double[]> GetLayer(int xCell, int yCell, int zCell, int Layer)
        {
            List<double[]> list = new List<double[]>();
            bool[] Shell = new bool[3];
            for (int i = Math.Max(xCell - Layer, 0); i <= Math.Min(xCell + Layer, CellsCount[0] - 1); i++)
            {
                if (i == xCell - Layer || i == xCell + Layer) Shell[0] = true;
                else Shell[0] = false;
                for (int j = Math.Max(yCell - Layer, 0); j <= Math.Min(yCell + Layer, CellsCount[1] - 1); j++)
                {
                    if (j == yCell - Layer || j == yCell + Layer) Shell[1] = true;
                    else Shell[1] = false;
                    for (int k = Math.Max(zCell - Layer, 0); k <= Math.Min(zCell + Layer, CellsCount[2] - 1); k++)
                    {
                        if (k == zCell - Layer || k == zCell + Layer) Shell[2] = true;
                        else Shell[2] = false;
                        if (!(Shell[0] && Shell[1] && Shell[2])) continue;
                        List<double[]> points = GetCell(i, j, k);
                        if (points != null) list.AddRange(points);

                    }
                }
            }
            return list;
        }

        List<double[]> GetLayer(double x, double y, double z, int Layer)
        {
            int xCell = (int)Math.Floor((x - Min[0]) / CellSize);
            int yCell = (int)Math.Floor((y - Min[1]) / CellSize);
            int zCell = (int)Math.Floor((z - Min[2]) / CellSize);
            if (x < Min[0]) xCell--;
            if (y < Min[1]) yCell--;
            if (z < Min[2]) zCell--;
            return GetLayer(xCell, yCell, zCell, Layer);
        }

        List<double[]> GetLayer(double[] p, int Layer)
        {
            return GetLayer(p[0], p[1], p[2], Layer);
        }

        public static CellIndex GetCellIndex(Matrix mat_source, Matrix mat_target, int minCellCount)
        {
            double[] Min = { mat_source[0, 0], mat_source[1, 0], mat_source[2, 0] };
            double[] Max = { mat_source[0, 0], mat_source[1, 0], mat_source[2, 0] };

            for (int i = 1; i < mat_source.Columns; i++)
            {
                Min[0] = Math.Min(Min[0], mat_source[0, i]);
                Min[1] = Math.Min(Min[1], mat_source[1, i]);
                Min[2] = Math.Min(Min[2], mat_source[2, i]);
                Max[0] = Math.Max(Max[0], mat_source[0, i]);
                Max[1] = Math.Max(Max[1], mat_source[1, i]);
                Max[2] = Math.Max(Max[2], mat_source[2, i]);
            }
            for (int i = 0; i < mat_target.Columns; i++)
            {
                Min[0] = Math.Min(Min[0], mat_target[0, i]);
                Min[1] = Math.Min(Min[1], mat_target[1, i]);
                Min[2] = Math.Min(Min[2], mat_target[2, i]);
                Max[0] = Math.Max(Max[0], mat_target[0, i]);
                Max[1] = Math.Max(Max[1], mat_target[1, i]);
                Max[2] = Math.Max(Max[2], mat_target[2, i]);
            }

            CellIndex cellIndex = new CellIndex(Min, Max, minCellCount);

            for (int i = 0; i < mat_target.Columns; i++)
            {
                double[] p = { mat_target[0, i], mat_target[1, i], mat_target[2, i] };
                cellIndex.InsertPoint(p);
            }

            return cellIndex;
        }


        public Matrix DoPointMatch(Matrix mat_source)
        {

            Matrix matQ = new Matrix(mat_source.Rows, mat_source.Columns);

            for (int i = 0; i < mat_source.Columns; i++)
            {
                List<double[]> Neighbor = new List<double[]>();
                int Layer = 0;
                while (Neighbor.Count == 0)
                {
                    Neighbor.AddRange(this.GetLayer(mat_source[0, i], mat_source[1, i], mat_source[2, i], Layer));
                    Layer++;
                }
                double[] p = null;
                double nearDist = -1;
                foreach (double[] np in Neighbor)
                {
                    if (p == null)
                    {
                        p = np;
                        nearDist = (p[0] - np[0]) * (p[0] - np[0]) + (p[1] - np[1]) * (p[1] - np[1]) + (p[2] - np[2]) * (p[2] - np[2]);
                    }
                    else
                    {
                        double dist = (p[0] - np[0]) * (p[0] - np[0]) + (p[1] - np[1]) * (p[1] - np[1]) + (p[2] - np[2]) * (p[2] - np[2]);
                        if (dist < nearDist)
                        {
                            p = np;
                            nearDist = dist;
                        }
                    }
                }
                matQ[0, i] = p[0];
                matQ[1, i] = p[1];
                matQ[2, i] = p[2];
            }

            return matQ;
        }

    }

}
