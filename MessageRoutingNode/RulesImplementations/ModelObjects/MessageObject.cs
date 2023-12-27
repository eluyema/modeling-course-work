using CourserWork.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects
{
    public enum MessageDirection
    {
        First,
        Second
    }

    public class MessageObject : IModelObject
    {
        private MessageDirection _diraction;

        public MessageObject(MessageDirection direction) {
            _diraction = direction;
        }

        public MessageDirection GetDirection() {
            return _diraction;
        }

        public void StartProcessing(string processName, double tcurr) {
        }
        public void FinishProcessing(string processName, double tcurr) {
        }
    }
}
