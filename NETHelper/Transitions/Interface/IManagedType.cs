using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Interface
{
    internal interface IManagedType
    {
        Type getManagedType();

        object copy(object o);

        object getIntermediateValue(object start, object end, double dPercentage);
    }
}
