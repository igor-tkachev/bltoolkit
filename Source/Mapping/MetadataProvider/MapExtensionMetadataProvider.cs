using System;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapExtensionMetadataProvider : MapMetadataProvider
	{
		private object GetValue(ObjectMapper mapper, MemberAccessor member, string elemName, out bool isSet)
		{
			object value = mapper.Extension[member.Name][elemName].Value;

			isSet = value != null;

			return value;
		}

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapField", out isSet);

			if (value != null)
				return value.ToString();

			return base.GetFieldName(mapper, member, out isSet);
		}

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapIgnore", out isSet);

			if (value != null)
				return TypeExtension.ToBoolean(value);

			return base.GetIgnore(mapper, member, out isSet);
		}

		public override bool GetTrimmable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				object value = GetValue(mapper, member, "Trimmable", out isSet);

				if (value != null)
					return TypeExtension.ToBoolean(value);
			}

			return base.GetTrimmable(mapper, member, out isSet);
		}
	}
}
