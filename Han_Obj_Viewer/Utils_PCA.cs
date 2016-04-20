using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Han_Obj_Viewer
{
    public class Utils_PCA
    {
        public static bool DoPCA(Matrix inputMat, out double[] sortedEigenValue, out Transform PCATrans)
        {
            double[] Origin = new double[3];
            double[] BasisX = new double[3];
            double[] BasisY = new double[3];
            double[] BasisZ = new double[3];
            double Scale;

            sortedEigenValue = new double[3];
            PCATrans = new Transform();

            double[] data = new double[3 * inputMat.Columns];
            Array.Copy(inputMat.GetData(), data, 3 * inputMat.Columns);

            Matrix dataMat = new Matrix(3, inputMat.Columns, data);

            Matrix meanMat = getMeanMat(dataMat);

            Origin[0] = meanMat[0, 0];
            Origin[1] = meanMat[0, 1];
            Origin[2] = meanMat[0, 2];

            dataMat = dataMat - meanMat;
            Matrix covar = dataMat * dataMat.Transpose();

            double[] dblEigenValue = new double[3];
            double eps=0.001;
            Matrix mtxEigenVector = new Matrix(3, 3);
            bool EvSuccess = covar.ComputeEvJacobi(dblEigenValue, mtxEigenVector, eps);
            if (!EvSuccess) return false;

            int index = getMaxEigenValueIndex(dblEigenValue);
            sortedEigenValue[0] = dblEigenValue[index];
            dblEigenValue[index] = 0;
            BasisX[0] = mtxEigenVector[0, index];
            BasisX[1] = mtxEigenVector[1, index];
            BasisX[2] = mtxEigenVector[2, index];

            index = getMaxEigenValueIndex(dblEigenValue);
            sortedEigenValue[1] = dblEigenValue[index];
            dblEigenValue[index] = 0;
            BasisY[0] = mtxEigenVector[0, index];
            BasisY[1] = mtxEigenVector[1, index];
            BasisY[2] = mtxEigenVector[2, index];

            index = getMaxEigenValueIndex(dblEigenValue);
            sortedEigenValue[2] = dblEigenValue[index];
            dblEigenValue[index] = 0;
            BasisZ[0] = mtxEigenVector[0, index];
            BasisZ[1] = mtxEigenVector[1, index];
            BasisZ[2] = mtxEigenVector[2, index];

            Scale = Math.Sqrt(sortedEigenValue[0] * sortedEigenValue[0] + sortedEigenValue[1] * sortedEigenValue[1] + sortedEigenValue[2] * sortedEigenValue[2]);

            PCATrans = new Transform(BasisX, BasisY, BasisZ, Origin, Scale);
            return true;
        }

        private static Matrix getMeanMat(Matrix mat)
        {
            Matrix meanMat = new Matrix(mat.Rows, mat.Columns);

            for (int i = 0; i < mat.Rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < mat.Columns; j++)
                {
                    sum += mat[i, j];
                }
                double ave = sum / mat.Columns;
                for (int j = 0; j < mat.Columns; j++)
                {
                    meanMat[i, j] = ave;
                }
            }
            return meanMat;
        }

        private static int getMaxEigenValueIndex(double[] dblEigenValue)
        {
            int index = 0;
            double max = 0;
            for (int i = 0; i < dblEigenValue.Length; i++)
            {
                double val = Math.Abs(dblEigenValue[i]);
                if (max < Math.Max(max, val))
                {
                    max = val;
                    index = i;
                }
            }
            return index;
        }

    }
}
