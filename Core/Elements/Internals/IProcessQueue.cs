using CourserWork.Core.ModelObjects;

namespace CourserWork.Core.Elements.Internals
{
    public interface IProcessQueue
    {
        public bool TryToEnqueue(IModelObject modelObject);

        public int Count { get; }

        public IModelObject Dequeue();

        public List<IModelObject> GetListModelsObject();
    }
}
