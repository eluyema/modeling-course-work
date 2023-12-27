using CourserWork.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core.Elements.Internals
{
    public interface IModelObjectBearer
    {
        public bool IsModelObjectAllowed(IModelObject modelObject);
    }
}
