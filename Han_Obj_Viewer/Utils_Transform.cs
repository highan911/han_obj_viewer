using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Han_Obj_Viewer
{
    public class Transform
    {
        public double[] BasisX = { 1, 0, 0 };//BasisX
        public double[] BasisY = { 0, 1, 0 };//BasisY
        public double[] BasisZ = { 0, 0, 1 };//BasisZ
        public double[] Origin = { 0, 0, 0 };//Origin
        public double Scale = 1;//Scale
        //public bool R = false;//HasReflection

        private DenseMatrix Mat;

        public Transform()
        {
            Mat = DenseMatrix.CreateIdentity(4);
        }

        public Transform(double[] BasisX, double[] BasisY, double[] BasisZ, double[] Origin, double Scale)
        {
            this.BasisX = BasisX;
            this.BasisY = BasisY;
            this.BasisZ = BasisZ;
            this.Origin = Origin;
            this.Scale = Scale;
            this.ToMatrix();
        }

        public Transform(DenseMatrix Mat)
        {
            this.Mat = Mat;
            this.FromMatrix();
        }

        public void DoTransform(DenseMatrix mat)
        {
            this.Mat = mat * this.Mat;
            this.FromMatrix();
        }

        public void DoScale(double scale)
        {
            this.Scale = this.Scale * scale;
            this.ToMatrix();
        }

        public void DoMove(double x, double y, double z)
        {
            this.Origin[0] = this.Origin[0] + x;
            this.Origin[1] = this.Origin[1] + y;
            this.Origin[2] = this.Origin[2] + z;
            this.ToMatrix();
        }

        public void DoRotateX(double radian)
        {
            double sin = Math.Sin(radian);
            double cos = Math.Cos(radian);
            double[] MR_list = 
        {1,0,0,0,
        0,cos,-sin,0,
        0,sin,cos,0,
        0,0,0,1};
            DenseMatrix MR = new DenseMatrix(4, 4, MR_list).Transpose() as DenseMatrix;
            DoTransform(MR);
        }

        public void DoRotateY(double radian)
        {
            double sin = Math.Sin(radian);
            double cos = Math.Cos(radian);
            double[] MR_list = 
        {cos,0,sin,0,
        0,1,0,0,
        -sin,0,cos,0,
        0,0,0,1};
            DenseMatrix MR = new DenseMatrix(4, 4, MR_list).Transpose() as DenseMatrix;
            DoTransform(MR);
        }

        public void DoRotateZ(double radian)
        {
            double sin = Math.Sin(radian);
            double cos = Math.Cos(radian);
            double[] MR_list = 
        {cos,-sin,0,0,
        sin,cos,0,0,
        0,0,1,0,
        0,0,0,1};
            DenseMatrix MR = new DenseMatrix(4, 4, MR_list).Transpose() as DenseMatrix;
            DoTransform(MR);
        }



        public void ApplyTrans(double x, double y, double z, out double xp, out double yp, out double zp)
        {
                DenseMatrix coor = new DenseMatrix(4, 1);
                coor[0, 0] = x;
                coor[1, 0] = y;
                coor[2, 0] = z;
                coor[3, 0] = 1;
                coor = Mat * coor;
                xp = coor[0, 0];
                yp = coor[1, 0];
                zp = coor[2, 0];
        }

        public DenseMatrix ToMoveMatrix()
        {
            double[] MT_list = 
        {1,0,0,Origin[0],
        0,1,0,Origin[1],
        0,0,1,Origin[2],
        0,0,0,1};
            DenseMatrix MT = new DenseMatrix(4, 4, MT_list).Transpose() as DenseMatrix;
            return MT;
        }

        public DenseMatrix ToRotationMatrix()
        {
            double[] MR_list = 
        {BasisX[0],BasisY[0],BasisZ[0],0,
        BasisX[1],BasisY[1],BasisZ[1],0,
        BasisX[2],BasisY[2],BasisZ[2],0,
        0,0,0,1};
            DenseMatrix MR = new DenseMatrix(4, 4, MR_list).Transpose() as DenseMatrix;
            return MR;
        }

        public DenseMatrix ToScaleMatrix()
        {
            double[] MS_list = 
        {Scale,0,0,0,
        0,Scale,0,0,
        0,0,Scale,0,
        0,0,0,1};
            DenseMatrix MS = new DenseMatrix(4, 4, MS_list).Transpose() as DenseMatrix;
            return MS;
        }


        private void ToMatrix()
        {
            DenseMatrix MT = ToMoveMatrix();
            DenseMatrix MR = ToRotationMatrix();
            DenseMatrix MS = ToScaleMatrix();

            this.Mat = MT * MR * MS;
        }


        private void FromMatrix()
        {
            DenseMatrix mat = new DenseMatrix(4, 4);
            this.Mat.CopyTo(mat);

            Origin[0] = mat[0, 3];
            Origin[1] = mat[1, 3];
            Origin[2] = mat[2, 3];
            double[] MT_list = 
        {1,0,0,Origin[0],
        0,1,0,Origin[1],
        0,0,1,Origin[2],
        0,0,0,1};
            DenseMatrix IMT = new DenseMatrix(4, 4, MT_list).Inverse().Transpose() as DenseMatrix;
            mat = IMT * mat;

            Scale = Math.Sqrt(mat[0, 0] * mat[0, 0] + mat[1, 0] * mat[1, 0] + mat[2, 0] * mat[2, 0]);
            double[] MS_list = 
        {Scale,0,0,0,
        0,Scale,0,0,
        0,0,Scale,0,
        0,0,0,1};
            DenseMatrix IMS = new DenseMatrix(4, 4, MS_list).Inverse().Transpose() as DenseMatrix;
            mat = mat * IMS;

            BasisX[0] = mat[0, 0]; BasisX[1] = mat[1, 0]; BasisX[2] = mat[2, 0];
            BasisY[0] = mat[0, 1]; BasisY[1] = mat[1, 1]; BasisY[2] = mat[2, 1];
            BasisZ[0] = mat[0, 2]; BasisZ[1] = mat[1, 2]; BasisZ[2] = mat[2, 2];
        }

        public DenseMatrix GetMatrix()
        {
            DenseMatrix mat = new DenseMatrix(4, 4);
            Mat.CopyTo(mat);
            return mat;
        }

    }
}
