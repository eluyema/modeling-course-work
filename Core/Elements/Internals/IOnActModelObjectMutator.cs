using CourserWork.Core.ModelObjects;

namespace CourserWork.Core.Elements.Internals
{
    public interface IOnActModelObjectMutator
    {
        public void MutateModelObject(IModelObject modelObject);
    }
}
