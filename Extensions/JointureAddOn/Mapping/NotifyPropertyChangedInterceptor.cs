#region

using System.ComponentModel;
using Castle.DynamicProxy;

#endregion

namespace BLToolkit.Mapping
{
    public class NotifyPropertyChangedInterceptor : IInterceptor
    {
        private readonly string _typeName;
        private PropertyChangedEventHandler _subscribers = delegate { };

        public NotifyPropertyChangedInterceptor(string typeName)
        {
            _typeName = typeName;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.DeclaringType == typeof (TypeFactory.DataBindingFactory.IMarkerInterface))
            {
                invocation.ReturnValue = _typeName;
                return;
            }
            if (invocation.Method.DeclaringType == typeof (INotifyPropertyChanged))
            {
                var propertyChangedEventHandler = (PropertyChangedEventHandler) invocation.Arguments[0];
                if (invocation.Method.Name.StartsWith("add_"))
                {
                    _subscribers += propertyChangedEventHandler;
                }
                else
                {
                    _subscribers -= propertyChangedEventHandler;
                }
                return;
            }
            invocation.Proceed();
            if (invocation.Method.Name.StartsWith("set_"))
            {
                var propertyName = invocation.Method.Name.Substring(4);
                _subscribers(invocation.InvocationTarget, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}