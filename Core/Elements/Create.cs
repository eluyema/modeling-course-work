using CourserWork.Core.Elements.Internals;
using CourserWork.Core.ModelObjects;
using CourserWork.Core.Nodes;

namespace CourserWork.Core.Elements
{
    public class Create : AbstractElement
    {
        private IModelObjectFabric modelFabric;
        private INode? node;
        private ITimeEstimator? timeEstimator;
        private int quantity;

        public Create(string name, double delay,
            IModelObjectFabric modelFabric) : base(name, delay)
        {
            this.modelFabric = modelFabric;
            this.node = null;
            this.quantity = 0;
            SetTnext(0.0);
        }

        public override void OutAct()
        {

            base.OutAct();

            double tcurr = GetTcurr();

            IModelObject modelObject = modelFabric.generateNextModelObject(tcurr);

            SetTnext(tcurr + GetDelay(modelObject));
            quantity++;

            node?.TriggerAct(modelObject);

        }

        public void SetTimeEstimator(ITimeEstimator timeEstimator)
        {
            this.timeEstimator = timeEstimator;
        }


        public double GetDelay(IModelObject modelObject)
        {
            if (timeEstimator is not null)
            {
                return timeEstimator.GetEstimetedTime(modelObject);
            }
            return base.GetDelay();
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public void SetNode(INode node)
        {
            this.node = node;
        }
    }
}
