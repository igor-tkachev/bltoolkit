/*
 * File:    MapResultSet.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapResultSet
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectType"></param>
		public MapResultSet(Type objectType)
		{
			_objectType = objectType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="parameters"></param>
		public MapResultSet(Type objectType, object[] parameters)
		{
			_objectType = objectType;
			_parameters = parameters;
		}

		private  Type _objectType;
		internal Type  ObjectType
		{
			get { return _objectType; }
		}

		private  object[] _parameters;
		internal object[]  Parameters
		{
			get { return _parameters; }
		}

		private  MapRelation[] _relations;
		internal MapRelation[]  Relations
		{
			get { return _relations;  }
			set { _relations = value; }
		}
	}
}
