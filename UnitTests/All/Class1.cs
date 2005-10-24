using System;
using System.Collections;

using BLToolkit.TypeBuilder.Builders;

namespace UnitTests.All
{
	public class Class1
	{
		static object[] parameters;

		static Class1()
		{
			Type[] ps;

			ps = Type.EmptyTypes;

			parameters = DefaultTypeBuilder.GetPropertyParameters(
				typeof(string), "propertyName", typeof(int), ps);

			ps = new Type[] { typeof(float), typeof(decimal) };

			parameters = DefaultTypeBuilder.GetPropertyParameters(
				typeof(string), "propertyName", typeof(int), ps);
		}

		private void Foo(int p1, float p2)
		{
		}

		private void EnsureInstance_List()
		{
			int p1 = 10;
			int p2 = 10;

			Foo(p1, p2);
		}
	}
}
