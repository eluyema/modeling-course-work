using CourserWork.Core.ModelObjects;

namespace CourserWork.Core.Elements.Internals
{
    public interface ITimeEstimator
    {
        public double GetEstimetedTime(IModelObject modelObject);
    }
}
