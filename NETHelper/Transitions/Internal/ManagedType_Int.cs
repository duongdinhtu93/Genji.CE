using GenjiCore.Transitions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Internal
{
    internal class ManagedType_Int : IManagedType
    {
        public Type getManagedType()
        {
            return typeof(int);
        }

        public object copy(object o)
        {
            return (object)(int)o;
        }

        public object getIntermediateValue(object start, object end, double dPercentage)
        {
            return (object)Utility.interpolate((int)start, (int)end, dPercentage);
        }
    }
}
