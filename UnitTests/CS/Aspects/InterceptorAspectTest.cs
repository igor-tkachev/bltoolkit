using System;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
	public class InterceptorAspectTest
	{
		public InterceptorAspectTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public class TestInterceptor : IInterceptor
		{
			public void Intercept(InterceptCallInfo info)
			{
				if (info.CallMethodInfo.MethodInfo.ReturnType == typeof(int))
					info.ReturnValue = 10;
			}
		}

		[Interceptor(typeof(TestInterceptor), InterceptType.BeforeCall | InterceptType.OnCatch)]
		public abstract class TestClass
		{
			public abstract int Test(int i1, ref int i2, out int i3, out decimal d4);

			[NoInterception(typeof(TestInterceptor), InterceptType.BeforeCall | InterceptType.OnCatch)]
			public abstract int TestNo();

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

			[Interceptor(
				typeof(PropInterceptor), InterceptType.BeforeCall | InterceptType.AfterCall,
				TypeBuilderConsts.Priority.Normal - 100)]
			public virtual int Prop { get { return 50; } }
		}

		[Test]
		public void Test()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			int     i2 = 2;
			int     i3;
			decimal d4;

			Assert.AreEqual(10, t.Test(1, ref i2, out i3, out d4));
			Assert.AreEqual(0,  t.TestNo());
			Assert.AreEqual(60, t.Prop);
		}
	}
}
