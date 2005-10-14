using System;

using NUnit.Framework;

using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class TypeFactoryTest
	{
		#region GetType_WithException

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
		public class BuilderAttribute : TypeBuilderAttribute
		{
			public BuilderAttribute(Type type)
			{
				if (type != null)
					_typeBuilder = (ITypeBuilder)Activator.CreateInstance(type);
			}

			private         ITypeBuilder _typeBuilder;
			public override ITypeBuilder  TypeBuilder
			{ 
				get { return _typeBuilder; }
			}
		}

		[Builder(null)]
		public class Object
		{
		}

		[Test]
		[ExpectedException(typeof(TypeBuilderException))]
		public void GetType_WithException()
		{
			TypeFactory.GetType(typeof(Object), null);
		}

		#endregion

		#region GetType

		public class TypeBuilder0 : TypeBuilderBase 
		{
			public override bool IsCompatible(ITypeBuilder typeBuilder)
			{
				return IsRelative(typeBuilder) == false;
			}
		}

		public class TypeBuilder1 : TypeBuilder0 {}
		public class TypeBuilder2 : TypeBuilder1 {}
		public class TypeBuilder3 : TypeBuilder0 {}
		public class TypeBuilder4 : TypeBuilder3 {}

		[Builder(typeof(TypeBuilder2))] public class Object1 {}
		[Builder(typeof(TypeBuilder4))] public class Object2 : Object1 {}
		[Builder(typeof(TypeBuilder3))] public class Object3 : Object2 {}

		[Test]
		public new void GetType()
		{
			TypeBuilderInfo info = TypeFactory.GetType(typeof(Object3), new TypeBuilder1());

			foreach (ITypeBuilder tb in info.TypeBuilders)
				Console.WriteLine(tb.GetType().FullName);

			Assert.AreEqual(typeof(TypeBuilder3), info.TypeBuilders[0].GetType());
			Assert.AreEqual(typeof(TypeBuilder2), info.TypeBuilders[1].GetType());
		}

		#endregion
	}
}
