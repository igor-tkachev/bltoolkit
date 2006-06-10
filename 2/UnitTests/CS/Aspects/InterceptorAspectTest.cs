using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	[TestFixture]
	public class InterceptorAspectTest
	{
		public InterceptorAspectTest()
		{
			TypeFactory.SaveTypes = true;
		}

		static InterceptorAspectTest()
		{
			/*
			ParameterModifier pm = new ParameterModifier(4);

			pm[2] = true;
			pm[4] = true;

			_fooMethodInfo = typeof(InterceptorAspectTest).
				GetMethod("Foo",
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				CallingConventions.Any,
				new Type[]
				{
					typeof(int),
					typeof(string),
					typeof(int),
					typeof(string)
				},
				new ParameterModifier[] { pm }
				);
			 */
		}

		public virtual string FooInternal(int i, string s, ref int ri, ref string rs)
		{
			ri = 234;
			rs = "345";

			return "123";
		}

		public void Interceptor(InterceptCallInfo ci)
		{
		}

		private static MethodInfo _fooMethodInfo;

		public virtual string Foo(int i, string s, ref int ri, ref string rs)
		{
			if (_fooMethodInfo == null)
				_fooMethodInfo = (MethodInfo)new System.Diagnostics.StackTrace(0).GetFrame(0).GetMethod();

			string ret = null;
			InterceptCallInfo ci = new InterceptCallInfo();

			try
			{

				ci.MethodInfo = _fooMethodInfo;
				ci.ParameterValues[0] = i;
				ci.ParameterValues[1] = s;
				ci.ParameterValues[2] = ri;
				ci.ParameterValues[3] = rs;

				Interceptor(ci);

				i = (int)ci.ParameterValues[0];
				s = (string)ci.ParameterValues[1];
				ri = (int)ci.ParameterValues[2];
				rs = (string)ci.ParameterValues[3];
				ret = (string)ci.ReturnValue;

				ret = FooInternal(i, s, ref ri, ref rs);

				if (ci.InterceptResult != InterceptResult.Return)
					return ret;

				ci.ReturnValue = ret;
				ci.ParameterValues[2] = ri;
				ci.ParameterValues[3] = rs;

				Interceptor(ci);

				ret = (string)ci.ReturnValue;
				ri = (int)ci.ParameterValues[2];
				rs = (string)ci.ParameterValues[3];
			}
			catch (Exception)
			{
				Interceptor(ci);
			}

			return ret;
		}

		public class TestInterceptor : IInterceptor
		{
			public void Intercept(InterceptCallInfo info)
			{
				if (info.MethodInfo.ReturnType == typeof(int))
					info.ReturnValue = 10;
			}
		}

		[Interceptor(typeof(TestInterceptor), InterceptType.BeforeCall | InterceptType.OnCatch)]
		public abstract class TestClass
		{
			public abstract int Test(int i1, ref int i2, out int i3);
		}

		[Test]
		public void Test()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			int i2 = 2;
			int i3;

			Assert.AreEqual(10, t.Test(1, ref i2, out i3));
		}
	}
}
