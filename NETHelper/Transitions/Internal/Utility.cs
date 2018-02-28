using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Transitions.Internal
{
    internal class Utility
    {
        public static object getValue(object target, string strPropertyName)
        {
            PropertyInfo property = target.GetType().GetProperty(strPropertyName);
            if (property == null)
                throw new Exception("Object: " + target.ToString() + " does not have the property: " + strPropertyName);
            return property.GetValue(target, (object[])null);
        }

        public static void setValue(object target, string strPropertyName, object value)
        {
            PropertyInfo property = target.GetType().GetProperty(strPropertyName);
            if (property == null)
                throw new Exception("Object: " + target.ToString() + " does not have the property: " + strPropertyName);
            property.SetValue(target, value, (object[])null);
        }

        public static double interpolate(double d1, double d2, double dPercentage)
        {
            double num = (d2 - d1) * dPercentage;
            return d1 + num;
        }

        public static int interpolate(int i1, int i2, double dPercentage)
        {
            return (int)Utility.interpolate((double)i1, (double)i2, dPercentage);
        }

        public static float interpolate(float f1, float f2, double dPercentage)
        {
            return (float)Utility.interpolate((double)f1, (double)f2, dPercentage);
        }

        public static double convertLinearToEaseInEaseOut(double dElapsed)
        {
            double num1 = dElapsed > 0.5 ? 0.5 : dElapsed;
            double num2 = dElapsed > 0.5 ? dElapsed - 0.5 : 0.0;
            return 2.0 * num1 * num1 + 2.0 * num2 * (1.0 - num2);
        }

        public static double convertLinearToAcceleration(double dElapsed)
        {
            return dElapsed * dElapsed;
        }

        public static double convertLinearToDeceleration(double dElapsed)
        {
            return dElapsed * (2.0 - dElapsed);
        }

        public static void raiseEvent<T>(EventHandler<T> theEvent, object sender, T args) where T : EventArgs
        {
            if (theEvent == null)
                return;
            foreach (EventHandler<T> invocation in theEvent.GetInvocationList())
            {
                try
                {
                    ISynchronizeInvoke target = invocation.Target as ISynchronizeInvoke;
                    if (target == null || !target.InvokeRequired)
                        invocation(sender, args);
                    else
                        target.BeginInvoke((Delegate)invocation, new object[2]
                        { sender, (object) args });
                }
                //Kệ mẹ lỗi
                catch (Exception ex)
                {
                }
            }
        }
    }
}
