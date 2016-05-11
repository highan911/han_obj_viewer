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
        double[] Data;
        double currentTotalDistance;
        double[] Sigmas;//the sigma values of Normal Random Sampling
        double alphaT;//(0,1), the decreasion rate of Temperature;
        double alphaS;//(0,1), the decreasion rate of Sigmas;

        //double lambda;//Random(0, 1)
        Random random = new Random();


        //TODO
        private int innerIterCount_Limit;
        private int outerIterCount_Limit;


        public SA_Processor(double initTemperature, double[] initData, double[] initSigmas)
        {
            this.Temperature = initTemperature;
            this.Data = initData;
            this.Sigmas = initSigmas;
            DataLength = initData.Length;
        }

        private double[] GetNormalRandomData()
        {
            double[] newData = new double[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                Normal normal = new Normal(Data[i], Sigmas[i]);
                newData[i] = normal.Sample();
            }
            return newData;
        }

        private void DoSA()
        {
            int outerIterCount = 0;
            while (true)
            {
                //inner loop: same temperature
                int innerIterCount = 0;
                while (true)
                {
                    double[] newData = GetNormalRandomData();
                    double nowTotalDistance2 = CalculateTotalDistance(newData);

                    double deltaTotalDistance = nowTotalDistance2 - currentTotalDistance;

                    if (deltaTotalDistance < 0)
                    {
                        for (int i = 0; i < DataLength; i++) Data[i] = newData[i];
                    }
                    else
                    {
                        double probability = Math.Exp(-(deltaTotalDistance / Temperature));
                        double lambda = ((double)(random.Next() % 10000)) / 10000.0;
                        if (probability > lambda)
                        {
                            for (int i = 0; i < DataLength; i++) Data[i] = newData[i];
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

        private double CalculateTotalDistance(double[] newData)
        {
            throw new NotImplementedException();
        }


    }
}
