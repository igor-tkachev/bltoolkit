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
	}
}
