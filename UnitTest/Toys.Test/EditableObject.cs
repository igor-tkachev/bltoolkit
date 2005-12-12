using System;

using NUnit.Framework;

using Rsdn.Framework.EditableObject;
using Rsdn.Framework.Data.Mapping;

namespace Toys.Test
{
	[TestFixture]
	public class EditableObject
	{
		public class Source
		{
			public int    ID   = 10;
			public string Name = "20";
		}

		public abstract class Dest: EditableObjectBase
		{
			public string ChangedPropertyName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			protected override void OnPropertyChanged(MapPropertyInfo pi)
			{
				ChangedPropertyName = pi.PropertyName;
			}
		}

		[Test]
		public void Notification()
		{
			Dest o = (Dest)Map.ToObject(new Source(), typeof(Dest));

			Assert.IsNull(o.ChangedPropertyName);

			o.ID = 1;

			Assert.AreEqual("ID", o.ChangedPropertyName);
		}

		public abstract class Object1 : EditableObjectBase
		{
			public Object1()
			{
			}

			[MapField("ObjectId")]
			public abstract int      ID      { get;  }
			public abstract short    Field1  { get; set; }

			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public abstract bool     Field2  { get; set; }

			public abstract DateTime Field3  { get; set; }
			public abstract long     Field4  { get; set; }
			public abstract byte     Field5  { get; set; }
			public abstract char     Field6  { get; set; }
			public abstract ushort   Field7  { get; set; }
			public abstract uint     Field8  { get; set; }
			public abstract ulong    Field9  { get; set; }
			public abstract sbyte    Field10 { get; set; }
			public abstract float    Field11 { get; set; }
			public abstract double   Field12 { get; set; }
			public abstract decimal  Field13 { get; set; }
			public abstract string   Field14 { get; set; }
			public abstract Guid     Field15 { get; set; }

			public static Object1 CreateInstance()
			{
				return (Object1)Map.CreateInstance(typeof(Object1));
			}
		}

		[Test]
		public void TestCreate()
		{
			Object1.CreateInstance();

			Map.CreateInstance(typeof(Person));
		}

		public abstract class Person : EditableObjectBase
		{
			public abstract int      ID        { get; }
			public abstract string   FirstName { get; set; }
			public abstract string   LastName  { get; set; }
			public abstract DateTime Birthday  { get; set; }
		}

		public class PersonGen : Person, IEditable, IMapGenerated
		{
			public PersonGen()
			{
			}

			public PersonGen(MapInitializingData data)
			{
			}

			private EditableValue<int> _ID = new EditableValue<int>();
			public  override      int   ID
			{
				get { return _ID.Value; }
				/* set { _ID.Value = value; PropertyChanged(_ID_MapPropertyInfo); } */
			}

			private EditableValue<string> _FirstName = new EditableValue<string>("");
			public  override      string   FirstName
			{
				get { return _FirstName.Value; }
				set
				{
					_FirstName.Value = value;
					 ((IMapNotifyPropertyChanged) this).PropertyChanged(_FirstName_MapPropertyInfo);
				}
			}

			private EditableValue<string> _LastName = new EditableValue<string>("");
			public  override      string   LastName
			{
				get { return _LastName.Value; }
				set
				{
					_LastName.Value = value;
					 ((IMapNotifyPropertyChanged) this).PropertyChanged(_LastName_MapPropertyInfo);
				}
			}

			private EditableValue<DateTime> _Birthday = new EditableValue<DateTime>();
			public  override      DateTime   Birthday
			{
				get { return _Birthday.Value; }
				set
				{
					_Birthday.Value = value;
					 ((IMapNotifyPropertyChanged) this).PropertyChanged(_Birthday_MapPropertyInfo);
				}
			}

			object[] IMapGenerated.GetCreatedMembers()
			{
				return new object[] { this._ID, this._FirstName, this._LastName, this._Birthday } ;
			}

			void IEditable.AcceptChanges()
			{
				_ID.       AcceptChanges();
				_FirstName.AcceptChanges();
				_LastName. AcceptChanges();
				_Birthday. AcceptChanges();
			}

			void IEditable.RejectChanges()
			{
				_ID.       RejectChanges();
				_FirstName.RejectChanges();
				_LastName. RejectChanges();
				_Birthday. RejectChanges();
			}

			bool IEditable.IsDirty
			{
				get
				{
					return _ID.IsDirty || _FirstName.IsDirty || _LastName.IsDirty || _Birthday.IsDirty;
				}
			}

			bool IEditable.IsDirtyMember(string memberName, MapPropertyInfo pi, ref bool isDirty)
			{
				return true;
					//_ID.       IsDirtyMember(memberName, _ID_MapPropertyInfo,        ref isDirty) ||
					//_FirstName.IsDirtyMember(memberName, _FirstName_MapPropertyInfo, ref isDirty) ||
					//_LastName. IsDirtyMember(memberName, _LastName_MapPropertyInfo,  ref isDirty) ||
					//_Birthday. IsDirtyMember(memberName, _Birthday_MapPropertyInfo,  ref isDirty);
			}


			private static MapPropertyInfo _Birthday_MapPropertyInfo;
			private static MapPropertyInfo _FirstName_MapPropertyInfo;
			private static MapPropertyInfo _LastName_MapPropertyInfo;
			private static MapPropertyInfo _ID_MapPropertyInfo;

			static PersonGen()
			{
				_ID_MapPropertyInfo        = new MapPropertyInfo(typeof(Person).GetProperty("ID"));
				_FirstName_MapPropertyInfo = new MapPropertyInfo(typeof(Person).GetProperty("FirstName"));
				_LastName_MapPropertyInfo  = new MapPropertyInfo(typeof(Person).GetProperty("LastName"));
				_Birthday_MapPropertyInfo  = new MapPropertyInfo(typeof(Person).GetProperty("Birthday"));
			}

			private static MapDescriptor _MapDescriptor;
			private static MapDescriptor  MapDescriptor
			{
				get
				{
					if (_MapDescriptor == null)
						_MapDescriptor = MapDescriptor.GetDescriptor(typeof(Person));

					return _MapDescriptor;
				}
			}
		}
	}
}
