using System.Configuration;

namespace BLToolkit.Configuration
{
	/// <summary>
	/// Implementation of custom configuration section.
	/// </summary>
	internal class BLToolkitSection: ConfigurationSection
	{
		private const string SectionName = "bltoolkit";
		private static readonly ConfigurationPropertyCollection _properties =
			new ConfigurationPropertyCollection();

		private static readonly ConfigurationProperty           _propDataProviders =
			new ConfigurationProperty("dataProviders",           typeof(DataProviderElementCollection),
			new DataProviderElementCollection(),                 ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty           _propDefaultConfiguration =
			new ConfigurationProperty("defaultConfiguration",    typeof(string),
			null,                                                ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty           _propLoadExtensionAssemblies =
			new ConfigurationProperty("loadExtensionAssemblies", typeof(bool),
			null,                                                ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty           _propTypeFactory =
			new ConfigurationProperty("typeFactory",             typeof(TypeFactoryElement),
			null,                                                ConfigurationPropertyOptions.None);

		static BLToolkitSection()
		{
			_properties.Add(_propDataProviders);
			_properties.Add(_propDefaultConfiguration);
			_properties.Add(_propLoadExtensionAssemblies);
			_properties.Add(_propTypeFactory);
		}

		public static BLToolkitSection Instance
		{
			get { return (BLToolkitSection)ConfigurationManager.GetSection(SectionName); }
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get { return _properties; }
		}

		public DataProviderElementCollection DataProviders
		{
			get { return (DataProviderElementCollection) base[_propDataProviders]; }
		}

		public string DefaultConfiguration
		{
			get { return (string)base[_propDefaultConfiguration]; }
		}

		public bool LoadExtensionAssemblies
		{
			get { return (bool)base[_propLoadExtensionAssemblies]; }
		}

		public TypeFactoryElement TypeFactory
		{
			get { return (TypeFactoryElement)base[_propTypeFactory]; }
		}
	}
}
