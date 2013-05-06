using System;
using System.ComponentModel;
using Castle.DynamicProxy;

namespace BLToolkit.Mapping
{
    public static class TypeFactory
    {
        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        public static class LazyLoading
        {
            public static object Create(Type type, IObjectMapper mapper, Func<IMapper, object, Type, object> lazyLoader)
            {
                return ProxyGenerator.CreateClassProxy(type, new LazyValueLoadInterceptor(mapper, lazyLoader));
            }
        }

        public static class LazyLoadingWithDataBinding
        {
            public static object Create(Type type, IObjectMapper mapper, Func<IMapper, object, Type, object> lazyLoader)
            {
                return ProxyGenerator.CreateClassProxy(type, new[]
                    {
                        typeof (INotifyPropertyChanged),
                        typeof (DataBindingFactory.IMarkerInterface)
                    }, new LazyValueLoadInterceptor(mapper, lazyLoader), new NotifyPropertyChangedInterceptor(type.FullName));
            }
        }

        public static class DataBindingFactory
        {
            public static object Create(Type type)
            {
                return ProxyGenerator.CreateClassProxy(type, new[]
                    {
                        typeof (INotifyPropertyChanged),
                        typeof (IMarkerInterface)
                    }, new NotifyPropertyChangedInterceptor(type.FullName));
            }

            public interface IMarkerInterface
            {
                string TypeName { get; }
            }
        }
    }
}