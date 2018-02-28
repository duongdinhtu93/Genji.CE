using GenjiCore.Transitions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions
{
    public class TransitionType_Bounce : TransitionType_UserDefined
    {
        public TransitionType_Bounce(int iTransitionTime)
        {
            IList<TransitionElement> elements = (IList<TransitionElement>)new List<TransitionElement>();
            elements.Add(new TransitionElement(50.0, 100.0, InterpolationMethod.Accleration));
            elements.Add(new TransitionElement(100.0, 0.0, InterpolationMethod.Deceleration));
            this.setup(elements, iTransitionTime);
        }
    }
}
