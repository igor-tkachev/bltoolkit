using System;
using System.Configuration;

namespace BLToolkit.Configuration
{
	[ConfigurationCollection(typeof(DataProviderElement))]
	internal class DataProviderElementCollection : ElementCollectionBase<DataProviderElement>
	{
		protected override object GetElementKey(DataProviderElement element)
		{
			// element.Name is optional and may be omitted.
			// element.TypeName is required, but is not unique.
			//
			return string.Concat(element.Name, "/", element.TypeName);
		}
	}
}