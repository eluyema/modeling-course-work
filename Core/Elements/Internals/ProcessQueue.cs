using CourserWork.Core.ModelObjects;

namespace CourserWork.Core.Elements.Internals
{
    public class ProcessQueue : IProcessQueue
    {
        private Queue<IModelObject> queue;

        public ProcessQueue()
        {
            this.queue = new Queue<IModelObject>();
        }

        public bool TryToEnqueue(IModelObject modelObject)
        {
            queue.Enqueue(modelObject);
            return true;
        }

        public List<IModelObject> GetListModelsObject() {
            List<IModelObject> list = queue.ToList();
            return list;
        }

        public int Count { get { return queue.Count; } }

        public IModelObject Dequeue()
        {
            return queue.Dequeue();
        }
    }
}
