using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    class Utils_CheckErr
    {
        public static double CheckErr(Matrix mat_source, Matrix mat_target, ref CellIndex cellIndex)
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

            Matrix matErr = matQ - matP;

            double Err = 0;

            for (int i = 0; i < matErr.Columns; i++)
            {
                Err += Math.Sqrt(matErr[0, i] * matErr[0, i] + matErr[1, i] * matErr[1, i] + matErr[2, i] * matErr[2, i]);
            }

            Err = Err / matErr.Columns;

            return Err;

        }

    }
}
