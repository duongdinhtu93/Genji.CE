using GenjiCore.Transitions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Internal
{
    internal class ManagedType_Double : IManagedType
    {
        public Type getManagedType()
        {
            return typeof(double);
        }

        public object copy(object o)
        {
            return (object)(double)o;
        }

        public object getIntermediateValue(object start, object end, double dPercentage)
        {
            return (object)Utility.interpolate((double)start, (double)end, dPercentage);
        }
    }
}
