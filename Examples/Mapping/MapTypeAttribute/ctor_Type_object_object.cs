/// example:
/// maptype ctor(Type,object,object)
/// comment:
/// The following example demonstrates how to use the <b>MapTypeAttribute</b> attribute.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapTypeAttribute
{
	[TestFixture]
	public class ctor_Type_object_object
	{
		public interface IValidatable
		{
			void Validate();
		}

		// This class can be used for internal implementation 
		// of string properties that need to be validated.
		//
		public class RequiredString : IValidatable
		{
			public RequiredString(string fieldName, int maxLength)
			{
				_fieldName = fieldName;
				_maxLength = maxLength;
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
				if (_value == null || _value.Length == 0)
					throw new ApplicationException(
						string.Format("Field '{0}' must not be empty.", _fieldName));

				if (_value.Length > _maxLength)
					throw new ApplicationException(
						string.Format("Field '{0}' length must be less than {1}.",
						_fieldName, 
						_maxLength));
			}

			#endregion
		}

		// Base entity class.
		// Implements the IValidatable interface.
		//
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
			// the RequiredString("Description", 20) constructor
			// during the concrete class generation.
			//
			[MapType(typeof(RequiredString), "Description", 20)]
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

			entity.Description = "Test";

			entity.Validate();
		}

		[Test]
		[ExpectedException(
			 typeof(ApplicationException),
			 "Field 'Description' must not be empty.")]
		public void Test2()
		{
			MyBizEntity entity = MyBizEntity.CreateInstance();

			entity.Description = "";

			entity.Validate();
		}

		[Test]
		[ExpectedException(
			 typeof(ApplicationException),
			 "Field 'Description' length must be less than 20.")]
		public void Test3()
		{
			MyBizEntity entity = MyBizEntity.CreateInstance();

			entity.Description = "Test 6789 123456789 12345";

			entity.Validate();
		}
	}
}
