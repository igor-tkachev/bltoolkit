using System;
using BLToolkit.TypeBuilder;
using System.Web.Services;

namespace Demo.WebServices.Server.WebServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class GenerateWebServiceAttribute : GenerateAttributeAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebServiceAttribute"/> class.
		/// </summary>
		/// <param name="namespace">The namespace of the web service.</param>
		public GenerateWebServiceAttribute(string @namespace): base(typeof(WebServiceAttribute))
		{
			this["Namespace"] = @namespace;
		}
	}
}