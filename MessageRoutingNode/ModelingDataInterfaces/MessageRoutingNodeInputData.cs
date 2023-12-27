using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.MessageRoutingNode.ModelingDataInterfaces
{
    public class MessageRoutingNodeInputData
    {
        public double SimulationTime { get; set; }
        public int FirstDirectionLimit { get; set; }
        public int SecondDirectionLimit { get; set; }
        public double FirstInputCreateMean { get; set; }
        public double FirstInputCreateDev { get; set; }
        public double SecondInputCreateMean { get; set; }
        public double SecondInputCreateDev { get; set; }
        public int ProcessorMaxQueue { get; set; }
        public double ProcessorMean { get; set; }
        public double ProcessorDev { get; set; }
        public double FirstLineDefaultDelay { get; set; }
        public double SecondLineDefaultDelay { get; set; }
        public double FirstDirectionDefaultMessagePrice { get; set; }
        public double SecondDirectionDefaultMessagePrice { get; set; }
        public double FirstLineActualDelay { get; set; }
        public double SecondLineActualDelay { get; set; }
        public double FirstLineUnitTimeCost { get; set; }
        public double SecondLineUnitTimeCost { get; set; }

        public MessageRoutingNodeInputData()
        {
            SimulationTime = 10000;
            FirstDirectionLimit = 3;
            SecondDirectionLimit = 3;
            FirstInputCreateMean = 6;
            FirstInputCreateDev = 1;
            SecondInputCreateMean = 5;
            SecondInputCreateDev = 1;
            ProcessorMaxQueue = 6;
            ProcessorMean = 2;
            ProcessorDev = 1;
            FirstLineDefaultDelay = 7;
            SecondLineDefaultDelay = 8;
            FirstDirectionDefaultMessagePrice = 20;
            SecondDirectionDefaultMessagePrice = 40;
            FirstLineActualDelay = 7;
            SecondLineActualDelay = 8;
            FirstLineUnitTimeCost = 2;
            SecondLineUnitTimeCost = 4;
        }

        public double GetFirstLineMessagePrice() {
            return CalculateOneMessagePrice(
                FirstLineDefaultDelay,
                FirstLineActualDelay,
                FirstDirectionDefaultMessagePrice,
                FirstLineUnitTimeCost);
        }

        public double GetSecondLineMessagePrice()
        {
            return CalculateOneMessagePrice(
                SecondLineDefaultDelay,
                SecondLineActualDelay,
                SecondDirectionDefaultMessagePrice,
                SecondLineUnitTimeCost);
        }

        private double CalculateOneMessagePrice(double defaultTime, double actualTime, double defaultPrice, double costPerTimeUnit)
        {
            if (actualTime < 0)
            {
                throw new ArgumentException("Actual time must be greater than 0.");
            }

            double timeReduction = defaultTime - actualTime;
            double priceLoss = timeReduction * costPerTimeUnit;

            double updatedRevenue = defaultPrice - priceLoss;
            return updatedRevenue > 0 ? updatedRevenue : 0;
        }
    }
}
