using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Interface
{
    public interface ITransitionType
    {
        void onTimer(int iTime, out double dPercentage, out bool bCompleted);
    }
}
