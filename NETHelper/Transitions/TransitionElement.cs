using GenjiCore.Transitions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions
{
    public class TransitionElement
    {
        public TransitionElement(double endTime, double endValue, InterpolationMethod interpolationMethod)
        {
            this.EndTime = endTime;
            this.EndValue = endValue;
            this.InterpolationMethod = interpolationMethod;
        }

        public double EndTime { get; set; }

        public double EndValue { get; set; }

        public InterpolationMethod InterpolationMethod { get; set; }
    }
}
