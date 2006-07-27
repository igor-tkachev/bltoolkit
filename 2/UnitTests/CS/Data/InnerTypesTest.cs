using System;
using System.Data;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.Data;
using BLToolkit.Mapping;

namespace A.Data
{
	[TestFixture]
	public class InnerTypesTest
	{
		public class TypeMapper: MemberMapper
		{
			public override object GetValue(object o)
			{
				Type value = (Type)MemberAccessor.GetValue(o);
				return (null != value && MapMemberInfo.NullValue != value)? value.FullName: null;
			}

			public override void SetValue(object o, object value)
			{
				MemberAccessor.SetValue(o, (null != value)?
					Type.GetType((string)value, true, true): MapMemberInfo.NullValue);
			}
		}

		public struct First
		{
			public string Name;
		}

		public class Middle
		{
			public string Name;
		}

		public abstract class Last
		{
			public abstract string Name { get; set; }
		}

		[MapField("FirstName",  "First.Name")]
		[MapField("LastName",   "Last.Name")]
		[MapField("MiddleName", "Middle.Name")]
		public abstract class Person
		{
			[MapField("PersonID")]
			public          int    ID;
			[MapIgnore]
			public          First  First;
			[MapIgnore(false)]
			public          Middle Middle = new Middle();
			[MapIgnore(false)]
			public abstract Last   Last { get; set; }
			public          string Name;
			[MemberMapper(typeof(TypeMapper))]
			public          Type   Type;
		}

		//[Test]
		public void CreateParametersTest()
		{
			using (DbManager db = new DbManager())
			{
				Person p = (Person)TypeAccessor.CreateInstance(typeof (Person));

				p.ID = 12345;
				p.First.Name = "Crazy";
				p.Middle.Name = "jr";
				p.Last.Name = "Grog";
				p.Name = "Groggy";
				p.Type = typeof(DbManager);

				IDbDataParameter[] parameters = db.CreateParameters(p);
				Assert.IsNotNull(parameters);
				Assert.AreEqual(parameters.Length, 6);
			}
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
			using (DbManager db = new DbManager())
			{
				ArrayList list = db
					.SetCommand(@"
						SELECT
							1   as ID,
							'2' as DisplayName,
							3   as TPL_ID, 
							'4' as TPL_DisplayName")
					.ExecuteList(typeof(Template1));
			}
		}
	}
}
