/// example:
/// mapsettable SetField(string,object)
/// comment:
/// The following example demonstrates how to use the <b>IMapSettable</b> interface.
using System;
using System.Collections;

using NUnit.Framework;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_IMapSettable
{
	[TestFixture]
	public class SetField_string_object
	{
		// Base business entity class.
		//
		public abstract class BizEntityBase : IMapSettable
		{
			private Guid _id;
			public  Guid  ID
			{
				get { return _id; }
			}

			#region IMapSettable Members

			bool IMapSettable.SetField(string fieldName, object value)
			{
				if (string.Compare(fieldName, GetType().Name + "ID") == 0)
				{
					_id = (Guid)value;

					return true;
				}

				return false;
			}

			#endregion
		}

		// Business entity.
		//
		public class Customer : BizEntityBase
		{
			public string Name;
			public string Description;
		}

		[Test]
		public void Test()
		{
			Customer customer;

			using (DbManager db = new DbManager())
			{
				customer = (Customer)db
					.SetCommand(@"
						SELECT
							NewID() as CustomerID,
							'RSDN'  as Name,
							'Number one Russian resource for software developers!' as Description")
					.ExecuteObject(typeof(Customer));
			}

			Console.WriteLine("ID:          {0}", customer.ID);
			Console.WriteLine("Name:        {0}", customer.Name);
			Console.WriteLine("Description: {0}", customer.Description);

			Assert.IsFalse(customer.ID == Guid.Empty);
		}
	}
}
