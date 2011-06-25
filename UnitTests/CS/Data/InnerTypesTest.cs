using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.Data;
using BLToolkit.Mapping;

#pragma warning disable 0252

namespace Data
{
	[TestFixture]
	public class InnerTypesTest
	{
		public class TypeMapper: MemberMapper
		{
			public override object GetValue(object o)
			{
				var value = (Type)MemberAccessor.GetValue(o);
				return (null != value && MapMemberInfo.NullValue != value)? value.FullName: null;
			}

			public override void SetValue(object o, object value)
			{
				MemberAccessor.SetValue(o, (null != value)?
					Type.GetType((string)value, true, true): MapMemberInfo.NullValue);
			}
		}

		public class First
		{
			public string Name;
		}

		public abstract class Last
		{
			public          string Name;
			public abstract string Suffix { get; set; }

			// Inner type of the inner type
			//
			[MapField("FirstName", "Name")]
			public abstract First First { get; set; }

			// This reference type field will be ignored
			//
			public          Type   Type;
		}

		[MapField("FirstName",     "First.Name")]
		[MapField("LastName",      "Last.Name")]
		[MapField("LastSuffix",    "Last.Suffix")]
		[MapField("LastFirstName", "Last.First.Name")]
		public abstract class Person
		{
			[MapField("PersonID")]
			public          int    ID;
			public          First  First = new First();
			public abstract Last   Last { get; set; }
			public          string Name;
			[MemberMapper(typeof(TypeMapper))]
			public          Type   Type;
		}

		public abstract class Person2
		{
			[MapField("PersonID")]
			public          int    ID;

			[MapField("FirstName",  "Name")]
			public          First  First = new First();

			[MapField(Format="Last{0}")]
			public abstract Last   Last { get; set; }

			public          string Name;
			public          string Type;
		}

		[Test]
		public void MapFieldTest()
		{
			var p = (Person)TypeAccessor.CreateInstance(typeof(Person));

			p.ID              = 12345;
			p.First.Name      = "Crazy";
			p.Last.Name       = "Frog";
			p.Last.Suffix     = "Jr";
			p.Last.Type       = typeof(DbManager);
			p.Last.First.Name = "Crazy Frog";
			p.Name            = "Froggy";
			p.Type            = typeof(DbManager);

			var p2 = (Person2)Map.ObjectToObject(p, typeof(Person2));

			Assert.AreEqual(p.ID,              p2.ID);
			Assert.AreEqual(p.First.Name,      p2.First.Name);
			Assert.AreEqual(p.Last.Name,       p2.Last.Name);
			Assert.AreEqual(p.Last.Suffix,     p2.Last.Suffix);
			Assert.AreEqual(p.Last.First.Name, p2.Last.First.Name);
			Assert.AreEqual(p.Name,            p2.Name);
			Assert.AreEqual(p.Type.FullName,   p2.Type);

			// The 'Last.Type' field should be ignored by mapping process.
			//
			Assert.IsNull(p2.Last.Type);
		}

		[Test]
		public void CreateParametersTest()
		{
			IDbDataParameter[] parameters;
			var p = (Person)TypeAccessor.CreateInstance(typeof(Person));
			p.ID         = 12345;
			p.First.Name = "Crazy";
			p.Last.Name  = "Frog";
			p.Name       = "Froggy";
			p.Type       = typeof(DbManager);

			using (var db = new DbManager())
			{
				parameters = db.CreateParameters(p);
			}

			Assert.IsNotNull(parameters);
			Assert.AreEqual(7, parameters.Length);

			foreach (var parameter in parameters)
				Console.WriteLine("{0}: {1}", parameter.ParameterName, parameter.Value);
		}

		public abstract class Template2
		{
			public abstract int    ID          { get; set; }
			public abstract string DisplayName { get; set; }
		}

		[MapField("TPL_ID",          "tpl.ID")]
		[MapField("TPL_DisplayName", "tpl.DisplayName")]
		public abstract class Template1
		{
			public abstract int       ID          { get; set; }
			public abstract string    DisplayName { get; set; }

			public abstract Template2 tpl         { get; set; }
		}

		[Test]
		public void TemplateTest()
		{
			using (var db = new DbManager())
			{
				var cmd = @"
					SELECT
						1   as ID,
						'2' as DisplayName,
						3   as TPL_ID, 
						'4' as TPL_DisplayName";
#if ORACLE || FIREBIRD
				cmd += " FROM dual";
#endif
				var list = db
					.SetCommand(cmd)
					.ExecuteList(typeof(Template1));

				Assert.IsNotNull(list);
			}
		}
	}
}
