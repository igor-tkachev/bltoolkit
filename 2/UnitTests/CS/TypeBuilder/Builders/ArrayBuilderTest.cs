using System;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class ArrayBuilderTest
	{
		public ArrayBuilderTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject
		{
			public abstract int[]    IntArray1 { get; set; }
			[LazyInstance]
			public abstract int[]    IntArray2 { get; set; }
			public abstract byte[][] ByteArray { get; set; }

			// There is a bug in the CLR.
			// In the file "Rotor2\sscli\clr\src\bcl\system\reflection\emit\signaturehelper.cs" we have:
			//
			// else if (clsArgument.IsArray)
			// {
			//   if (clsArgument.IsSzArray)
			//   {
			//     AddElementType(ELEMENT_TYPE_SZARRAY);
			//
			//     AddOneArgTypeHelper(clsArgument.GetElementType());
			//   }
			//   else
			//   {
			//     AddElementType(ELEMENT_TYPE_ARRAY);
			//
			//     AddOneArgTypeHelper(clsArgument.GetElementType());
			//
			//     // put the rank information
			//     AddData(clsArgument.GetArrayRank());
			//
			//     AddData(0);
			//     AddData(0);
			//   }
			// }
			//
			// As we can see, there is NO record for bounds. And we nothing can do with it.
			//
			// To bite this CLR bug we need to do the sunset by our own hands.
			//
			private int[,]          _dimArray = new int[1,1];
			public  int[,]           DimArray
			{
				get { return _dimArray;  }
				set
				{
					 _dimArray = value;
					// IsDirty = true;
				}
			}
		}

		[Test]
		public void AbstractProperties()
		{
			TestObject o = (TestObject)TypeAccessor.CreateInstance(typeof(TestObject));

			Assert.IsNotNull(o.IntArray1);
			Assert.AreSame(o.IntArray1, o.IntArray2);

			Assert.IsNotNull(o.ByteArray);
		}
	}
}
