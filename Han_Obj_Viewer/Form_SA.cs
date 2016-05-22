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

        public bool Finished = false;
        
        GeometryRoot geometryRoot;


        public GeometryObject TWO_MESHS_sourceObj = null;
        public GeometryObject TWO_MESHS_targetObj = null;

        int StartTime;
        int EndTime;


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


        DenseMatrix inputMat_source, inputMat_target;
        Transform PCATrans_source, PCATrans_target;
        DenseMatrix PCA_InvTransMat_source, PCA_TransMat_target, PCA_InvTransMat_target;
        CellIndex cellIndex;

        int CellsCount = 8;

        public bool Do(int iloop)
        {
            if (!initSA()) return false;

            double MoveRange = 0.1;
            double RotateRange = Math.PI / 12;
            double ScaleTopRange = 1.25;
            double ScaleBottomRange = 0.8;

            double[] initData = { 0, 0, 0, 0, 0, 0, 1, 0};
            double[] initTops = { MoveRange, MoveRange, MoveRange, RotateRange, RotateRange, RotateRange, ScaleTopRange, 8};
            double[] initBottoms = { -MoveRange, -MoveRange, -MoveRange, -RotateRange, -RotateRange, -RotateRange, ScaleBottomRange, 0 };

            double initTemperature = 5;


            StartTime = System.Environment.TickCount;
            sa_Processor = new SA_Processor(SA_CalculateErr, initTemperature, initData, initTops, initBottoms);

            sa_Processor.DoSA(this);

            EndTime = System.Environment.TickCount;
            endSA(iloop);
            return true;
        }

        int SourceSamples, TargetSamples;
        private bool initSA()
        {
            if (TWO_MESHS_sourceObj == null || TWO_MESHS_targetObj == null)
            {
                Form_SAMeshSelection form_Selection = new Form_SAMeshSelection(geometryRoot);
                if (form_Selection.ShowDialog() != DialogResult.OK) return false;
                TWO_MESHS_sourceObj = geometryRoot[form_Selection.source];
                TWO_MESHS_targetObj = geometryRoot[form_Selection.target];

                SourceSamples = form_Selection.SourceSamples;
                TargetSamples = form_Selection.TargetSamples;
            }

            TWO_MESHS_sourceObj.Transform = new Transform();
            TWO_MESHS_targetObj.Transform = new Transform();

            inputMat_source = TWO_MESHS_sourceObj.ToSampledDataMat(SourceSamples);
            inputMat_target = TWO_MESHS_targetObj.ToSampledDataMat_WithNormal(TargetSamples, out inputNormalMat_target);

            PCATrans_source = Utils_PCA.DoPCA(inputMat_source);
            PCATrans_target = Utils_PCA.DoPCA(inputMat_target);

            //PCATrans_source = correctPCATransform(inputMat_source, inputMat_target, PCATrans_source, PCATrans_target);

            PCA_InvTransMat_source = PCATrans_source.GetMatrix().Inverse() as DenseMatrix;

            PCA_TransMat_target = PCATrans_target.GetMatrix();
            PCA_InvTransMat_target = PCA_TransMat_target.Inverse() as DenseMatrix;

            inputMat_source = PCA_InvTransMat_source * inputMat_source;
            inputMat_target = PCA_InvTransMat_target * inputMat_target;

            inputNormalMat_target = PCATrans_target.ToRotationMatrix().Inverse() * inputNormalMat_target as DenseMatrix;
            return true;
        }



        public double SA_CalculateErr(double[] data)
        {
            DenseMatrix mirrorMat = Utils_PCA.getMirrorTransMat(Convert.ToInt32(data[7]));


            Transform transform = new Transform();
            transform.DoMove(data[0], data[1], data[2]);
            transform.DoRotateX(data[3]);
            transform.DoRotateY(data[4]);
            transform.DoRotateZ(data[5]);
            transform.DoScale(data[6]);

            DenseMatrix transformMat = transform.GetMatrix() * mirrorMat;


            DenseMatrix matP_init = new DenseMatrix(4, inputMat_source.ColumnCount);
            inputMat_source.CopyTo(matP_init);
            matP_init = transformMat * matP_init;
            DenseMatrix matP = matP_init.SubMatrix(0, 3, 0, matP_init.ColumnCount) as DenseMatrix;

            if (cellIndex == null)
            {
                DenseMatrix matQ_init = inputMat_target.SubMatrix(0, 3, 0, inputMat_target.ColumnCount) as DenseMatrix;
                cellIndex = CellIndex.GetCellIndex(matQ_init, CellsCount);
            }

            DenseMatrix matQ = cellIndex.DoPointMatch(matP);
            DenseMatrix matErr = matQ - matP;

            double Err = 0;

            for (int i = 0; i < matErr.ColumnCount; i++)
            {
                Err += Math.Sqrt(matErr[0, i] * matErr[0, i] + matErr[1, i] * matErr[1, i] + matErr[2, i] * matErr[2, i]);
            }

            double absScale = Math.Abs(data[6]);
            Err = Err / absScale;
            return Err;
        }

        private void endSA(int iloop)
        {

            double[] data = sa_Processor.currentMinData;

            DenseMatrix mirrorMat = Utils_PCA.getMirrorTransMat(Convert.ToInt32(data[7]));

            Transform transform = new Transform();
            transform.DoMove(data[0], data[1], data[2]);
            transform.DoRotateX(data[3]);
            transform.DoRotateY(data[4]);
            transform.DoRotateZ(data[5]);
            transform.DoScale(data[6]);

            DenseMatrix transformMat = transform.GetMatrix() * mirrorMat;


            TWO_MESHS_sourceObj.Transform = new Transform(PCA_TransMat_target * transformMat * PCA_InvTransMat_source);

            DenseMatrix checkSourceMat = PCA_TransMat_target * transformMat * PCA_InvTransMat_source * TWO_MESHS_sourceObj.ToDataMat();
            DenseMatrix checkSourceMat_PCA = PCA_TransMat_target * PCA_InvTransMat_source * TWO_MESHS_sourceObj.ToDataMat();

            int timeDelta = EndTime - StartTime;
            double err_pca = Utils_CheckErr.CheckErr(checkSourceMat_PCA, TWO_MESHS_targetObj.ToDataMat());
            double err = Utils_CheckErr.CheckErr(checkSourceMat, TWO_MESHS_targetObj.ToDataMat());
            

            string str = "Time: " + timeDelta.ToString() + "\n";
            str += "Err_PCA: " + err_pca.ToString() + "\n";
            str += "Err: " + err.ToString() + "\n";
            foreach (double rec in sa_Processor.Record)
            {
                str += rec.ToString() + "\n";
            }
            StreamWriter writer = new StreamWriter("d:/rec_sa_"
                + TargetSamples.ToString() + "_" + SourceSamples.ToString() + "_" + iloop.ToString() + ".txt");
            writer.Write(str);
            writer.Close();


        }

        //private Transform correctPCATransform(DenseMatrix inputMat_source, DenseMatrix inputMat_target, Transform PCATrans_source, Transform PCATrans_target)
        //{
        //    DenseMatrix mat0 = PCATrans_source.GetMatrix();
        //    List<DenseMatrix> mats = new List<DenseMatrix>();
        //    double min = -1;
        //    int minIndex = 0;

        //    DenseMatrix invTrans_target = PCATrans_target.GetMatrix().Inverse() as DenseMatrix;
        //    DenseMatrix invedMat_target = invTrans_target * inputMat_target;

        //    for (int i = 0; i < 8; i++)
        //    {
        //        DenseMatrix mat = Utils_PCA.getMirroredTransMat(mat0, i);
        //        mats.Add(mat);
        //        DenseMatrix invTrans_source = mat.Inverse() as DenseMatrix;

        //        double err = Utils_CheckErr.CheckErr(invTrans_source * inputMat_source, invedMat_target, ref cellIndex);
        //        if (min < 0 || min > err)
        //        {
        //            min = err;
        //            minIndex = i;
        //        }
        //    }

        //    Transform newPCATrans_source = new Transform(mats[minIndex]);
        //    return newPCATrans_source;
        //}

        private void Form_SA_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsOpening = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (Finished)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                for (int i = 1; i <= 10; i++)
                {
                    Do(i);
                }
                buttonStart.Text = "Close";
                this.Finished = true;
                //if (Do())
                //{
                //    buttonStart.Text = "Close";
                //    this.Finished = true;
                //}
            }
            
        }

    }
}
