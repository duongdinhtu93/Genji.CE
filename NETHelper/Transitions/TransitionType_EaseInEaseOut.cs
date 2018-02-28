using GenjiCore.Transitions.Interface;
using GenjiCore.Transitions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions
{
    public class TransitionType_EaseInEaseOut : ITransitionType
    {
        private double m_dTransitionTime;

        public TransitionType_EaseInEaseOut(int iTransitionTime)
        {
            if (iTransitionTime <= 0)
                throw new Exception("Thời gian effect phải > 0.");
            this.m_dTransitionTime = (double)iTransitionTime;
        }

        public void onTimer(int iTime, out double dPercentage, out bool bCompleted)
        {
            double dElapsed = (double)iTime / this.m_dTransitionTime;
            dPercentage = Utility.convertLinearToEaseInEaseOut(dElapsed);
            if (dElapsed >= 1.0)
            {
                dPercentage = 1.0;
                bCompleted = true;
            }
            else
                bCompleted = false;
        }
    }
}
