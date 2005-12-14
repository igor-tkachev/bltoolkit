using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Aspects;
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
			_fooMethodInfo = typeof(InterceptorAspectTest).GetMethod("Foo");
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
			string ret;

			InterceptCallInfo ci = new InterceptCallInfo();

			ci.MethodInfo         = _fooMethodInfo;
			ci.ParameterValues[0] = i;
			ci.ParameterValues[1] = s;
			ci.ParameterValues[2] = ri;
			ci.ParameterValues[3] = rs;

			Interceptor(ci);

			i   = (int)   ci.ParameterValues[0];
			s   = (string)ci.ParameterValues[1];
			ri  = (int)   ci.ParameterValues[2];
			rs  = (string)ci.ParameterValues[3];
			ret = (string)ci.ReturnValue;

			ret = FooInternal(i, s, ref ri, ref rs);

			ci.ReturnValue        = ret;
			ci.ParameterValues[2] = ri;
			ci.ParameterValues[3] = rs;

			Interceptor(ci);

			ret = (string)ci.ReturnValue;
			ri  = (int)   ci.ParameterValues[2];
			rs  = (string)ci.ParameterValues[3];

			return ret;
		}

		[Test]
		public void Test()
		{

		}
	}
}
