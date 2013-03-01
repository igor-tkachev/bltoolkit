using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.Fluent
{
	/// <summary>
	/// Configure BLToolkit in fluent style
	/// </summary>
	public static class FluentConfig
	{
		private static Dictionary<Assembly, ExtensionList> _hash = new Dictionary<Assembly, ExtensionList>();

		/// <summary>
		/// Configure DbManager
		/// </summary>
		/// <param name="dbManager"></param>
		public static MappingConfigurator Configure(DbManager dbManager)
		{
			MappingSchema mappingSchema = dbManager.MappingSchema ?? (dbManager.MappingSchema = Map.DefaultSchema);
			return Configure(mappingSchema);
		}

		/// <summary>
		/// Configure DataProvider
		/// </summary>
		/// <param name="dataProvider"></param>
		public static MappingConfigurator Configure(DataProviderBase dataProvider)
		{
			MappingSchema mappingSchema = dataProvider.MappingSchema ?? (dataProvider.MappingSchema = Map.DefaultSchema);
			return Configure(mappingSchema);
		}

		/// <summary>
		/// Configure MappingSchema
		/// </summary>
		/// <param name="mappingSchema"></param>
		public static MappingConfigurator Configure(MappingSchema mappingSchema)
		{
			ExtensionList extensionList = mappingSchema.Extensions ?? (mappingSchema.Extensions = new ExtensionList());
			return Configure(extensionList);
		}

		/// <summary>
		/// Configure ExtensionList
		/// </summary>
		/// <param name="extensionList"></param>
		public static MappingConfigurator Configure(ExtensionList extensionList)
		{
			return new MappingConfigurator(extensionList);
		}

		public class MappingConfigurator
		{
			private ExtensionList _extensions;

			public MappingConfigurator(ExtensionList extensions)
			{
				this._extensions = extensions;
			}

            /// <summary>
            /// Mapping IFluentMap-Type
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public void MapingFromType<T>() where T : IFluentMap
            {
                var res = new ExtensionList();
                var map = (IFluentMap)Activator.CreateInstance(typeof(T));
                map.MapTo(res);
                FluentMapHelper.MergeExtensions(res, ref this._extensions);
            }

			/// <summary>
			/// Mapping from assembly contains type
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <returns></returns>
			public void MapingFromAssemblyOf<T>()
			{
				this.MapingFromAssembly(typeof(T).Assembly);
			}
		    
		    /// <summary>
			/// Mapping from assembly
			/// </summary>
			/// <param name="assembly"></param>
			/// <returns></returns>
			public void MapingFromAssembly(Assembly assembly)
			{
				ExtensionList res;
				if (!_hash.TryGetValue(assembly, out res))
				{
					res = new ExtensionList();
					_hash.Add(assembly, res);

					string fluentType = typeof(IFluentMap).FullName;
					var maps = from type in assembly.GetTypes()
							   where type.IsClass && !type.IsAbstract && !type.IsGenericType
									 && (null != type.GetInterface(fluentType)) // Is IFluentMap
									 && (null != type.GetConstructor(new Type[0])) // Is defaut ctor
							   select (IFluentMap)Activator.CreateInstance(type);
					foreach (var fluentMap in maps)
					{
						fluentMap.MapTo(res);
					}
				}
				FluentMapHelper.MergeExtensions(res, ref this._extensions);
			}
		}
	}
}