using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class SelectTest : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(db =>
			{
				var list = db.Person.ToList();
				Assert.Less(0, list.Count);
			});
		}

		[Test]
		public void Test2()
		{
			ForEachProvider(db =>
			{
				var list = (from p in db.Person select p).ToList();
				Assert.Less(0, list.Count);
			});
		}

		[Test]
		public void Test3()
		{
			ForEachProvider(db =>
			{
				var list = db.Person.Select(p => p).Select(p => p).ToList();
				Assert.Less(0, list.Count);
			});
		}

		void Func(Expression<Func<LambdaExpression, string>> func)
		{
			var exp = Expression.Parameter(typeof(Expression), "exp");

			/*
			this.Func(Expression.Lambda<Func<LambdaExpression, string>>(
				Expression.Property(
					Expression.Call(
						Expression.Property(
							exp,
							(MethodInfo) null//methodof(LambdaExpression.get_Parameters)
						),
						(MethodInfo) null// methodof(ReadOnlyCollection<ParameterExpression>.get_Item, ReadOnlyCollection<ParameterExpression>)
						,
						new Expression[] { Expression.Constant(0, typeof(int)) }),
					(MethodInfo) methodof(ParameterExpression.get_Name))
					,
					new ParameterExpression[] { exp }));
			*/
		}

		void Foo(int i)
		{
			Func(exp => exp.Parameters[0].Name);
		}

		[Test]
		public void Test___()
		{
			Foo(0);
			Foo(1);
		}
	}
}
