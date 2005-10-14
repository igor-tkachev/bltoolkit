using System;

using NUnit.Framework;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class IAbstractTypeBuilder
	{
		public interface ITest
		{
			void Method();
			Type Property { get; }
		}

		public class AbstractTypeBuilder : BLToolkit.TypeBuilder.IAbstractTypeBuilder
		{
			public bool IsCompatible (ITypeBuilder typeBuilder)
			{
				return true;
			}

			public Type[] GetInterfaces() 
			{
				return new Type[] { typeof(ITest) };
			}

			public void BeforeBuild(TypeBuilderContext context)
			{
				ConstructorBuilderHelper cb = context.TypeBuilder.DefaultConstructor;
			}

			public void AfterBuild(TypeBuilderContext context)
			{
			}

			public void BuildInterfaceMethod(TypeBuilderContext context)
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
