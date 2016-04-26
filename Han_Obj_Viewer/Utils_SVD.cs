﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public class Utils_SVD
    {

        public static Matrix SVDGetTransMat(Matrix mat_source, Matrix mat_target, ref CellIndex cellIndex)
        {
            //4 * NSamples

            double[] data_source = new double[3 * mat_source.Columns];
            Array.Copy(mat_source.GetData(), data_source, 3 * mat_source.Columns);
            Matrix matP = new Matrix(3, mat_source.Columns, data_source);

            if (cellIndex == null)
            {
                double[] data_target = new double[3 * mat_target.Columns];
                Array.Copy(mat_target.GetData(), data_target, 3 * mat_target.Columns);
                Matrix matQ_init = new Matrix(3, mat_target.Columns, data_target);

                cellIndex = CellIndex.GetCellIndex(matP, matQ_init, 2);
            }

            Matrix matQ = cellIndex.DoPointMatch(matP);

            Matrix matM = matP * matQ.Transpose();

            Matrix matU = new Matrix(3, 3);
            Matrix matV = new Matrix(3, 3);

            matM.SplitUV(matU, matV, 0.0001);

            Matrix matUV = matV * matU.Transpose();
            Matrix matT = new Matrix(4, 4);
            matT.MakeUnitMatrix(4);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matT[i, j] = matUV[i, j];
                }
            }
            return matT;
        }

    }
}
