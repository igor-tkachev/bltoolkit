using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping.MetadataProvider
{
	public delegate MapMetadataProvider CreateProvider();
	public delegate MemberMapper        EnsureMapperHandler(string mapName, string origName);


	public abstract class MapMetadataProvider
	{
		public virtual void AddProvider(MapMetadataProvider provider)
		{
		}

		public virtual void InsertProvider(int index, MapMetadataProvider provider)
		{
		}

		public virtual string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			isSet = false;
			return member.Name;
		}

		public virtual void EnsureMapper(ObjectMapper mapper, EnsureMapperHandler handler)
		{
		}

		public virtual bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			isSet = false;
			return TypeHelper.IsScalar(member.Type) == false;
		}

		public virtual bool GetTrimmable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			isSet = member.Type != typeof(string);
			return isSet? false: TrimmableAttribute.Default.IsTrimmable;
		}

		public virtual MapValue[] GetMapValues(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			isSet = false;
			return null;
		}

		#region Static Members

		private static CreateProvider _createProvider = new CreateProvider(CreateInternal);
		public  static CreateProvider  CreateProvider
		{
			get { return _createProvider; }
			set { _createProvider = value != null? value: new CreateProvider(CreateInternal); }
		}

		private static MapMetadataProvider CreateInternal()
		{
			return new MapMetadataProviderList();
		}

		#endregion
	}
}
