using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class DataAccessorBuilderTest
	{
		public abstract class TypelessAccessor : DataAccessor
		{
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			public abstract Hashtable Typeless();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void TypelessTest()
		{
			// Exception here:
			// Can not determine object type for the method 'TypelessAccessor.Typeless'
			TypelessAccessor da = (TypelessAccessor)DataAccessor.CreateInstance(typeof(TypelessAccessor));

			da.Typeless();
		}
	}
}
