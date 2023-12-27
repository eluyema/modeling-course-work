using CourserWork.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects
{
    public class MessageObjectFabric : IModelObjectFabric
    {
        private readonly MessageDirection _direction;

        public MessageObjectFabric(MessageDirection direction) {
            _direction = direction;
        }

        public IModelObject generateNextModelObject(double tcurr) {
            return new MessageObject(_direction);
        }
    }
}
