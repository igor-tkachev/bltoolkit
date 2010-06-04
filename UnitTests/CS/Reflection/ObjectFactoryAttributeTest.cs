using System;

using NUnit.Framework;

using BLToolkit.Reflection;
using System.Reflection;
using System.Collections.Generic;
using BLToolkit.Mapping;

namespace Reflection
{
	[TestFixture]
	public class ObjectFactoryAttributeTest
	{
		[ObjectFactory(typeof(TestObject.Factory))]
		public class TestObject
		{
			public TestObject()
			{
				throw new InvalidOperationException();
			}

			private TestObject(int n)
			{
				Number = n;
			}

			public int Number;

			public class Factory : IObjectFactory
			{
				object IObjectFactory.CreateInstance(TypeAccessor typeAccessor, InitContext context)
				{
					return new TestObject(53);
				}
			}
		}

		[Test]
		public void Test()
		{
			TestObject o = (TestObject)TypeAccessor.CreateInstanceEx(typeof(TestObject));

			Assert.AreEqual(53, o.Number);
		}

		[ObjectFactory(typeof(Record.Factory))]
		public class Record
		{
			public class Factory : IObjectFactory
			{
				#region IObjectFactory Members

				public object CreateInstance(TypeAccessor typeAccessor, InitContext context)
				{
					Type t = typeAccessor.Type;

					ConstructorInfo[] ctis = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					ConstructorInfo   constructor = ctis[0];

					foreach (ConstructorInfo ci in ctis)
					{
						if (constructor.GetParameters().Length < ci.GetParameters().Length)
							constructor = ci;
					}
					
					ParameterInfo[] pis   = constructor.GetParameters();
					object[]        param = new object[pis.Length];

					for(int i = 0; i < pis.Length; i++)
					{
						ParameterInfo pi      = pis[i];
						Type          pType   = pi.ParameterType;
						string        pName   = pi.Name;
						int           ordinal = context.DataSource.GetOrdinal(pName);

						if (ordinal >= 0)
						{
							param[i] = context.MappingSchema.ConvertChangeType(
								context.DataSource.GetValue(context.SourceObject, ordinal),
								pType);
						}
						else
							param[i] = context.MappingSchema.GetDefaultValue(pType);
					}

					context.StopMapping = true;

					return constructor.Invoke(param);

				}

				#endregion
			}

			public Record(string name, int value)
			{
				_name = name;
				_value = value;
			}

			private string _name;
			public  string  Name { get { return _name; } }

			private int _value;
			public  int  Value { get { return _value; } }
		}

		[Test]
		public void RecordFactoryTest()
		{
			Record s = new Record("Elvis", 101);
			Record r = Map.ObjectToObject<Record>(s);

			Assert.IsNotNull(r);

			Assert.AreEqual(s.Value, r.Value);
			Assert.AreEqual(s.Name,  r.Name);
		}
	}
}
