using CourserWork.Core.Elements.Internals;
using CourserWork.Core.ModelObjects;
using CourserWork.Core.Models.Strategies;
using CourserWork.MessageRoutingNode.RulesImplementations.ModelObjects;
using CourserWork.MessageRoutingNode.RulesImplementations.Models.Strategies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.MessageRoutingNode.RulesImplementations.Elements.Internals
{
    public class GlobalMessageBearer : IModelObjectBearer
    {
        private int firstDirectionLimit = 0;
        private int secondDirectionLimit = 0;
        private MessageRoutingNodeStatisticsStrategy statisticsStrategy;
        public GlobalMessageBearer(MessageRoutingNodeStatisticsStrategy statisticsStrategy, int firstDirectionLimit, int secondDirectionLimit)
        {
            this.firstDirectionLimit = firstDirectionLimit;
            this.secondDirectionLimit = secondDirectionLimit;
            this.statisticsStrategy = statisticsStrategy;
        }

        public bool IsModelObjectAllowed(IModelObject modelObject) {
            MessageObject messageObject = (MessageObject)modelObject;
            MessageDirection direction = messageObject.GetDirection();

            int currentFirstDirectionMessagesAmount = statisticsStrategy.GetCurrentFirstDirectionMessagesAmount();
            int currentSecondDirectionMessagesAmount = statisticsStrategy.GetCurrentSecondDirectionMessagesAmount();

            if (direction == MessageDirection.First && currentFirstDirectionMessagesAmount >= firstDirectionLimit)
            {
                return false;
            }
            if (direction == MessageDirection.Second && currentSecondDirectionMessagesAmount >= secondDirectionLimit)
            {
                return false;
            }
            return true;
        }
    }
}
