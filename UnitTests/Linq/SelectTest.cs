using System;
using System.Linq;
using System.Linq.Expressions;

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
				Assert.AreNotEqual(0, list.Count);
			});
		}

		[Test]
		public void Test2()
		{
			ForEachProvider(db =>
			{
				var list = (from p in db.Person select p).ToList();
				Assert.AreNotEqual(0, list.Count);
			});
		}

		void Func(Expression<Func<Expression, int>> func)
		{
			var exp = Expression.Parameter(typeof(Expression), "exp");

			/*
			this.Func(
				Expression.Lambda<Func<Expression, int>>(
					Expression.Property(
						Expression.Property(
							Expression.Convert(
								Expression.Call(
									Expression.Property(
										Expression.Convert(exp, typeof(MethodCallExpression)),
										"Arguments"),
									(System.Reflection.MethodInfo)null, //"Item",
									new Expression[] { Expression.Constant(0, typeof(int)) }), typeof(MethodCallExpression)),
								(MethodInfo) methodof(MethodCallExpression.get_Arguments)),
								(MethodInfo) methodof(ReadOnlyCollection<Expression>.get_Count,
								ReadOnlyCollection<Expression>)
					),
					new ParameterExpression[] { CS$0$0000 }
				)
			);
			*/
		}

		void Foo(int i)
		{
			Func((exp) => ((MethodCallExpression)((MethodCallExpression)exp).Arguments[0]).Arguments.Count);
		}

		[Test]
		public void Test___()
		{
			Foo(0);
			Foo(1);
		}
	}
}
