using System;

using NUnit.Framework;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class IAbstractTypeBuilder
	{
		public class AbstractTypeBuilder : BLToolkit.TypeBuilder.IAbstractTypeBuilder
		{
			public bool IsCompatible (ITypeBuilder typeBuilder)
			{
				return true;
			}

			public Type[] GetInterfaces() 
			{
				return null;
			}

			public void BeforeBuild(TypeBuilderContext context)
			{
				ConstructorBuilderHelper cb = context.TypeBuilder.DefaultConstructor;
			}

			public void AfterBuild(TypeBuilderContext context)
			{
			}
		}

		public abstract class Object
		{
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;

			TypeBuilderInfo info = TypeFactory.GetType(typeof(Object), new AbstractTypeBuilder());

			Console.WriteLine(info.Type.Type);
		}
	}
}
