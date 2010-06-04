using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Demo.WebServices.ObjectModel
{
	[Serializable]
	public class XmlMap<TKey, TValue>: Dictionary<TKey, TValue>, IXmlSerializable
	{
		private const string KEY_NAME     = "k";
		private const string VALUE_NAME   = "v";

		private static readonly XmlSerializer<TKey>   _keySerializer   = new XmlSerializer<TKey>(KEY_NAME);
		private static readonly XmlSerializer<TValue> _valueSerializer = new XmlSerializer<TValue>(VALUE_NAME);

		#region IXmlSerializable members

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			if (!reader.ReadToDescendant(KEY_NAME))
			{
				reader.Skip();
				return;
			}

			do
			{
				Add(_keySerializer.Deserialize(reader),
				    _valueSerializer.Deserialize(reader));
			}
			while (reader.NodeType != XmlNodeType.EndElement);

			reader.ReadEndElement();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				_keySerializer  .Serialize(writer, pair.Key);
				_valueSerializer.Serialize(writer, pair.Value);
			}
		}

		#endregion

		private class XmlSerializer<T>
		{
			private readonly XmlSerializer _instance;

			public XmlSerializer(string elementName)
			{
				_instance = new XmlSerializer(
					TypeFactory.GetType(typeof(T)), new XmlRootAttribute(elementName));
			}

			public void Serialize(XmlWriter writer, T value)
			{
				_instance.Serialize(writer, value);
			}

			public T Deserialize(XmlReader reader)
			{
				return (T)_instance.Deserialize(reader);
			}
		}
	}
}