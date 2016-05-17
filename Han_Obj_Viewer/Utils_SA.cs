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
        double alphaT = 0.8;
        Random random = new Random();


        //TODO
        private int innerIterCount_Limit = 100;
        private int innerIterCount_Accept_Limit = 10;
        private int outerIterCount_Limit = 120;

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

        public void DoSA(Form_SA form)
        {

            double Temperature = InitTemperature;

            int outerIterCount = 0;
            int totalIterCount = 0;
            int totalAccept = 0;

            for (outerIterCount = 0; outerIterCount < outerIterCount_Limit; outerIterCount++)
            {
                //Temperature = (outerIterCount_Limit - outerIterCount) * InitTemperature / outerIterCount_Limit;
                Temperature = Temperature * alphaT;
                //inner loop: same temperature
                int innerIterCount = 0;

                int InnerAcceptCount = 0;

                int dim_i = random.Next(DataLength);

                for(innerIterCount = 0; innerIterCount < innerIterCount_Limit; innerIterCount++)
                {
                    totalIterCount++;

                    if (innerIterCount > innerIterCount_Limit)
                        break;
                    if (!form.IsOpening)
                        break;

                    double[] newData = GetRandomData_OneDim(dim_i);
                    double nowValue = TheValueFunction(newData);

                    form.SetVal(Temperature, outerIterCount, innerIterCount, totalIterCount, totalAccept, nowValue, currentMinValue);

                    Record.Add(nowValue);
                    if (nowValue < currentMinValue)
                    {
                        currentValue = nowValue;
                        currentData = newData;
                        currentMinValue = nowValue;
                        currentMinData = newData;

                        InnerAcceptCount++;
                        totalAccept++;
                        if (InnerAcceptCount > innerIterCount_Accept_Limit)
                            break;
                        if (dim_i == 7) break;
                        continue;
                    }


                    double deltaValue = nowValue - currentValue;


                    if (deltaValue < 0)
                    {
                        currentValue = nowValue;
                        currentData = newData;
                    }
                    else
                    {
                        double probability = Math.Exp(-(deltaValue / Temperature));
                        double lambda = random.NextDouble();
                        if (probability > lambda)
                        {
                            currentValue = nowValue;
                            currentData = newData;
                            InnerAcceptCount++;
                            totalAccept++;
                            if (InnerAcceptCount > innerIterCount_Accept_Limit)
                                break;
                            if (dim_i == 7) break;
                        }
                    }
                    
                }
                if (!form.IsOpening)
                    break;
            }
            //form.Close();
        }
    }
}
