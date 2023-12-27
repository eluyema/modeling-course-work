using CourserWork.MessageRoutingNode.ModelingDataInterfaces;
using System;

namespace CourserWork.MessageRoutingNode
{
public class SimulatedAnnealingOptimization
    {
        private double initialTemperature;
        private double coolingRate;
        private Random random;

        public SimulatedAnnealingOptimization(double initialTemperature, double coolingRate)
        {
            this.initialTemperature = initialTemperature;
            this.coolingRate = coolingRate;
            this.random = new Random();
        }


        public void Optimize(MessageRoutingNodeSimulation simulation, MessageRoutingNodeInputData inputData)
        {
            double temperature = initialTemperature;
            double bestRevenue = 0;
            int n = 50;
            for (int i = 0; i < n; i++) {
                bestRevenue+=simulation.StartSimulation(inputData, false, false).Revenue;
            }
            bestRevenue = bestRevenue / n;

            double bestFirstDelay = inputData.FirstLineActualDelay;
            double bestSecondDelay = inputData.SecondLineActualDelay;

            while (temperature > 1)
            {
                double newFirstDelay = Math.Max(0, Math.Min(7, bestFirstDelay + (random.NextDouble() - 0.5) * 10));
                double newSecondDelay = Math.Max(0, Math.Min(8, bestSecondDelay + (random.NextDouble() - 0.5) * 10));

                inputData.FirstLineActualDelay = newFirstDelay;
                inputData.SecondLineActualDelay = newSecondDelay;

                double newRevenue = simulation.StartSimulation(inputData, false, false).Revenue;

                if (AcceptanceProbability(bestRevenue, newRevenue, temperature) > random.NextDouble())
                {
                    bestRevenue = newRevenue;
                    bestFirstDelay = newFirstDelay;
                    bestSecondDelay = newSecondDelay;
                }
                temperature *= 1 - coolingRate;
            }

            inputData.FirstLineActualDelay = bestFirstDelay;
            inputData.SecondLineActualDelay = bestSecondDelay;

            System.Console.WriteLine($"Optimized FirstLineActualDelay: {inputData.FirstLineActualDelay}");
            System.Console.WriteLine($"Optimized SecondLineActualDelay: {inputData.SecondLineActualDelay}");
        }

        private double AcceptanceProbability(double oldRevenue, double newRevenue, double temperature)
        {
            if (newRevenue > oldRevenue)
            {
                return 1.0;
            }
            return Math.Exp((newRevenue - oldRevenue) / temperature);
        }
    }


}
