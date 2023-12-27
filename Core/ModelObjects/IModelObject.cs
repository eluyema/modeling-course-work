using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core.ModelObjects
{
    public interface IModelObject
    {
        public void StartProcessing(string processName, double tcurr);
        public void FinishProcessing(string processName, double tcurr);
    }
}
