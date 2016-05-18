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
        public double currentMinValue;
        public double[] currentMinData;
        double[] RangeTops;
        double[] RangeBottoms;
        double alphaT = 0.9;
        Random random = new Random();


        //TODO
        private int innerIterCount_Limit = 25;
        private int innerIterCount_Accept_Limit = 5;
        private int outerIterCount_Limit = 100;
        private int no_Accept_Limit = 10;

        public delegate double ValueFunction(double[] data);
        ValueFunction TheValueFunction;

        public List<double> Record;


        public SA_Processor(ValueFunction TheValueFunction, double initTemperature, double[] initData, double[] RangeTops, double[] RangeBottoms)
        {
            this.TheValueFunction = TheValueFunction;
            this.InitTemperature = initTemperature;
            this.currentData = initData;
            this.currentMinData = initData;
            this.RangeTops = RangeTops;
            this.RangeBottoms = RangeBottoms;
            DataLength = initData.Length;

            this.currentValue = TheValueFunction(currentData);
            this.currentMinValue = this.currentValue;

            Record = new List<double>();
            Record.Add(this.currentValue);
        }

        private double[] GetRandomData()
        {
            double[] newData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                double sample = random.NextDouble();
                newData[i] = RangeBottoms[i] + sample * (RangeTops[i] - RangeBottoms[i]);
            }
            return newData;
        }

        private double[] GetRandomData_OneDim()
        {
            int dim_i = random.Next(DataLength);
            return GetRandomData_OneDim(dim_i);
        }

        private double[] GetRandomData_OneDim(int dim_i)
        {
            double[] newData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                newData[i] = currentData[i];
            }
            double sample = random.NextDouble();
            newData[dim_i] = RangeBottoms[dim_i] + sample * (RangeTops[dim_i] - RangeBottoms[dim_i]);
            return newData;
        }

        private double[] GetRandomData_DimSeven(int dimValue)
        {
            double[] newData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                newData[i] = currentData[i];
            }
            newData[7] = dimValue;
            return newData;
        }

        public void DoSA(Form_SA form)
        {

            double Temperature = InitTemperature;

            int outerIterCount = 0;
            int totalIterCount = 0;
            int totalAccept = 0;

            int no_Accept_Count = 0;

            for (outerIterCount = 0; outerIterCount < outerIterCount_Limit; outerIterCount++)
            {
                //Temperature = (outerIterCount_Limit - outerIterCount) * InitTemperature / outerIterCount_Limit;
                Temperature = Temperature * alphaT;
                //inner loop: same temperature

                int InnerAcceptCount = 0;

                int innerIterCount = 0;

                bool has_Accept = false;

                
                //int dim_i = random.Next(DataLength);
                bool[] visited = { false, false, false, false, false, false, false, false};

                for (int j = 0; j < 8; j++)
                {
                    int dim_i = 0;
                    
                    while (true)
                    {
                        dim_i = random.Next(8);
                        if(!visited[dim_i])
                        {
                            visited[dim_i] = true;
                            break;
                        }
                    }

                    if (dim_i == 7)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            innerIterCount++;
                            totalIterCount++;
                            Record.Add(currentValue);

                            double[] nowData = GetRandomData_DimSeven(i);
                            double nowValue = TheValueFunction(nowData);

                            form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

                            double deltaValue = nowValue - currentValue;
                            if (deltaValue < 0)
                            {
                                currentValue = nowValue;
                                currentData = nowData;
                                if (nowValue < currentMinValue)
                                {
                                    currentMinValue = nowValue;
                                    currentMinData = nowData;
                                }
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
                                    currentData = nowData;
                                    InnerAcceptCount++;
                                    totalAccept++;
                                    break;
                                }
                            }
                        }
                    }else{

                        for (int i = 0; i < innerIterCount_Limit; i++)
                        {
                            innerIterCount++;
                            totalIterCount++;
                            Record.Add(currentValue);


                            if (i > innerIterCount_Limit)
                                break;
                            if (!form.IsOpening)
                                break;

                            double[] nowData = GetRandomData_OneDim(dim_i);
                            double nowValue = TheValueFunction(nowData);

                            form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

                            double deltaValue = nowValue - currentValue;

                            if (deltaValue < 0)
                            {
                                currentValue = nowValue;
                                currentData = nowData;
                                if (nowValue < currentMinValue)
                                {
                                    currentMinValue = nowValue;
                                    currentMinData = nowData;
                                }
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
                                    currentData = nowData;
                                    InnerAcceptCount++;
                                    totalAccept++;
                                    if (InnerAcceptCount > innerIterCount_Accept_Limit)
                                        break;
                                }
                            }
                        }//inner
                    }
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
