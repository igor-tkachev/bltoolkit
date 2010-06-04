using System;
using System.Collections;
using System.Data;
using System.Reflection;
using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class DataAccessorBuilderTest : MarshalByRefObject
	{
		DataAccessorBuilderTest _localTest;
		AppDomain               _localDomain;

		public struct Person
		{
		}

		[TestFixtureSetUp]
		public void SetUp()
		{
			string path = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;

			_localDomain = AppDomain.CreateDomain("NewDomain");
			_localDomain.Load(typeof(DataAccessor).Assembly.GetName());
			_localTest = (DataAccessorBuilderTest)_localDomain.CreateInstanceFromAndUnwrap(path, GetType().FullName);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			AppDomain.Unload(_localDomain);
		}

		public abstract class TypelessAccessor : DataAccessor
		{
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			public abstract Hashtable Typeless();
		}

		private void Typeless()
		{
			// Can not determine object type for the method 'TypelessAccessor.Typeless'
			//
			DataAccessor.CreateInstance(typeof(TypelessAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void TypelessTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.Typeless);
		}

		public abstract class TypelessAccessor2 : DataAccessor
		{
			[SprocName("Person_SelectAll")]
			public abstract ArrayList Typeless();
		}

		private void Typeless2()
		{
			// Can not determine object type for the method 'TypelessAccessor2.Typeless'
			//
			DataAccessor.CreateInstance(typeof(TypelessAccessor2));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void Gen_SelectAllListException()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.Typeless2);
		}

		public abstract class MultiDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract IList SelectAll([Destination] IList list1, [Destination] IList list2);
		}

		private void MultiDestinationException()
		{
			// More then one parameter is marked as destination.
			//
			DataAccessor.CreateInstance(typeof(MultiDestinationAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void MultiDestinationExceptionTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.MultiDestinationException);
		}

		public abstract class ScalarDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract int SelectAll([Destination] int p);
		}

		private void ScalarDestinationException()
		{
			// ExecuteScalar destination must be an out or a ref parameter
			//
			DataAccessor.CreateInstance(typeof(ScalarDestinationAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void ScalarDestinationExceptionTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.ScalarDestinationException);
		}

		public abstract class IncompatibleScalarDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract int SelectAll([Destination] out string p);
		}

		private void IncompatibleScalarDestinationException()
		{
			// The return type 'System.Int32' of the method 'SelectAll'
			// is incompatible with the destination parameter type 'System.String'
			//
			DataAccessor.CreateInstance(typeof(IncompatibleScalarDestinationAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void IncompatibleScalarDestinationExceptionTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.IncompatibleScalarDestinationException);
		}

		public abstract class VoidDestinationAccessor : DataAccessor
		{
			[ObjectType(typeof(Person))]
			public abstract void SelectAll([Destination] int p);
		}

		private void VoidDestinationException()
		{
			// ExecuteNonQuery does not support the Destination attribute
			//
			DataAccessor.CreateInstance(typeof(VoidDestinationAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void VoidDestinationExceptionTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.VoidDestinationException);
		}

		public abstract class IllegalDataSetTableAccessor : DataAccessor
		{
			[DataSetTable(12345)]
			public abstract DataTable SelectAll();
		}

		private void IllegalDataSetTable()
		{
			// DataSetTable attribute may not be an index
			//
			DataAccessor.CreateInstance(typeof(IllegalDataSetTableAccessor));
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void IllegalDataSetTableTest()
		{
			AppDomain.CurrentDomain.DoCallBack(_localTest.IllegalDataSetTable);
		}
	}
}