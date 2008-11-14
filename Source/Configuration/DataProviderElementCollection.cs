using System.Configuration;

namespace BLToolkit.Configuration
{
	[ConfigurationCollection(typeof(DataProviderElement))]
	internal class DataProviderElementCollection : ElementCollectionBase<DataProviderElement>
	{
		protected override object GetElementKey(DataProviderElement element)
		{
			return element.Name;
		}
	}
}