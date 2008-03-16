using System;
using System.Data.Linq.Mapping;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapLinqMetadataProvider : MapMetadataProvider
	{
		#region Helpers

		private ObjectMapper _mapper;
		private bool?        _isLinqObject;

		private void EnsureMapper(ObjectMapper mapper)
		{
			if (_mapper != mapper)
			{
				_mapper       = mapper;
				_isLinqObject = null;
			}
		}

		private bool IsLinqObject(ObjectMapper mapper)
		{
			EnsureMapper(mapper);

			if (_isLinqObject == null)
			{
				object[] attrs = mapper.TypeAccessor.Type.GetCustomAttributes(typeof(TableAttribute), true);
				_isLinqObject = attrs.Length > 0;
			}

			return _isLinqObject.Value;
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (IsLinqObject(mapper))
			{
				ColumnAttribute a = member.GetAttribute<ColumnAttribute>();

				if (a != null && !string.IsNullOrEmpty(a.Name))
				{
					isSet = true;
					return a.Name;
				}
			}

			return base.GetFieldName(mapper, member, out isSet);
		}

		#endregion

		#region GetIgnore

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (IsLinqObject(mapper))
			{
				isSet = true;
				return member.GetAttribute<ColumnAttribute>() == null;
			}

			return base.GetIgnore(mapper, member, out isSet);
		}

		#endregion

		#region GetNullable

		public override bool GetNullable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (IsLinqObject(mapper))
			{
				var attr = member.GetAttribute<ColumnAttribute>();

				if (attr != null)
				{
					isSet = true;
					return attr.CanBeNull;
				}
			}

			return base.GetNullable(mapper, member, out isSet);
		}

		#endregion
	}
}
