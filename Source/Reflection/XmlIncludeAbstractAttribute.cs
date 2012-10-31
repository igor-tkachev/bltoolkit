using System;
using System.Xml.Serialization;

namespace BLToolkit.Reflection
{
	/// <summary>
	/// Allows the <see cref="XmlSerializer"/> to recognize a type generated
	/// by the BLToolkit when it serializes or deserializes an object.
	/// </summary>
	public class XmlIncludeAbstractAttribute : XmlIncludeAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlIncludeAbstractAttribute"/> class.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of an abstract class to
		/// include its BLToolkit extensions.</param>
		public XmlIncludeAbstractAttribute(Type type)
			: base(TypeBuilder.TypeFactory.GetType(type))
		{
		}
	}
}