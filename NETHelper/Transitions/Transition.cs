using GenjiCore.Transitions.Interface;
using GenjiCore.Transitions.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenjiCore.Transitions
{
    public class Transition
    {
        private IList<Transition.TransitionedPropertyInfo> m_listTransitionedProperties = (IList<Transition.TransitionedPropertyInfo>)new List<Transition.TransitionedPropertyInfo>();
        private Stopwatch m_Stopwatch = new Stopwatch();
        private object m_Lock = new object();
        private static IDictionary<System.Type, IManagedType> m_mapManagedTypes = (IDictionary<System.Type, IManagedType>)new Dictionary<System.Type, IManagedType>();
        private ITransitionType m_TransitionMethod;

        static Transition()
        {
            Transition.registerType((IManagedType)new ManagedType_Int());
            Transition.registerType((IManagedType)new ManagedType_Float());
            Transition.registerType((IManagedType)new ManagedType_Double());
            Transition.registerType((IManagedType)new ManagedType_Color());
            Transition.registerType((IManagedType)new ManagedType_String());
        }

        public event EventHandler<Transition.Args> TransitionCompletedEvent;

        public static void run(object target, string strPropertyName, object destinationValue, ITransitionType transitionMethod)
        {
            Transition transition = new Transition(transitionMethod);
            transition.add(target, strPropertyName, destinationValue);
            transition.run();
        }

        public static void run(object target, string strPropertyName, object initialValue, object destinationValue, ITransitionType transitionMethod)
        {
            Utility.setValue(target, strPropertyName, initialValue);
            Transition.run(target, strPropertyName, destinationValue, transitionMethod);
        }

        public static void runChain(params Transition[] transitions)
        {
            TransitionChain transitionChain = new TransitionChain(transitions);
        }

        public Transition(ITransitionType transitionMethod)
        {
            this.m_TransitionMethod = transitionMethod;
        }

        public void add(object target, string strPropertyName, object destinationValue)
        {
            PropertyInfo property = target.GetType().GetProperty(strPropertyName);
            if (property == null)
                throw new Exception("Đối tượng: " + target.ToString() + " không có thuộc tính: " + strPropertyName);
            System.Type propertyType = property.PropertyType;
            if (!Transition.m_mapManagedTypes.ContainsKey(propertyType))
                throw new Exception("Transition không thể handle các thuộc tính thuộc kiểu: " + propertyType.ToString());
            if (!property.CanRead || !property.CanWrite)
                throw new Exception("Modifier của thuộc tính phải cho phép đọc hoặc ghi: " + strPropertyName);
            IManagedType mapManagedType = Transition.m_mapManagedTypes[propertyType];
            Transition.TransitionedPropertyInfo transitionedPropertyInfo = new Transition.TransitionedPropertyInfo();
            transitionedPropertyInfo.endValue = destinationValue;
            transitionedPropertyInfo.target = target;
            transitionedPropertyInfo.propertyInfo = property;
            transitionedPropertyInfo.managedType = mapManagedType;
            lock (this.m_Lock)
                this.m_listTransitionedProperties.Add(transitionedPropertyInfo);
        }

        public void run()
        {
            foreach (Transition.TransitionedPropertyInfo transitionedProperty in (IEnumerable<Transition.TransitionedPropertyInfo>)this.m_listTransitionedProperties)
            {
                object o = transitionedProperty.propertyInfo.GetValue(transitionedProperty.target, (object[])null);
                transitionedProperty.startValue = transitionedProperty.managedType.copy(o);
            }
            this.m_Stopwatch.Reset();
            this.m_Stopwatch.Start();
            TransitionManager.getInstance().register(this);
        }

        internal IList<Transition.TransitionedPropertyInfo> TransitionedProperties
        {
            get
            {
                return this.m_listTransitionedProperties;
            }
        }

        internal void removeProperty(Transition.TransitionedPropertyInfo info)
        {
            lock (this.m_Lock)
                this.m_listTransitionedProperties.Remove(info);
        }

        internal void onTimer()
        {
            double dPercentage;
            bool bCompleted;
            this.m_TransitionMethod.onTimer((int)this.m_Stopwatch.ElapsedMilliseconds, out dPercentage, out bCompleted);
            IList<Transition.TransitionedPropertyInfo> transitionedPropertyInfoList = (IList<Transition.TransitionedPropertyInfo>)new List<Transition.TransitionedPropertyInfo>();
            lock (this.m_Lock)
            {
                foreach (Transition.TransitionedPropertyInfo transitionedProperty in (IEnumerable<Transition.TransitionedPropertyInfo>)this.m_listTransitionedProperties)
                    transitionedPropertyInfoList.Add(transitionedProperty.copy());
            }
            foreach (Transition.TransitionedPropertyInfo transitionedPropertyInfo in (IEnumerable<Transition.TransitionedPropertyInfo>)transitionedPropertyInfoList)
            {
                object intermediateValue = transitionedPropertyInfo.managedType.getIntermediateValue(transitionedPropertyInfo.startValue, transitionedPropertyInfo.endValue, dPercentage);
                this.setProperty((object)this, new Transition.PropertyUpdateArgs(transitionedPropertyInfo.target, transitionedPropertyInfo.propertyInfo, intermediateValue));
            }
            if (!bCompleted)
                return;
            this.m_Stopwatch.Stop();
            Utility.raiseEvent<Transition.Args>(this.TransitionCompletedEvent, (object)this, new Transition.Args());
        }

        private void setProperty(object sender, Transition.PropertyUpdateArgs args)
        {
            try
            {
                if (this.isDisposed(args.target))
                    return;
                ISynchronizeInvoke target = args.target as ISynchronizeInvoke;
                if (target != null && target.InvokeRequired)
                    target.BeginInvoke((Delegate)new EventHandler<Transition.PropertyUpdateArgs>(this.setProperty), new object[2]
                    {
            sender,
            (object) args
                    }).AsyncWaitHandle.WaitOne(50);
                else
                    args.propertyInfo.SetValue(args.target, args.value, (object[])null);
            }
            catch (Exception ex)
            {
            }
        }

        private bool isDisposed(object target)
        {
            Control control = target as Control;
            return control != null && (control.IsDisposed || control.Disposing);
        }

        private static void registerType(IManagedType transitionType)
        {
            System.Type managedType = transitionType.getManagedType();
            Transition.m_mapManagedTypes[managedType] = transitionType;
        }

        public class Args : EventArgs
        {
        }

        internal class TransitionedPropertyInfo
        {
            public object startValue;
            public object endValue;
            public object target;
            public PropertyInfo propertyInfo;
            public IManagedType managedType;

            public Transition.TransitionedPropertyInfo copy()
            {
                return new Transition.TransitionedPropertyInfo()
                {
                    startValue = this.startValue,
                    endValue = this.endValue,
                    target = this.target,
                    propertyInfo = this.propertyInfo,
                    managedType = this.managedType
                };
            }
        }

        private class PropertyUpdateArgs : EventArgs
        {
            public object target;
            public PropertyInfo propertyInfo;
            public object value;

            public PropertyUpdateArgs(object t, PropertyInfo pi, object v)
            {
                this.target = t;
                this.propertyInfo = pi;
                this.value = v;
            }
        }
    }
}
