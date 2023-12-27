using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core.ModelObjects
{
    public interface IModelObjectFabric
    {
        public IModelObject generateNextModelObject(double tcurr);

    }
}
