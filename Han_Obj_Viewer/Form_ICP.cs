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
    public partial class Form_ICP : Form
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

        public Form_ICP(GeometryRoot geometryRoot, int RefreshRate)
        {
            this.geometryRoot = geometryRoot;
            this.RefreshRate = RefreshRate;
            InitializeComponent();
        }

        public void SetVal(int TotalIter, double Value, double Min)
        {
            int nowTime = System.Environment.TickCount;
            if (nowTime - TimeTick < RefreshRate)
            {
                return;
            }
            else
            {
                TimeTick = nowTime;
                _SetVal(TotalIter, Value, Min);
                this.Update();
            }
        }

        private void _SetVal(int TotalIter, double Value, double Min)
        {
            this.labelTotalIter.Text = "Total Iter: " + TotalIter.ToString();
            this.labelVal.Text = "Value: " + Value.ToString();
            this.labelMin.Text = "Min: " + Min.ToString();
        }

        DenseMatrix inputMat_source, inputMat_target;//, checkMat_source, checkMat_target;
        Transform PCATrans_source, PCATrans_target;
        int SourceSamples, TargetSamples, SVDLoops;
        DenseMatrix PCA_InvTransMat_source, PCA_TransMat_target, PCA_InvTransMat_target, SVD_TransMat;
        List<double> errList;
        CellIndex cellIndex;

        private bool initICP()
        {
            if (TWO_MESHS_sourceObj == null || TWO_MESHS_targetObj == null)
            {
                Form_ICPMeshSelection form_Selection = new Form_ICPMeshSelection(geometryRoot);
                if (form_Selection.ShowDialog() != DialogResult.OK) return false;
                TWO_MESHS_sourceObj = geometryRoot[form_Selection.source];
                TWO_MESHS_targetObj = geometryRoot[form_Selection.target];

                SVDLoops = form_Selection.SVDLoops;

                SourceSamples = form_Selection.SourceSamples;
                TargetSamples = form_Selection.TargetSamples;
            }


            TWO_MESHS_sourceObj.Transform = new Transform();
            TWO_MESHS_targetObj.Transform = new Transform();

            inputMat_source = TWO_MESHS_sourceObj.ToSampledDataMat(SourceSamples);
            inputMat_target = TWO_MESHS_targetObj.ToSampledDataMat(TargetSamples);

            //checkMat_source = TWO_MESHS_sourceObj.ToSampledDataMat(NCheck);
            //checkMat_target = TWO_MESHS_targetObj.ToSampledDataMat(NCheck);

            PCATrans_source = Utils_PCA.DoPCA(inputMat_source);
            PCATrans_target = Utils_PCA.DoPCA(inputMat_target);

            //PCATrans_source = correctPCATransform(inputMat_source, inputMat_target, PCATrans_source, PCATrans_target);

            PCA_InvTransMat_source = PCATrans_source.GetMatrix().Inverse() as DenseMatrix;

            PCA_TransMat_target = PCATrans_target.GetMatrix();
            PCA_InvTransMat_target = PCA_TransMat_target.Inverse() as DenseMatrix;

            inputMat_source = PCA_InvTransMat_source * inputMat_source;
            inputMat_target = PCA_InvTransMat_target * inputMat_target;

            errList = new List<double>();

            SVD_TransMat = DenseMatrix.CreateIdentity(4);

            return true;
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


        private void endICP(int iLoop)
        {

            TWO_MESHS_sourceObj.Transform = new Transform(PCA_TransMat_target * SVD_TransMat * PCA_InvTransMat_source);

            DenseMatrix checkSourceMat = PCA_TransMat_target * SVD_TransMat * PCA_InvTransMat_source * TWO_MESHS_sourceObj.ToDataMat();
            DenseMatrix checkSourceMat_PCA = PCA_TransMat_target * PCA_InvTransMat_source * TWO_MESHS_sourceObj.ToDataMat();

            int timeDelta = EndTime - StartTime;
            double err_pca = Utils_CheckErr.CheckErr(checkSourceMat_PCA, TWO_MESHS_targetObj.ToDataMat());
            double err = Utils_CheckErr.CheckErr(checkSourceMat, TWO_MESHS_targetObj.ToDataMat());


            string str = "Time: " + timeDelta.ToString() + "\n";
            str += "Err_PCA: " + err_pca.ToString() + "\n";
            str += "Err: " + err.ToString() + "\n";
            foreach (double rec in errList)
            {
                str += rec.ToString() + "\n";
            }
            StreamWriter writer = new StreamWriter("d:/rec_icp_"
                + TargetSamples.ToString() + "_" + SourceSamples.ToString() + "_" + iLoop.ToString() + ".txt");
            writer.Write(str);
            writer.Close();
        }

        double CurrentMin = 0;
        double lastErrValue = 0;
        int SameValLoops = 0;
        int SameValLimit = 3;

        public bool Do(int iLoop)
        {
            if (!initICP()) return false;
            StartTime = System.Environment.TickCount;
            for (int i = 0; i < SVDLoops; i++)
            {
                double ErrValue = 0;
                DenseMatrix temp_SVD_TransMat = Utils_SVD.SVDGetTransMat(inputMat_source, inputMat_target, ref cellIndex, out ErrValue);
                SVD_TransMat = temp_SVD_TransMat * SVD_TransMat;
                inputMat_source = temp_SVD_TransMat * inputMat_source;
                errList.Add(ErrValue);

                if (i == 0 || ErrValue < CurrentMin)
                    CurrentMin = ErrValue;
                SetVal(i, ErrValue, CurrentMin);

                if (Math.Abs(ErrValue - lastErrValue) < 0.01 * CurrentMin)
                {
                    SameValLoops++;
                    if (SameValLoops > SameValLimit)
                    {
                        SetVal(i, ErrValue, CurrentMin);
                        break;
                    }
                }
                else
                {
                    SameValLoops = 0;
                }
                lastErrValue = ErrValue;

            }
            EndTime = System.Environment.TickCount;
            endICP(iLoop);
            return true;
        }



        private void Form_ICP_FormClosing(object sender, FormClosingEventArgs e)
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
