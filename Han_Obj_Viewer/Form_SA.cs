using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Han_Obj_Viewer
{
    public partial class Form_SA : Form
    {
        int RefreshRate = 100;
        int TimeTick = 0;
        public bool IsOpening = true;
        GeometryRoot geometryRoot;

        public Form_SA(GeometryRoot geometryRoot, int RefreshRate)
        {
            this.geometryRoot = geometryRoot;
            this.RefreshRate = RefreshRate;
            InitializeComponent();
        }

        public void SetVal(double Temperature, int OutIter, int InIter, int TotalIter, int Acceptance, double Value, double Min)
        {
            int nowTime = System.Environment.TickCount;
            if (nowTime - TimeTick < RefreshRate)
            {
                return;
            }
            else
            {
                TimeTick = nowTime;
                _SetVal(Temperature, OutIter, InIter, TotalIter, Acceptance, Value, Min);
                this.Update();
            }
        }

        private void _SetVal(double Temperature, int OutIter, int InIter, int TotalIter, int Acceptance, double Value, double Min)
        {
            this.labelT.Text = "Temperature: " + Temperature.ToString();
            this.labelOutIter.Text = "Outer Iter: " + OutIter.ToString();
            this.labelInIter.Text = "Inner Iter: " + InIter.ToString();
            this.labelTotalIter.Text = "Total Iter: " + TotalIter.ToString();
            this.labelAccept.Text = "Acceptance: " + Acceptance.ToString();
            this.labelVal.Text = "Value: " + Value.ToString();
            this.labelMin.Text = "Min: " + Min.ToString();
        }

        SA_Processor sa_Processor;
        DenseMatrix inputNormalMat_target;

        public GeometryObject TWO_MESHS_sourceObj;
        public GeometryObject TWO_MESHS_targetObj;

        DenseMatrix inputMat_source, inputMat_target;
        Transform PCATrans_source, PCATrans_target;
        DenseMatrix PCA_InvTransMat_source, PCA_TransMat_target, PCA_InvTransMat_target;
        CellIndex cellIndex;

        public void Do()
        {
            if (!initSA()) return;

            double MoveRange = 0.1;
            double RotateRange = Math.PI / 12;
            double ScaleTopRange = 1.25;
            double ScaleBottomRange = 0.8;

            double[] initData = { 0, 0, 0, 0, 0, 0, 1, 0};
            double[] initTops = { MoveRange, MoveRange, MoveRange, RotateRange, RotateRange, RotateRange, ScaleTopRange, 8};
            double[] initBottoms = { -MoveRange, -MoveRange, -MoveRange, -RotateRange, -RotateRange, -RotateRange, ScaleBottomRange, 0 };

            double initTemperature = 5;

            sa_Processor = new SA_Processor(SA_CalculateErr, initTemperature, initData, initTops, initBottoms);

            sa_Processor.DoSA(this);

            endSA();
        }


        private bool initSA()
        {
            Form_SAMeshSelection form_Selection = new Form_SAMeshSelection(geometryRoot);
            //form_Selection.DisableSVD();
            if (form_Selection.ShowDialog() != DialogResult.OK) return false;
            TWO_MESHS_sourceObj = geometryRoot[form_Selection.source];
            TWO_MESHS_targetObj = geometryRoot[form_Selection.target];

            int NCheck = Math.Min(TWO_MESHS_sourceObj.Points.Count, TWO_MESHS_targetObj.Points.Count);

            int NSamples = form_Selection.NSamples;

            NSamples = Math.Min(NCheck, NSamples);


            TWO_MESHS_sourceObj.Transform = new Transform();
            TWO_MESHS_targetObj.Transform = new Transform();

            inputMat_source = TWO_MESHS_sourceObj.ToSampledDataMat(NSamples);
            inputMat_target = TWO_MESHS_targetObj.ToSampledDataMat_WithNormal(NCheck, out inputNormalMat_target);

            //checkMat_source = TWO_MESHS_sourceObj.ToSampledDataMat(NCheck);
            //checkMat_target = TWO_MESHS_targetObj.ToSampledDataMat_WithNormal(NCheck, out checkNormalMat_target);

            PCATrans_source = Utils_PCA.DoPCA(inputMat_source);
            PCATrans_target = Utils_PCA.DoPCA(inputMat_target);

            PCATrans_source = correctPCATransform(inputMat_source, inputMat_target, PCATrans_source, PCATrans_target);

            PCA_InvTransMat_source = PCATrans_source.GetMatrix().Inverse() as DenseMatrix;

            PCA_TransMat_target = PCATrans_target.GetMatrix();
            PCA_InvTransMat_target = PCA_TransMat_target.Inverse() as DenseMatrix;

            inputMat_source = PCA_InvTransMat_source * inputMat_source;
            inputMat_target = PCA_InvTransMat_target * inputMat_target;

            inputNormalMat_target = PCATrans_target.ToRotationMatrix().Inverse() * inputNormalMat_target as DenseMatrix;
            //checkNormalMat_target = PCATrans_target.ToRotationMatrix().Inverse() * checkNormalMat_target as DenseMatrix;

            //checkMat_source = PCA_InvTransMat_source * checkMat_source;
            //checkMat_target = PCA_InvTransMat_target * checkMat_target;

            //errList = new List<double>();
            //errCellIndex = null;
            //double err = Utils_CheckErr.CheckErr(checkMat_source, checkMat_target, ref errCellIndex);
            //errList.Add(err);

            //SVD_TransMat = DenseMatrix.CreateIdentity(4);

            //MessageBox.Show(err.ToString());

            return true;
        }



        public double SA_CalculateErr(double[] data)
        {
            Transform transform = new Transform();
            transform.DoMove(data[0], data[1], data[2]);
            transform.DoRotateX(data[3]);
            transform.DoRotateY(data[4]);
            transform.DoRotateZ(data[5]);
            transform.DoScale(data[6]);
            DenseMatrix transformMat = transform.GetMatrix();
            Utils_PCA.getMirroredTransMat(transformMat, Convert.ToInt32(data[7]));
            transform.SetMatrix(transformMat);

            DenseMatrix matP_init = new DenseMatrix(4, inputMat_source.ColumnCount);
            inputMat_source.CopyTo(matP_init);
            matP_init = transform.GetMatrix() * matP_init;
            DenseMatrix matP = matP_init.SubMatrix(0, 3, 0, matP_init.ColumnCount) as DenseMatrix;

            if (cellIndex == null)
            {
                DenseMatrix matQ_init = inputMat_target.SubMatrix(0, 3, 0, inputMat_target.ColumnCount) as DenseMatrix;
                cellIndex = CellIndex.GetCellIndex(matQ_init, 8);
            }

            DenseMatrix matQ = cellIndex.DoPointMatch(matP);
            DenseMatrix matErr = matQ - matP;

            double Err = 0;

            for (int i = 0; i < matErr.ColumnCount; i++)
            {
                //Err += Math.Abs(matErr[0, i] * inputNormalMat_target[0, i]
                //    + matErr[1, i] * inputNormalMat_target[1, i]
                //    + matErr[2, i] * inputNormalMat_target[2, i]);
                Err += Math.Sqrt(matErr[0, i] * matErr[0, i] + matErr[1, i] * matErr[1, i] + matErr[2, i] * matErr[2, i]);
            }

            //Err = Err / matErr.ColumnCount;

            double absScale = Math.Abs(data[6]);
            Err = Err / absScale;
            return Err;
        }

        private void endSA()
        {
            string str = "";
            foreach (double rec in sa_Processor.Record)
            {
                str += rec.ToString() + "\n";
            }
            StreamWriter writer = new StreamWriter("d:/rec.txt");
            writer.Write(str);
            writer.Close();
            MessageBox.Show(sa_Processor.currentValue.ToString());

            double[] data = sa_Processor.currentData;

            Transform SA_Transform = new Transform();
            SA_Transform.DoMove(data[0], data[1], data[2]);
            SA_Transform.DoRotateX(data[3]);
            SA_Transform.DoRotateY(data[4]);
            SA_Transform.DoRotateZ(data[5]);
            SA_Transform.DoScale(data[6]);

            TWO_MESHS_sourceObj.Transform = new Transform(PCA_TransMat_target * SA_Transform.GetMatrix() * PCA_InvTransMat_source);
        }

        private Transform correctPCATransform(DenseMatrix inputMat_source, DenseMatrix inputMat_target, Transform PCATrans_source, Transform PCATrans_target)
        {
            DenseMatrix mat0 = PCATrans_source.GetMatrix();
            List<DenseMatrix> mats = new List<DenseMatrix>();
            double min = -1;
            int minIndex = 0;

            DenseMatrix invTrans_target = PCATrans_target.GetMatrix().Inverse() as DenseMatrix;
            DenseMatrix invedMat_target = invTrans_target * inputMat_target;

            for (int i = 0; i < 8; i++)
            {
                DenseMatrix mat = Utils_PCA.getMirroredTransMat(mat0, i);
                mats.Add(mat);
                DenseMatrix invTrans_source = mat.Inverse() as DenseMatrix;

                double err = Utils_CheckErr.CheckErr(invTrans_source * inputMat_source, invedMat_target, ref cellIndex);
                if (min < 0 || min > err)
                {
                    min = err;
                    minIndex = i;
                }
            }

            Transform newPCATrans_source = new Transform(mats[minIndex]);
            return newPCATrans_source;
        }

        private void Form_SA_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsOpening = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Do();
        }





    }
}
