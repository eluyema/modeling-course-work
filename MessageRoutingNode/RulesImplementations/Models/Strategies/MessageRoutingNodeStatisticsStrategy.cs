using CourserWork.Core.Elements;
using CourserWork.Core.ModelObjects;
using CourserWork.Core.Models.Strategies;
using CourserWork.Core.Nodes;
using CourserWork.MessageRoutingNode.ModelingDataInterfaces;
using CourserWork.MessageRoutingNode.RulesImplementations.Elements;
using CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects;

namespace CourserWork.MessageRoutingNode.RulesImplementations.Models.Strategies
{
    public class MessageRoutingNodeStatisticsStrategy : IStatisticsStrategy
    {
        private readonly double simulationTime;
        private int firstDirectionMessagesAmount;
        private int secondDirectionMessagesAmount;
        private double globalRevenue;

        public MessageRoutingNodeStatisticsStrategy(double simulationTime)
        {
            this.simulationTime = simulationTime;
            firstDirectionMessagesAmount = 0;
            secondDirectionMessagesAmount = 0;
            globalRevenue = 0;
        }

        public void DoStatistics(List<AbstractElement> elements, List<INode> nodes, double delta)
        {
            int firstDirectionAmount = 0;
            int secondDirectionAmount = 0;
            double calculatedRevenue = 0;
            foreach (var element in elements)
            {
                if (element is Process)
                {
                    Process p = (Process)element;

                    List<IModelObject> list = p.GetAllObjects();

                    foreach (IModelObject modelObject in list)
                    {
                        MessageObject messageObject = (MessageObject)modelObject;

                        if (messageObject.GetDirection() == MessageDirection.First)
                        {
                            firstDirectionAmount++;
                        }
                        else
                        {
                            secondDirectionAmount++;
                        }

                    }
                }
                if (element is MessageDispose) {
                    MessageDispose d = (MessageDispose)element;
                    calculatedRevenue += d.GetRevenue();
                }
            }
            firstDirectionMessagesAmount = firstDirectionAmount;
            secondDirectionMessagesAmount = secondDirectionAmount;
            globalRevenue = calculatedRevenue;
        }

        public int GetCurrentFirstDirectionMessagesAmount()
        {
            return firstDirectionMessagesAmount;
        }
        public int GetCurrentSecondDirectionMessagesAmount()
        {
            return secondDirectionMessagesAmount;
        }

        public void PrintStepStats(List<AbstractElement> elements, List<INode> nodes, double tcurr)
        {
            foreach (AbstractElement e in elements)
            {
                Console.Write("\n");
                if (e is Process)
                {
                    Process p = (Process)e;
                    string name = p.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + p.GetQuantity());
                    Console.WriteLine("   queue = " + p.GetQueueSize());
                    Console.WriteLine("   failure = " + p.GetFailure());
                    Console.WriteLine("   state = " + p.GetState());

                }

                if (e is Create)
                {
                    Create c = (Create)e;
                    string name = c.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + c.GetQuantity());
                }
                if (e is MessageDispose)
                {
                    MessageDispose d = (MessageDispose)e;
                    string name = d.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                    Console.WriteLine("   revenue = " + d.GetRevenue());
                }
            }
            Console.WriteLine("   global revenue = " + globalRevenue);
        }

        public void PrintFinalStats(List<AbstractElement> elements, List<INode> nodes, double tcurr)
        {
            Console.WriteLine("\n-------------RESULTS-------------");
            Console.WriteLine("\n        Clinic simulation!       ");
            Console.WriteLine("Simulation time - " + simulationTime);
            Console.WriteLine("Global revenue - " + globalRevenue);
            foreach (AbstractElement e in elements)
            {
                Console.Write("\n");
                if (e is Create)
                {
                    Create c = (Create)e;
                    string name = c.GetName();
                    int quantity = c.GetQuantity();

                    Console.WriteLine(name);
                    Console.WriteLine("  quantity = " + quantity);
                }
                if (e is Process)
                {
                    Process p = (Process)e;

                    string name = p.GetName();
                    double meanLengthOfQueue = p.GetMeanQueue() / tcurr;
                    double workload = p.GetWorkTime() / tcurr;
                    double averageInterval = p.GetAverageInterval();
                    double failureProbability = p.GetFailure() / (p.GetFailure() + (double)p.GetQuantity());
                    int quantity = p.GetQuantity();

                    Console.WriteLine(name);
                    Console.WriteLine("  quantity = " + quantity);
                    Console.WriteLine("  mean length of queue = " + meanLengthOfQueue);
                    Console.WriteLine("  workload = " + workload);
                    Console.WriteLine("  failure probabilitye = " + failureProbability);
                    if (name == "Laboratory")
                    {
                        Console.WriteLine("  average inerval = " + averageInterval);
                    }
                }
                if (e is MessageDispose)
                {
                    MessageDispose d = (MessageDispose)e;
                    string name = d.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                    Console.WriteLine("   revenue = " + d.GetRevenue());
                }
            }


        }

        public MessageRoutingNodeOutputData GetSimulationOutputData() {
            return new MessageRoutingNodeOutputData(globalRevenue);
        }

    }
}
