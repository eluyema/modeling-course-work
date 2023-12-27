using CourserWork.Core.Elements;
using CourserWork.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core.Models.Strategies
{
    public interface IStatisticsStrategy
    {
        public void DoStatistics(List<AbstractElement> elements, List<INode> nodes, double delta);

        public void PrintStepStats(List<AbstractElement> elements, List<INode> nodes, double tcurr);

        public void PrintFinalStats(List<AbstractElement> elements, List<INode> nodes, double tcurr);
    }
}
