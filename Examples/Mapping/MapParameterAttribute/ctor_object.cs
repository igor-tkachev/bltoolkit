/// example:
/// mapparam ctor(object)
/// comment:
/// The following example demonstrates how to use the <b>MapParameterAttribute</b> attribute.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapParameterAttribute
{
	[TestFixture]
	public class ctor_object
	{
		public interface IValidatable
		{
			void Validate();
		}

		// This class can be used for internal implementation of string properties.
		//
		public class EditableString : IValidatable
		{
			public EditableString()
			{
			}

			public EditableString(string fieldName, int maxLength)
			{
				_fieldName = fieldName;
				_maxLength = maxLength;
			}

			public EditableString(object[] parameters)
			{
				foreach (object o in parameters)
				{
					if (o is string)
						_fieldName = (string)o;
					else if (o is int)
						_maxLength = (int)o;
				}
			}

			private string _fieldName;
			private int    _maxLength;

			private string _value;
			public  string  Value
			{
				get { return _value;  }
				set { _value = value; }
			}

			#region IValidatable Members

			void IValidatable.Validate()
			{
				if (_maxLength > 0)
				{
					if (_value == null || _value.Length == 0)
						throw new ApplicationException(
							string.Format("Field '{0}' must not be empty.", _fieldName));

					if (_value.Length > _maxLength)
						throw new ApplicationException(
							string.Format("Field '{0}' length must be less than {1}.",
							_fieldName, 
							_maxLength));
				}
			}

			#endregion
		}

		// Base entity class.
		// Implements the IValidatable interface and defines internal implementation types
		// of abstract properties.
		//
		[MapType(typeof(string), typeof(EditableString))]
		public abstract class BizEntityBase : IValidatable
		{
			#region IValidatable Members

			public void Validate()
			{
				// This call returns all internal objects created by the mapper
				// in order to implement concrete class.
				//
				object[] generatedMembers = ((IMapGenerated)this).GetCreatedMembers();

				foreach (object o in generatedMembers)
					if (o is IValidatable)
						((IValidatable)o).Validate();
			}

			#endregion
		}

		// Business entity.
		//
		public abstract class MyBizEntity : BizEntityBase
		{
			// This attribute enforces the mapper to call
			// the EditableString("Name", 20) constructor
			// during the concrete class generation.
			//
			[MapParameter("Name", 20)]
			public abstract string Name { get; set; }

			// This attribute enforces the mapper to call
			// the EditableString(object[] parameters) constructor.
			//
			[MapParameter("Description")]
			public abstract string Description { get; set; }

			// Creates a concrete class.
			//
			public static MyBizEntity CreateInstance()
			{
				return (MyBizEntity)Map.Descriptor(typeof(MyBizEntity)).CreateInstance();
			}
		}

		[Test]
		public void Test1()
		{
			MyBizEntity entity = MyBizEntity.CreateInstance();

			entity.Name        = "Test";
			entity.Description = "Test 6789 123456789 12345";

			entity.Validate();
		}

		[Test]
		[ExpectedException(
			 typeof(ApplicationException),
			 "Field 'Name' must not be empty.")]
		public void Test2()
		{
			MyBizEntity entity = MyBizEntity.CreateInstance();

			entity.Name = "";

			entity.Validate();
		}

		[Test]
		[ExpectedException(
			 typeof(ApplicationException),
			 "Field 'Name' length must be less than 20.")]
		public void Test3()
		{
			MyBizEntity entity = MyBizEntity.CreateInstance();

			entity.Name = "Test 6789 123456789 12345";

			entity.Validate();
		}
	}
}
