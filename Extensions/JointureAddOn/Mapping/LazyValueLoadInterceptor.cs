using System;
using BLToolkit.Aspects;
using Castle.DynamicProxy;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace BLToolkit.Mapping
{
    public class LazyValueLoadInterceptor : IInterceptor
    {
        private readonly IObjectMapper _mapper;
        private readonly Func<IMapper, object, Type, object> _lazyLoader;

        public LazyValueLoadInterceptor(IObjectMapper mapper, Func<IMapper, object, Type, object> lazyLoader)
        {
            _mapper = mapper;
            _lazyLoader = lazyLoader;
        }

        private bool _intercepted;

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.StartsWith("get_"))
            {
                string propertyName = invocation.Method.Name.Substring(4);

                foreach (IMapper map in _mapper.PropertiesMapping)
                {
                    if (map.PropertyName == propertyName)
                    {
                        if (!_intercepted)
                        {
                            _intercepted = true;
                            map.Setter(invocation.Proxy, _lazyLoader(map, invocation.Proxy, invocation.TargetType));
                        }
                        break;
                    }
                }
            }

            // let the original call go through first, so we can notify *after*
            invocation.Proceed();
        }
    }

    public class PropInterceptor : Interceptor
    {
        protected override void BeforeCall(InterceptCallInfo info)
        {
            info.Items["ReturnValue"] = info.ReturnValue;
        }

        protected override void AfterCall(InterceptCallInfo info)
        {
            info.ReturnValue = (int)info.ReturnValue + (int)info.Items["ReturnValue"];
        }
    }
}