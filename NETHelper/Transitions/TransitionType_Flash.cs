using GenjiCore.Transitions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions
{
    public class TransitionType_Flash : TransitionType_UserDefined
    {
        public TransitionType_Flash(int iNumberOfFlashes, int iFlashTime)
        {
            double num1 = 100.0 / (double)iNumberOfFlashes;
            IList<TransitionElement> elements = (IList<TransitionElement>)new List<TransitionElement>();
            for (int index = 0; index < iNumberOfFlashes; ++index)
            {
                double num2 = (double)index * num1;
                double endTime1 = num2 + num1;
                double endTime2 = (num2 + endTime1) / 2.0;
                elements.Add(new TransitionElement(endTime2, 100.0, InterpolationMethod.EaseInEaseOut));
                elements.Add(new TransitionElement(endTime1, 0.0, InterpolationMethod.EaseInEaseOut));
            }
            this.setup(elements, iFlashTime * iNumberOfFlashes);
        }
    }
}
