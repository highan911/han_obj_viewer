using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Distributions;

namespace Han_Obj_Viewer
{
    public class SA_Processor
    {

        double Temperature;
        int DataLength;
        public double currentValue;
        public double[] currentData;
        public double currentMinValue;
        public double[] currentMinData;
        double[] Sigmas;//the sigma values of Normal Random Sampling
        double alphaT = 0.88;//(0,1), the decreasion rate of Temperature;
        double alphaS = 1;//(0,1), the decreasion rate of Sigmas;

        //double lambda;//Random(0, 1)
        Random random = new Random();


        //TODO
        private int innerIterCount_Limit = 20;
        private int outerIterCount_Limit = 200;

        public delegate double ValueFunction(double[] data);
        ValueFunction TheValueFunction;

        public List<double> Record;

        public SA_Processor(ValueFunction TheValueFunction, double initTemperature, double[] initData, double[] initSigmas)
        {
            this.TheValueFunction = TheValueFunction;
            this.Temperature = initTemperature;
            this.currentData = initData;
            this.currentMinData = initData;
            this.Sigmas = initSigmas;
            DataLength = initData.Length;

            this.currentValue = TheValueFunction(currentData);
            this.currentMinValue = this.currentValue;

            Record = new List<double>();
            Record.Add(this.currentValue);
        }

        private double[] GetNormalRandomData()
        {
            double[] newData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                Normal normal = new Normal(currentData[i], Sigmas[i], random);
                newData[i] = normal.Sample();
            }
            return newData;
        }

        private double[] GetNormalRandomData_OneDim()
        {
            double[] newData = new double[DataLength];

            int i = random.Next(DataLength);

            Normal normal = new Normal(currentData[i], Sigmas[i], random);
            newData[i] = normal.Sample();

            return newData;
        }

        public void DoSA()
        {
            int outerIterCount = 0;
            while (true)
            {
                //inner loop: same temperature
                int innerIterCount = 0;
                while (true)
                {
                    double[] newData = GetNormalRandomData();
                    double nowValue = TheValueFunction(newData);

                    Record.Add(nowValue);
                    if (nowValue < currentMinValue)
                    {
                        currentValue = nowValue;
                        currentData = newData;
                        currentMinValue = nowValue;
                        currentMinData = newData;
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
                        double lambda = ((double)(random.Next() % 10000)) / 10000.0;
                        if (probability > lambda)
                        {
                            currentValue = nowValue;
                            currentData = newData;
                        }
                    }
                    if (innerIterCount >= innerIterCount_Limit)
                    {
                        break;
                    }
                    else
                    {
                        innerIterCount++;
                    }
                }

                Temperature = alphaT * Temperature;
                for (int i = 0; i < DataLength; i++) Sigmas[i] = alphaS * Sigmas[i];

                if (outerIterCount >= outerIterCount_Limit)
                {
                    break;
                }
                else
                {
                    outerIterCount++;
                }

            }
        }
    }
}
