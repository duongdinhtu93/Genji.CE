using GenjiCore.Transitions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions
{
    public class TransitionType_Acceleration : ITransitionType
    {
        private double m_dTransitionTime;

        public TransitionType_Acceleration(int iTransitionTime)
        {
            if (iTransitionTime <= 0)
                throw new Exception("Thời gian effect phải > 0.");
            this.m_dTransitionTime = (double)iTransitionTime;
        }

        public void onTimer(int iTime, out double dPercentage, out bool bCompleted)
        {
            double num = (double)iTime / this.m_dTransitionTime;
            dPercentage = num * num;
            if (num >= 1.0)
            {
                dPercentage = 1.0;
                bCompleted = true;
            }
            else
                bCompleted = false;
        }
    }
}
