using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Han_Obj_Viewer
{
    class Utils_CheckErr
    {
        public static double CheckErr(DenseMatrix mat_source, DenseMatrix mat_target, ref CellIndex cellIndex)
        {
            //4 * NSamples
            DenseMatrix matP = mat_source.SubMatrix(0, 3, 0, mat_source.ColumnCount) as DenseMatrix;

            if (cellIndex == null)
            {
                DenseMatrix matQ_init = mat_target.SubMatrix(0, 3, 0, mat_target.ColumnCount) as DenseMatrix;
                cellIndex = CellIndex.GetCellIndex(matP, matQ_init, 4);
            }

            DenseMatrix matQ = cellIndex.DoPointMatch(matP);

            DenseMatrix matErr = matQ - matP;

            double Err = 0;

            for (int i = 0; i < matErr.ColumnCount; i++)
            {
                Err += Math.Sqrt(matErr[0, i] * matErr[0, i] + matErr[1, i] * matErr[1, i] + matErr[2, i] * matErr[2, i]);
            }

            Err = Err / matErr.ColumnCount;

            return Err;

        }

    }
}
