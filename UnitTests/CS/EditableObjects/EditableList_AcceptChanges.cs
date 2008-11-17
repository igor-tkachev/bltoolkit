using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace A.EditableObjects
{
	//[TestFixture]
	public class EditableList_AcceptChanges
	{
		public static bool IsAcceptChangesCallForPersonPhone = false;

		public abstract class _DomainObject : EditableObject
		{
			[PrimaryKey(0), NonUpdatable]
			[NullValue()]
			public abstract int ID { get; set; }

			public override void AcceptChanges()
			{
				base.AcceptChanges();

				if (typeof(_PersonPhone).IsAssignableFrom(GetType()))
					IsAcceptChangesCallForPersonPhone = true;
			}
		}

		public abstract class _PersonPhone : _DomainObject, IEditable
		{
			public abstract string Number_PersonPhone { get; set; }
		}

		public abstract class _Person : _DomainObject
		{
			public abstract EditableList<_PersonPhone> Phones { get; set; }
		}

		[Test]
		public void Test_EditableList_AcceptChanges()
		{
			_Person person = TypeAccessor<_Person>.CreateInstance();

			person.Phones.AddRange(new _PersonPhone[]
			{
				TypeAccessor<_PersonPhone>.CreateInstanceEx(),
				TypeAccessor<_PersonPhone>.CreateInstanceEx()
			});

			person.Phones[1].Number_PersonPhone = "222-22-22";

			person.AcceptChanges();

			Assert.IsFalse(person.IsDirty);
			Assert.IsTrue(IsAcceptChangesCallForPersonPhone);
		}
	}
}
