using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class DefaultTypeBuilder
	{
		public abstract class Object
		{
			public    abstract int       Int       { get; set; }
			public    abstract double    Double    { get; set; }
			public    abstract DateTime  DateTime  { get; set; }
			public    abstract ArrayList ArrayList { get; set; }

			protected abstract string this[int i]    { get; set; }
			protected abstract int    this[string i] { get; set; }

			protected abstract int    Method1(float f);
			public    abstract int    Method2(float f);
		}

		[Test]
		public void AbstractProperties()
		{
			TypeFactory.SaveTypes = true;

			BuildContext context = TypeFactory.GetType(typeof(Object));

			Console.WriteLine(context.Type.Type);

			Object o = (Object)Activator.CreateInstance(context.Type);

			o.Int    = 100; Assert.AreEqual(100, o.Int);
			o.Double = 200; Assert.AreEqual(200, o.Double);

			ArrayList list = new ArrayList();

			o.ArrayList = list;
			Assert.AreSame(list, o.ArrayList);
		}

		public class AbstractTypeBuilder : AbstractTypeBuilderBase
		{
			public override bool IsApplied(BuildContext context)
			{
				return context.IsVirtualMethod && context.Step == BuildStep.After;
			}

			protected override void AfterBuildVirtualMethod()
			{
				Context.MethodBuilder.Emitter
					.ldc_i4 (25)
					.stloc  (Context.ReturnValue);
			}

			protected override void BuildType()
			{
				ConstructorBuilderHelper cb = Context.TypeBuilder.DefaultConstructor;

				cb = Context.TypeBuilder.TypeInitializer;
				cb = Context.TypeBuilder.InitConstructor;
			}
		}

		public abstract class VirtObject
		{
			public virtual int Foo(int i, ref int ii, DateTime d, string s)
			{
				ii = i;
				return 50;
			}
		}

		[Test]
		public void AbstractMethod()
		{
			TypeFactory.SaveTypes = true;

			BuildContext context = TypeFactory.GetType(typeof(VirtObject), new AbstractTypeBuilder());

			Console.WriteLine(context.Type.Type);

			VirtObject o = (VirtObject)Activator.CreateInstance(context.Type);

			int i = 0;
			int r = o.Foo(10, ref i, DateTime.Now, "");

			Assert.AreEqual(10, i);
			Assert.AreEqual(25, r);
		}
	}
}
