using CourserWork.Core.Elements;
using CourserWork.Core.Models;
using CourserWork.Core.Nodes;
using CourserWork.MessageRoutingNode.ModelingDataInterfaces;
using CourserWork.MessageRoutingNode.RulesImplementations.Elements;
using CourserWork.MessageRoutingNode.RulesImplementations.Elements.Internals;
using CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects;
using CourserWork.MessageRoutingNode.RulesImplementations.Models.Strategies;
using CourserWork.MessageRoutingNode.RulesImplementations.Nodes;

namespace CourserWork.MessageRoutingNode
{
    public class MessageRoutingNodeSimulation
    {
        public MessageRoutingNodeOutputData StartSimulation(MessageRoutingNodeInputData inputData, bool showLogs, bool showFinal)
        {
            MessageRoutingNodeStatisticsStrategy statisticsStrategy = new MessageRoutingNodeStatisticsStrategy(inputData.SimulationTime);

            MessageObjectFabric firstDirectionMessageObjectFabric = new MessageObjectFabric(MessageDirection.First);
            MessageObjectFabric secondDirectionMessageObjectFabric = new MessageObjectFabric(MessageDirection.Second);

            Create firstDirectionMessageCreate = new Create("Message Create (First Direction)", inputData.FirstInputCreateMean, firstDirectionMessageObjectFabric);
            firstDirectionMessageCreate.SetDelayDev(inputData.FirstInputCreateDev);
            firstDirectionMessageCreate.SetDistribution(DistributionType.NORM);

            Create secondDirectionMessageCreate = new Create("Message Create (Second Direction)", inputData.SecondInputCreateMean, secondDirectionMessageObjectFabric);
            secondDirectionMessageCreate.SetDelayDev(inputData.SecondInputCreateDev);
            secondDirectionMessageCreate.SetDistribution(DistributionType.NORM);

            GlobalMessageBearer globalMessageBearer = new GlobalMessageBearer(statisticsStrategy, inputData.FirstDirectionLimit, inputData.SecondDirectionLimit);

            Process messageProcessor = new Process("Message Processor", inputData.ProcessorMean);
            messageProcessor.SetDistribution(DistributionType.UNIF);
            messageProcessor.SetDelayDev(inputData.ProcessorDev);
            messageProcessor.SetModelObjectBearer(globalMessageBearer);
            messageProcessor.SetMaxqueue(inputData.ProcessorMaxQueue);

            Process firstDirectionLine = new Process("Line (First Direction)", inputData.FirstLineActualDelay);
            firstDirectionLine.SetDistribution(DistributionType.None);

            Process secondDirectionLine = new Process("Line (Second Direction)", inputData.SecondLineActualDelay);
            secondDirectionLine.SetDistribution(DistributionType.None);

            MessageDispose firstDirectionMessageDispose = new MessageDispose(
                "Message Dispose (First Direction)", inputData.GetFirstLineMessagePrice());
            MessageDispose secondDirectionMessageDispose = new MessageDispose(
                "Message Dispose (Second Direction)", inputData.GetSecondLineMessagePrice());

            CommonNode nodeProcessor = new CommonNode(new List<AbstractElement> { messageProcessor });

            MessageBranchingNode nodeBranchingLines = new MessageBranchingNode(
                new List<(MessageDirection, AbstractElement)> {
                    (MessageDirection.First, firstDirectionLine),
                    (MessageDirection.Second, secondDirectionLine) },
                secondDirectionLine);

            CommonNode nodeFirstLineDispose = new CommonNode(new List<AbstractElement> { firstDirectionMessageDispose });
            CommonNode nodeSecondLineDispose = new CommonNode(new List<AbstractElement> { secondDirectionMessageDispose });

            firstDirectionMessageCreate.SetNode(nodeProcessor);
            secondDirectionMessageCreate.SetNode(nodeProcessor);
            messageProcessor.SetNode(nodeBranchingLines);
            firstDirectionLine.SetNode(nodeFirstLineDispose);
            secondDirectionLine.SetNode(nodeSecondLineDispose);

            List<INode> nodes = new List<INode> { nodeProcessor, nodeBranchingLines, nodeFirstLineDispose, nodeSecondLineDispose };
            List<AbstractElement> elements = new List<AbstractElement> {
                firstDirectionMessageCreate,
                secondDirectionMessageCreate,
                messageProcessor,
                firstDirectionLine,
                secondDirectionLine,
                firstDirectionMessageDispose,
                secondDirectionMessageDispose
            };

            Model model = new Model(elements, nodes, statisticsStrategy);

            model.Simulate(inputData.SimulationTime, showLogs, showFinal);

            return statisticsStrategy.GetSimulationOutputData();
        }
    }
}
