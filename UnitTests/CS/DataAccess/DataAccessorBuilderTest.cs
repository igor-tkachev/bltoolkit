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
		public struct Person
		{
		}

		public abstract class TypelessAccessor : DataAccessor
		{
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			public abstract Hashtable Typeless();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void TypelessTest()
		{
			// Can not determine object type for the method 'TypelessAccessor.Typeless'
			//
			DataAccessor.CreateInstance(typeof(TypelessAccessor));
		}

		public abstract class TypelessAccessor2 : DataAccessor
		{
			[SprocName("Person_SelectAll")]
			public abstract ArrayList Typeless();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void Gen_SelectAllListException()
		{
			// Can not determine object type for the method 'TypelessAccessor2.Typeless'
			//
			DataAccessor.CreateInstance(typeof(TypelessAccessor2));
		}

		public abstract class IListDataAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract IList SelectAllIList();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void IListException()
		{
			// Can not create an instance of the type 'System.Collections.IList'
			//
			DataAccessor.CreateInstance(typeof(IListDataAccessor));
		}

		public abstract class MultiDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract IList SelectAll([Destination] IList list1, [Destination] IList list2);
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void MultiDestinationException()
		{
			// More then one parameter is marked as destination.
			//
			DataAccessor.CreateInstance(typeof(MultiDestinationAccessor));
		}

		public abstract class ScalarDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract int SelectAll([Destination] int p);
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void ScalarDestinationException()
		{
			// ExecuteScalar destination must be an out or a ref parameter
			//
			DataAccessor.CreateInstance(typeof(ScalarDestinationAccessor));
		}

		public abstract class IncompatibleScalarDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract int SelectAll([Destination] out string p);
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void IncompatibleScalarDestinationException()
		{
			// The return type 'System.Int32' of the method 'SelectAll'
			// is incompatible with the destination parameter type 'System.String'
			//
			DataAccessor.CreateInstance(typeof(IncompatibleScalarDestinationAccessor));
		}

		public abstract class VoidDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract void SelectAll([Destination] int p);
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void VoidDestinationException()
		{
			// ExecuteNonQuery does not support the Destination attribute
			//
			DataAccessor.CreateInstance(typeof(VoidDestinationAccessor));
		}
	}
}
