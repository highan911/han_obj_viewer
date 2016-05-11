using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Han_Obj_Viewer
{
    public class Utils_SVD
    {

        public static DenseMatrix SVDGetTransMat(DenseMatrix mat_source, DenseMatrix mat_target, ref CellIndex cellIndex)
        {
            //4 * NSamples

            DenseMatrix matP = mat_source.SubMatrix(0, 3, 0, mat_source.ColumnCount) as DenseMatrix;

            if (cellIndex == null)
            {
                DenseMatrix matQ_init = mat_target.SubMatrix(0, 3, 0, mat_target.ColumnCount) as DenseMatrix;
                cellIndex = CellIndex.GetCellIndex(matP, matQ_init, 4);
            }

            DenseMatrix matQ = cellIndex.DoPointMatch(matP);

            DenseMatrix matP_Mean = Utils_PCA.getMeanMat(matP);
            DenseMatrix matQ_Mean = Utils_PCA.getMeanMat(matQ);

            DenseMatrix matT_MoveP = DenseMatrix.CreateIdentity(4);
            DenseMatrix matT_MoveQ = DenseMatrix.CreateIdentity(4);
            for (int i = 0; i < 3; i++)
            {
                matT_MoveP[i, 3] = matP_Mean[i, 0];
                matT_MoveQ[i, 3] = matQ_Mean[i, 0];
            }


            matP = matP - matP_Mean;
            matQ = matQ - matQ_Mean;


            DenseMatrix matM = matP * matQ.Transpose() as DenseMatrix;

            //matM.SplitUV(matU, matV, 0.01);
            MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> svd = matM.Svd(true);

            DenseMatrix matU = svd.U as DenseMatrix;
            DenseMatrix matVT = svd.VT as DenseMatrix;


            DenseMatrix matR = (matU * matVT).Transpose() as DenseMatrix;
            DenseMatrix matT = DenseMatrix.CreateIdentity(4);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matT[i, j] = matR[i, j];
                }
            }

            matT = matT_MoveP.Inverse() * matT * matT_MoveQ as DenseMatrix;

            return matT;

        }

    }
}
