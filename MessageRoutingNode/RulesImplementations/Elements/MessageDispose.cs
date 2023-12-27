using CourserWork.Core.Elements;
using CourserWork.Core.ModelObjects;

namespace CourserWork.MessageRoutingNode.RulesImplementations.Elements
{
    public class MessageDispose : AbstractDispose
    {
        private int quantity;
        private double oneMessagePrice;

        public MessageDispose(string disposeName, double oneMessagePrice) : base(disposeName)
        {
            quantity = 0;
            this.oneMessagePrice = oneMessagePrice;
        }

        public override void InAct(IModelObject modelObject)
        {
            quantity++;

        }

        public int GetQuantity()
        {
            return quantity;
        }

        public double GetOneMessagePrice() {
            return oneMessagePrice;
        }

        public double GetRevenue()
        {
            return quantity * oneMessagePrice;
        }
    }
}
