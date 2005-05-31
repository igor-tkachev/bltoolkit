// MapDataTest.Test.h

#pragma once

using namespace System;
using namespace System::Data;

using namespace NUnit::Framework;

using namespace Rsdn::Framework::Data;
using namespace Rsdn::Framework::Data::Mapping;

namespace Cpp
{
	public __value enum State
	{
		[MapValue(S"A")] Active,
		[MapValue(S"I")] Inactive,
		[MapValue(S"P")] Pending
	};

	public __value enum StateNullable
	{
		[MapDefaultValue(0)] Unknown,
		[MapNullValue(0)]    Null,
		[MapValue(S"A")]     Active,
		[MapValue(S"I")]     Inactive,
		[MapValue(S"P")]     Pending
	};

	public __abstract __gc class AbstractClass
	{
	public:
		__property virtual String *get_Name() = 0;
	};

	public __gc class TestClass
	{
	public:
		__property String *get_Name() { return S"123"; }
	};

	[TestFixture]
	public __gc class MapDataTest
	{
	private:

		void CheckState(Object *value, StateNullable state)
		{
			String *message;
			
			if (value != 0)
			{
				message = String::Format(
					"'{0}' value of '{1}' type does not map to '{2}'", 
					value->ToString(),
					value->GetType(),
					__box(state));
			}
			else
			{
				message = String::Format("'null' does not map to '{0}'", __box(state));
			}

			Object       *obj      = Map::ToValue(value, __typeof(StateNullable));
			StateNullable mapValue = obj != 0? *dynamic_cast<__box StateNullable*>(obj): (StateNullable)-1;

			Assert::IsTrue(mapValue == state, message);
		}

	public:

		[Test]
		void ToValue()
		{
			CheckState(S"A", StateNullable::Active);
			CheckState(S"I", StateNullable::Inactive);
			CheckState(S"P", StateNullable::Pending);
			CheckState(S"X",     StateNullable::Unknown);
			CheckState(__box(0), StateNullable::Unknown);
			CheckState(0,             StateNullable::Null);
			CheckState(DBNull::Value, StateNullable::Null);
		}

		[Test]
		[ExpectedException(__typeof(RsdnMapException))]
		void ToValue_Exception1()
		{
			Map::ToValue(DBNull::Value, __typeof(State));
		}

		[Test]
		[ExpectedException(__typeof(RsdnMapException))]
		void ToValue_Exception2()
		{
			Map::ToValue(S"X", __typeof(State));
		}
	
		[Test]
		[ExpectedException(__typeof(RsdnMapException))]
		void ToValue_Exception3()
		{
			Map::ToValue(0, __typeof(State));
		}

		[Test]
		void AbstractProperty()
		{
			AbstractClass *ac = static_cast<AbstractClass*>(Map::ToObject(
				new TestClass(), __typeof(AbstractClass)));

			String *s = ac->Name;

			Console::WriteLine(s);
			Assert::AreEqual(s, S"123");
		}
	};
}