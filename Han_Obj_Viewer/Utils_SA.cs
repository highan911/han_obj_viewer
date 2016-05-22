using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Distributions;
using System.Threading;

namespace Han_Obj_Viewer
{
    public class SA_Processor
    {
        double InitTemperature;
        int DataLength;
        public double currentValue;
        public double[] currentData;

        public double[][] currentMinData_byMirrors;
        //public double[] currentMinValue_byMirrors;

        public double currentMinValue;
        public double[] currentMinData;
        double[] RangeTops;
        double[] RangeBottoms;
        double alphaT = 0.9;
        Random random = new Random();


        //TODO
        private int innerIterCount_Limit = 20;
        private int innerIterCount_Accept_Limit = 20;

        //private int innerIterCount_Limit = 100;
        //private int innerIterCount_Accept_Limit = 20;

        private int middleIterCount_Limit = 10;


        private int outerIterCount_Limit = 100;
        private int no_Accept_Limit = 5;

        int MirrorsAcceptRate_Init = 2;
        //int MirrorsChangeRate = 1;

        public delegate double ValueFunction(double[] data);
        ValueFunction TheValueFunction;

        public List<double> Record;


        public SA_Processor(ValueFunction TheValueFunction, double initTemperature, double[] initData, double[] RangeTops, double[] RangeBottoms)
        {
            this.TheValueFunction = TheValueFunction;
            this.InitTemperature = initTemperature;
            //this.currentData = initData;


            this.currentMinData = initData;
            this.RangeTops = RangeTops;
            this.RangeBottoms = RangeBottoms;
            DataLength = initData.Length;

            this.currentValue = TheValueFunction(initData);
            this.currentMinValue = this.currentValue;

            this.currentMinData_byMirrors = new double[8][];//8 mirrors
            //this.currentMinValue_byMirrors = new double[8];
            for (int i = 0; i < 8; i++)
            {
                double[] initData_byMirrors = new double[DataLength];
                initData.CopyTo(initData_byMirrors, 0);
                initData_byMirrors[DataLength - 1] = i;
                this.currentMinData_byMirrors[i] = initData_byMirrors;
                //this.currentMinValue_byMirrors[i] = this.currentValue;
            }




            Record = new List<double>();
            Record.Add(this.currentValue);
        }

        //private double[] GetRandomData()
        //{
        //    double[] newData = new double[DataLength];
        //    for (int i = 0; i < DataLength; i++)
        //    {
        //        double sample = random.NextDouble();
        //        newData[i] = RangeBottoms[i] + sample * (RangeTops[i] - RangeBottoms[i]);
        //    }
        //    return newData;
        //}

        //private double[] GetRandomData_OneDim()
        //{
        //    int dim_i = random.Next(DataLength);
        //    return GetRandomData_OneDim(dim_i);
        //}

        private void ChangeCurrentMirror(ref int currentMirror, int[] MirrorsAcceptRate)
        {
            //int random_code = random.Next(MirrorsAcceptRate.Sum());
            //for (int i = 0; i < 8; i++)
            //{
            //    random_code -= MirrorsAcceptRate[i];
            //    if (random_code < 0)
            //    {
            //        currentMirror = i;
            //        break;
            //    }
            //}
            double MirrorsAcceptRate_Sum = 0;
            for (int i = 0; i < 8; i++)
            {
                MirrorsAcceptRate_Sum += Math.Log(MirrorsAcceptRate[i]);
            }
            double random_code = random.NextDouble() * MirrorsAcceptRate_Sum;
            for (int i = 0; i < 8; i++)
            {
                random_code -= Math.Log(MirrorsAcceptRate[i]);
                if (random_code < 0)
                {
                    currentMirror = i;
                    break;
                }
            }
            currentData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                currentData[i] = currentMinData_byMirrors[currentMirror][i];
            }
            //currentValue = currentMinValue_byMirrors[currentMirror];

        }

        private double[] GetRandomData_OneDim(int dim_i, int currentMirror)
        {
            double[] newData = new double[DataLength];

            for (int i = 0; i < DataLength; i++)
            {
                //newData[i] = currentData_byMirrors[currentMirror][i];
                newData[i] = currentData[i];
            }
            double sample = random.NextDouble();
            newData[dim_i] = RangeBottoms[dim_i] + sample * (RangeTops[dim_i] - RangeBottoms[dim_i]);

            return newData;
        }

        //private double[] GetRandomData_DimSeven(int dimValue)
        //{
        //    double[] newData = new double[DataLength];
        //    for (int i = 0; i < DataLength; i++)
        //    {
        //        newData[i] = currentData[i];
        //    }
        //    newData[7] = dimValue;
        //    return newData;
        //}

        //public void DoSA(Form_SA form)
        //{

        //    double Temperature = InitTemperature;

        //    int outerIterCount = 0;
        //    int totalIterCount = 0;
        //    int totalAccept = 0;

        //    int no_Accept_Count = 0;

        //    for (outerIterCount = 0; outerIterCount < outerIterCount_Limit; outerIterCount++)
        //    {
        //        //Temperature = (outerIterCount_Limit - outerIterCount) * InitTemperature / outerIterCount_Limit;
        //        Temperature = Temperature * alphaT;

        //        int InnerAcceptCount = 0;

        //        int innerIterCount = 0;

        //        bool has_Accept = false;


        //        //int dim_i = random.Next(DataLength);
        //        bool[] dim_visited = { false, false, false, false, false, false, false, false};

        //        for (int j = 0; j < 8; j++)
        //        {
        //            int dim_i = 0;

        //            while (true)
        //            {
        //                dim_i = random.Next(8);
        //                if(!dim_visited[dim_i])
        //                {
        //                    dim_visited[dim_i] = true;
        //                    break;
        //                }
        //            }

        //            if (dim_i == 7)
        //            {

        //                bool[] mirror_visited = { false, false, false, false, false, false, false, false };
        //                for (int i = 0; i < 8; i++)
        //                {

        //                    int mirror_i = 0;

        //                    while (true)
        //                    {
        //                        mirror_i = random.Next(8);
        //                        if (!mirror_visited[mirror_i])
        //                        {
        //                            mirror_visited[mirror_i] = true;
        //                            break;
        //                        }
        //                    }

        //                    innerIterCount++;
        //                    totalIterCount++;
        //                    Record.Add(currentValue);

        //                    double[] nowData = GetRandomData_DimSeven(mirror_i);
        //                    double nowValue = TheValueFunction(nowData);

        //                    form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

        //                    double deltaValue = nowValue - currentValue;
        //                    if (deltaValue < 0)
        //                    {
        //                        currentValue = nowValue;
        //                        currentData = nowData;
        //                        if (nowValue < currentMinValue)
        //                        {
        //                            currentMinValue = nowValue;
        //                            currentMinData = nowData;
        //                        }
        //                        InnerAcceptCount++;
        //                        totalAccept++;
        //                        has_Accept = true;
        //                        if (InnerAcceptCount > innerIterCount_Accept_Limit)
        //                            break;
        //                    }
        //                    else
        //                    {
        //                        double probability = Math.Exp(-(deltaValue / Temperature));
        //                        double lambda = random.NextDouble();
        //                        if (probability > lambda)
        //                        {
        //                            currentValue = nowValue;
        //                            currentData = nowData;
        //                            InnerAcceptCount++;
        //                            totalAccept++;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                for (int i = 0; i < innerIterCount_Limit; i++)
        //                {
        //                    innerIterCount++;
        //                    totalIterCount++;
        //                    Record.Add(currentValue);


        //                    if (i > innerIterCount_Limit)
        //                        break;
        //                    if (!form.IsOpening)
        //                        break;

        //                    double[] nowData = GetRandomData_OneDim(dim_i);
        //                    double nowValue = TheValueFunction(nowData);

        //                    form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

        //                    double deltaValue = nowValue - currentValue;

        //                    if (deltaValue < 0)
        //                    {
        //                        currentValue = nowValue;
        //                        currentData = nowData;
        //                        if (nowValue < currentMinValue)
        //                        {
        //                            currentMinValue = nowValue;
        //                            currentMinData = nowData;
        //                        }
        //                        InnerAcceptCount++;
        //                        totalAccept++;
        //                        has_Accept = true;
        //                        if (InnerAcceptCount > innerIterCount_Accept_Limit)
        //                            break;
        //                    }
        //                    else
        //                    {
        //                        double probability = Math.Exp(-(deltaValue / Temperature));
        //                        double lambda = random.NextDouble();
        //                        if (probability > lambda)
        //                        {
        //                            currentValue = nowValue;
        //                            currentData = nowData;
        //                            InnerAcceptCount++;
        //                            totalAccept++;
        //                            if (InnerAcceptCount > innerIterCount_Accept_Limit)
        //                                break;
        //                        }
        //                    }
        //                }//inner
        //            }
        //        }//middle

        //        if (!has_Accept)
        //            no_Accept_Count++;
        //        if (no_Accept_Count > no_Accept_Limit)
        //            break;
        //        if (!form.IsOpening)
        //            break;
        //    }//outer

        //}

        public void DoSA(Form_SA form)
        {

            double Temperature = InitTemperature;

            int outerIterCount = 0;
            int totalIterCount = 0;
            int totalAccept = 0;

            int currentMirror = 0;

            int[] MirrorsAcceptRate = { MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init, MirrorsAcceptRate_Init };
            int no_Accept_Count = 0;

            for (outerIterCount = 0; outerIterCount < outerIterCount_Limit; outerIterCount++)
            {
                //Temperature = (outerIterCount_Limit - outerIterCount) * InitTemperature / outerIterCount_Limit;
                Temperature = Temperature * alphaT;

                int InnerAcceptCount = 0;

                int innerIterCount = 0;

                bool has_Accept = false;


                //int dim_i = random.Next(DataLength);
                //bool[] dim_visited = { false, false, false, false, false, false, false, false };

                for (int middleIterCount = 0; middleIterCount < middleIterCount_Limit; middleIterCount++)
                {
                    //int dim_i = 0;

                    //while (true)
                    //{
                    //    dim_i = random.Next(7);
                    //    if (!dim_visited[dim_i])
                    //    {
                    //        dim_visited[dim_i] = true;
                    //        break;
                    //    }
                    //}


                    //if (random.Next(MirrorsChangeRate) == 0)
                    //{
                    //    ChangeCurrentMirror(ref currentMirror, MirrorsAcceptRate);//change mirror
                    //}
                    ChangeCurrentMirror(ref currentMirror, MirrorsAcceptRate);//change mirror

                    for (innerIterCount = 0; innerIterCount < innerIterCount_Limit; innerIterCount++)
                    {

                        if (innerIterCount > innerIterCount_Limit)
                            break;
                        if (!form.IsOpening)
                            break;

                        totalIterCount++;
                        Record.Add(currentValue);

                        int dim_i = random.Next(7);
                        double[] nowData = GetRandomData_OneDim(dim_i, currentMirror);

                        double nowValue = TheValueFunction(nowData);

                        form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

                        double deltaValue = nowValue - currentValue;

                        if (deltaValue < 0)
                        {
                            currentValue = nowValue;
                            //currentData_byMirrors[currentMirror] = nowData;
                            currentData = nowData;

                            MirrorsAcceptRate[currentMirror]++;

                            if (nowValue < currentMinValue)
                            {
                                currentMinValue = nowValue;
                                currentMinData = nowData;
                                currentMinData_byMirrors[currentMirror] = nowData;

                            }

                            //if (nowValue < currentMinValue_byMirrors[currentMirror])
                            //{
                            //    currentMinValue_byMirrors[currentMirror] = nowValue;
                            //}


                            InnerAcceptCount++;
                            totalAccept++;
                            has_Accept = true;
                            if (InnerAcceptCount > innerIterCount_Accept_Limit)
                                break;
                        }
                        else
                        {
                            double probability = Math.Exp(-(deltaValue / Temperature));
                            double lambda = random.NextDouble();
                            if (probability > lambda)
                            {
                                currentValue = nowValue;
                                //currentData_byMirrors[currentMirror] = nowData;
                                currentData = nowData;
                                InnerAcceptCount++;
                                totalAccept++;
                                if (InnerAcceptCount > innerIterCount_Accept_Limit)
                                    break;
                            }
                        }
                    }//inner
                }//middle

                if (!has_Accept)
                    no_Accept_Count++;
                if (no_Accept_Count > no_Accept_Limit)
                    break;
                if (!form.IsOpening)
                    break;
            }//outer

        }


    }
}