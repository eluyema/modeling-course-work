using CourserWork.Core.Elements;
using CourserWork.Core.ModelObjects;
using CourserWork.Core.Nodes;
using CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.MessageRoutingNode.RulesImplementations.Nodes
{
    public class MessageBranchingNode : INode
    {
        private List<(MessageDirection, AbstractElement)> branches;
        private AbstractElement defaultElement;

        public MessageBranchingNode(List<(MessageDirection, AbstractElement)> branches, AbstractElement defaultElement)
        {
            this.branches = branches;
            this.defaultElement = defaultElement;
        }

        public void TriggerAct(IModelObject modelObject)
        {
            MessageObject messageObject = (MessageObject)modelObject;


            foreach (var (direction, element) in branches)
            {
                if (direction == messageObject.GetDirection())
                {
                    element.InAct(modelObject);
                    return;
                }
            }
            defaultElement.InAct(modelObject);
        }
    }
}
