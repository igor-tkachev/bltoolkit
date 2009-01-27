using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;
using BLToolkit.Reflection.MetadataProvider;

namespace Mapping.MetadataProvider
{
	[TestFixture]
	public class MetadataProviderTest
	{
		class CustomMetadataProvider : MetadataProviderBase
		{
			public override string GetFieldName(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
			{
				string name = string.Empty;

				foreach (char c in member.Name)
				{
					if (char.IsUpper(c))
					{
						if (name.Length > 0)
							name += '_';
						name += c;
					}
					else
					{
						name += char.ToUpper(c);
					}
				}

				isSet = true;

				return name;
			}
		}

		public class Person
		{
			public string FirstName;
			public string LastName;
		}

		static void MapMetadataProvider_OnCreateProvider(MetadataProviderBase parentProvider)
		{
			parentProvider.AddProvider(new CustomMetadataProvider());
		}

		[Test]
		public void Test()
		{
			MetadataProviderBase.OnCreateProvider += MapMetadataProvider_OnCreateProvider;

			string cmd = "SELECT '1' as FIRST_NAME, '2' as LAST_NAME";
#if ORACLE || FIREBIRD
			cmd += " FROM dual";
#endif
			using (DbManager db = new DbManager())
			{
				Person p = (Person)db
					.SetCommand(cmd)
					.ExecuteObject(typeof(Person));

				Assert.AreEqual("1", p.FirstName);
				Assert.AreEqual("2", p.LastName);
			}
		}

		[TearDown]
		public void TearDown()
		{
			MetadataProviderBase.OnCreateProvider -= MapMetadataProvider_OnCreateProvider;
		}
	}
}
