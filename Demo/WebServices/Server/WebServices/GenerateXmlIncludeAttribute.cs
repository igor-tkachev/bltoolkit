using System;
using System.Xml.Serialization;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Demo.WebServices.Server.WebServices
{
	/// <summary>
	/// Allows the <see cref="XmlSerializer"/> to recognize a type generated
	/// by the BLToolkit when it serializes or deserializes an object.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
	public class GenerateXmlIncludeAttribute : GenerateAttributeAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateXmlIncludeAttribute"/> class.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of an abstract class to
		/// include its BLToolkit extensions.</param>
		public GenerateXmlIncludeAttribute(Type type)
			: base(typeof(XmlIncludeAbstractAttribute), type)
		{
		}
	}
}