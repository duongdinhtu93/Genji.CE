using GenjiCore.Transitions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Internal
{
    internal class ManagedType_Float : IManagedType
    {
        public Type getManagedType()
        {
            return typeof(float);
        }

        public object copy(object o)
        {
            return (object)(float)o;
        }

        public object getIntermediateValue(object start, object end, double dPercentage)
        {
            return (object)Utility.interpolate((float)start, (float)end, dPercentage);
        }
    }
}
