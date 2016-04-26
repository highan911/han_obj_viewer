using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace Han_Obj_Viewer
{
    public class Utils_PCA
    {
        public static bool DoPCA(DenseMatrix inputMat, out double[] sortedEigenValues, out Transform PCATrans)
        {
            double[] Origin = new double[3];
            double[] BasisX = new double[3];
            double[] BasisY = new double[3];
            double[] BasisZ = new double[3];
            double Scale;

            
            PCATrans = new Transform();

            DenseMatrix dataMat = inputMat.SubMatrix(0, 3, 0, inputMat.ColumnCount) as DenseMatrix;

            DenseMatrix meanMat = getMeanMat(dataMat);

            Origin[0] = meanMat[0, 0];
            Origin[1] = meanMat[1, 0];
            Origin[2] = meanMat[2, 0];

            dataMat = dataMat - meanMat;
            DenseMatrix covar = dataMat * dataMat.Transpose() as DenseMatrix;

            
            //bool EvSuccess = covar.ComputeEvJacobi(dblEigenValue, mtxEigenVector, eps);

            MathNet.Numerics.LinearAlgebra.Factorization.Evd<double> evd = covar.Evd();

            Vector<Complex> eigenValues = evd.EigenValues;
            DenseMatrix eigenVectors = evd.EigenVectors as DenseMatrix;

            sortedEigenValues = new double[3];

            //if (!EvSuccess) return false;

            int index = getMaxEigenValueIndex(eigenValues);
            sortedEigenValues[0] = eigenValues[index].Magnitude;
            eigenValues[index] = 0;
            BasisX[0] = eigenVectors[0, index];
            BasisX[1] = eigenVectors[1, index];
            BasisX[2] = eigenVectors[2, index];

            index = getMaxEigenValueIndex(eigenValues);
            sortedEigenValues[1] = eigenValues[index].Magnitude;
            eigenValues[index] = 0;
            BasisY[0] = eigenVectors[0, index];
            BasisY[1] = eigenVectors[1, index];
            BasisY[2] = eigenVectors[2, index];

            index = getMaxEigenValueIndex(eigenValues);
            sortedEigenValues[2] = eigenValues[index].Magnitude;
            eigenValues[index] = 0;
            BasisZ[0] = eigenVectors[0, index];
            BasisZ[1] = eigenVectors[1, index];
            BasisZ[2] = eigenVectors[2, index];

            //Scale = Math.Sqrt(sortedEigenValue[0] * sortedEigenValue[0] + sortedEigenValue[1] * sortedEigenValue[1] + sortedEigenValue[2] * sortedEigenValue[2]);

            double pmin, pmax;
            pmin = BasisX[0] * dataMat[0, 0] + BasisX[1] * dataMat[1, 0] + BasisX[2] * dataMat[2, 0];
            pmax = pmin;
            for (int i = 1; i < inputMat.ColumnCount; i++)
            {
                double pdata = BasisX[0] * dataMat[0, i] + BasisX[1] * dataMat[1, i] + BasisX[2] * dataMat[2, i];
                if (pdata > pmax) pmax = pdata;
                if (pdata < pmin) pmin = pdata;
            }
            Scale = pmax - pmin;

            PCATrans = new Transform(BasisX, BasisY, BasisZ, Origin, Scale);

            return true;
        }

        private static DenseMatrix getMeanMat(DenseMatrix mat)
        {
            DenseMatrix meanMat = new DenseMatrix(mat.RowCount, mat.ColumnCount);

            for (int i = 0; i < mat.RowCount; i++)
            {
                double sum = 0;
                for (int j = 0; j < mat.ColumnCount; j++)
                {
                    sum += mat[i, j];
                }
                double ave = sum / mat.ColumnCount;
                for (int j = 0; j < mat.ColumnCount; j++)
                {
                    meanMat[i, j] = ave;
                }
            }
            return meanMat;
        }

        private static int getMaxEigenValueIndex(Vector<Complex> eigenValues)
        {
            int index = 0;
            double max = 0;
            for (int i = 0; i < eigenValues.Count; i++)
            {
                double val = eigenValues[i].Magnitude;
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
