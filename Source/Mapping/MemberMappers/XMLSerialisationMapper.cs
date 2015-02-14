using System;
using System.Collections;
using System.IO;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;
using BLToolkit.Reflection;
using System.Text;
using BLToolkit.TypeBuilder;

namespace BLToolkit.Mapping.MemberMappers
{
	public class XMLSerialisationMapper : MemberMapper
	{
		public bool UseDefaultValueForNull { get; set; }

		public XMLSerialisationMapper()
			: this(false)
		{ }

		public XMLSerialisationMapper(bool useDefaultValueForNull)
		{
			UseDefaultValueForNull = useDefaultValueForNull;
		}

		public override void SetValue(object o, object value)
		{
			MemberAccessor.SetValue(o, Deserialize(value));
		}

		public override object GetValue(object o)
		{
			return XmlSerialize(MemberAccessor.GetValue(o));
		}

		string XmlSerialize(object obj)
		{
			if (obj == null) 
				return MappingSchema.DefaultStringNullValue;

			var serializer = GetSerializer(obj);

			var settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.NewLineHandling = NewLineHandling.Entitize;


			using (var s = new MemoryStream())
			using (var wr = XmlWriter.Create(s, settings))
			{
				serializer.Serialize(wr, obj);
				return Encoding.UTF8.GetString(s.GetBuffer()).Trim('\0');
			}
		}

		private XmlSerializer GetSerializer(object obj)
		{
			var type       = TypeAccessor.GetAccessor(MemberAccessor.Type).Type;
			var extraType  = obj != null ? TypeHelper.GetListItemType(obj) : typeof(object);
			var extraType2 = TypeHelper.GetListItemType(type);
			var extraTypes = new[]
				{/*extraType,*/ TypeAccessor.GetAccessor(extraType).Type, /*extraType2,*/ TypeAccessor.GetAccessor(extraType2).Type};

			var serializer = extraType != typeof (object) || extraType2 != typeof(object)
				? new XmlSerializer(type, extraTypes)
				: new XmlSerializer(type);
			return serializer;
		}

		object Deserialize(object value)
		{
			var txt = value == null ? string.Empty : value.ToString();
			object retVal = null;

			try
			{
				var originalType = MemberAccessor.Type;

				if (!string.IsNullOrEmpty(txt))
				{
					var ser   = GetSerializer(null);
					var bytes = Encoding.UTF8.GetBytes(txt);

					using (var s = new MemoryStream(bytes))
					{
						s.Position = 0;
						retVal = ser.Deserialize(s);
					}
				}
				else
				{
					var na = MemberAccessor.GetAttribute<NoInstanceAttribute>();
					if (na == null && UseDefaultValueForNull)
					{
						retVal = originalType == typeof(string)
								? string.Empty
								: TypeAccessor.CreateInstanceEx(originalType);
					}

				}
			}
			catch (Exception)
			{
			}
			return retVal;
		}

	}
}
