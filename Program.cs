using CourserWork.Core;
using CourserWork.MessageRoutingNode;
using CourserWork.MessageRoutingNode.ModelingDataInterfaces;


namespace CourseWork
{

    public class Pogram
    {
        public static void Main(String[] args)
        {
            System.Console.WriteLine("Application started\n");
            MessageRoutingNodeOptimization();
        }

        public static void MessageRoutingNode()
        {
            MessageRoutingNodeSimulation simulation = new MessageRoutingNodeSimulation();

            MessageRoutingNodeInputData inputData = new MessageRoutingNodeInputData();
            inputData.FirstLineActualDelay = 7;
            inputData.SecondLineActualDelay = 8;

            bool showLogs = false;
            bool showFinalLogs = true;
            double revenue = 0;

            simulation.StartSimulation(inputData, showLogs, showFinalLogs);

        }

        public static void StartSimulationTImeExperiment()
        {
            MessageRoutingNodeSimulation simulation = new MessageRoutingNodeSimulation();

            bool showLogs = false;
            bool showFinalLogs = false;

            List<(int Key, double Value)> result = new List<(int Key, double Value)>();

            int startT = 1000;
            int endT = 100000;
            int step = 200;

            for (int time = startT; time < endT; time += step)
            {
                System.Console.WriteLine("Simulation time - " + time);
                MessageRoutingNodeInputData inputData = new MessageRoutingNodeInputData();
                inputData.SimulationTime = time;
                MessageRoutingNodeOutputData outputData = simulation.StartSimulation(inputData, showLogs, showFinalLogs);
                result.Add((time, outputData.Revenue));
            }

            CsvWriter.WriteToCsv(result, "D:\\Study\\ModelingOfSystems\\course-work");
        }

        public static void MessageRoutingNodeOptimization()
        {
            MessageRoutingNodeSimulation simulation = new MessageRoutingNodeSimulation();
            MessageRoutingNodeInputData inputData = new MessageRoutingNodeInputData();
            SimulatedAnnealingOptimization optimizer = new SimulatedAnnealingOptimization(10000, 0.003);
            optimizer.Optimize(simulation, inputData);

            double revenue = simulation.StartSimulation(inputData, false, false).Revenue;
            System.Console.WriteLine("Optimized revenue = " + revenue);
            System.Console.WriteLine("FirstLineActualDelay = " + inputData.FirstLineActualDelay);
            System.Console.WriteLine("SecondLineActualDelay = " + inputData.SecondLineActualDelay);

        }
    }
}