using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Mapping.Fluent
{
	public partial class MapFieldMap<T, TR>
	{
		public class AssociationMap<TRt>
		{
			private readonly MapFieldMap<T, TR> _owner;
			private readonly bool _canBeNull;
			private readonly List<Expression<Func<T, TRt>>> _thisKeys;

			public AssociationMap(MapFieldMap<T, TR> owner, bool canBeNull, List<Expression<Func<T, TRt>>> thisKeys)
			{
				this._owner = owner;
				this._canBeNull = canBeNull;
				this._thisKeys = thisKeys;
			}

			public MapFieldMap<T, TR> ToMany<TRf, TRo>(Expression<Func<TRf, TRo>> otherKey, params Expression<Func<TRf, TRo>>[] otherKeys)
			{
                var keys = new List<Expression<Func<TRf, TRo>>>(otherKeys);
				keys.Insert(0, otherKey);
				return this._owner.Association(this._canBeNull, this._thisKeys, keys);
			}

			public MapFieldMap<T, TR> ToOne<TRo>(Expression<Func<TR, TRo>> otherKey, params Expression<Func<TR, TRo>>[] otherKeys)
			{
				var keys = new List<Expression<Func<TR, TRo>>>(otherKeys);
				keys.Insert(0, otherKey);
				return this._owner.Association(this._canBeNull, this._thisKeys, keys);
			}
		}
	}
}