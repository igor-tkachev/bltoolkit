using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.Fluent
{
	/// <summary>
	/// Fluent settings for field
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TR"></typeparam>
	public partial class MapFieldMap<T, TR> : FluentMap<T>
	{
		private readonly Expression<Func<T, TR>> _prop;

		public MapFieldMap(TypeExtension owner, List<IFluentMap> childs, Expression<Func<T, TR>> prop)
			: base(owner, childs)
		{
			this._prop = prop;
		}

		/// <summary>
		/// PrimaryKeyAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="order"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> PrimaryKey(int order = -1)
		{
			return this.PrimaryKey(this._prop, order);
		}

        public MapFieldMap<T, TR> LazyInstance(bool isLazy = true)
        {
            return this.LazyInstance(this._prop, isLazy);
        }


		/// <summary>
		/// NonUpdatableAttribute
		/// </summary>
		/// <returns></returns>
		public MapFieldMap<T, TR> NonUpdatable()
		{
			return this.NonUpdatable(this._prop);
		}

		/// <summary>
		/// IdentityAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <returns></returns>
		public MapFieldMap<T, TR> Identity()
		{
			return this.Identity(this._prop);
		}

		/// <summary>
		/// SqlIgnoreAttribute
		/// </summary>
		/// <param name="ignore"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> SqlIgnore(bool ignore = true)
		{
			return this.SqlIgnore(this._prop, ignore);
		}

		/// <summary>
		/// MapIgnoreAttribute
		/// </summary>
		/// <param name="ignore"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MapIgnore(bool ignore = true)
		{
			return this.MapIgnore(this._prop, ignore);
		}

		/// <summary>
		/// TrimmableAttribute
		/// </summary>
		/// <returns></returns>
		public MapFieldMap<T, TR> Trimmable()
		{
			return this.Trimmable(this._prop);
		}

		/// <summary>
		/// MapValueAttribute
		/// </summary>
		/// <typeparam name="TV"> </typeparam>
		/// <param name="origValue"></param>
		/// <param name="value"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MapValue<TV>(TR origValue, TV value, params TV[] values)
		{
			return this.MapValue(this._prop, origValue, value, values);
		}

		/// <summary>
		/// DefaultValueAttribute
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> DefaultValue(TR value)
		{
			return this.DefaultValue(this._prop, value);
		}

        /// <summary>
        /// DbTypeAttribute
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public MapFieldMap<T, TR> DbType(DbType dbType)
        {
            return this.DbType(this._prop, dbType);
        }

        /// <summary>
        /// MemberMapperAttribute
        /// at the Moment you also have to specify MapIgnore(false) when using Complex types with Member Mapper.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public MapFieldMap<T, TR> MemberMapper(Type memberMapperType)
        {
            return this.MemberMapper(this._prop, memberMapperType);
        }

        /// <summary>
        /// MemberMapperAttribute
        /// at the Moment you also have to specify MapIgnore(false) when using Complex types with Member Mapper.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public MapFieldMap<T, TR> MemberMapper(Type memberType, Type memberMapperType)
        {
            return this.MemberMapper(this._prop, memberType, memberMapperType);
        }

		/// <summary>
		/// NullableAttribute
		/// </summary>
		/// <param name="isNullable"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Nullable(bool isNullable = true)
		{
			return this.Nullable(this._prop, isNullable);
		}

		/// <summary>
		/// NullValueAttribute
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> NullValue(TR value)
		{
			return this.NullValue(this._prop, value);
		}

		/// <summary>
		/// AssociationAttribute
		/// </summary>
		/// <typeparam name="TRt"></typeparam>
		/// <param name="canBeNull"></param>
		/// <param name="thisKey"></param>
		/// <param name="thisKeys"></param>
		/// <returns></returns>
		public AssociationMap<TRt> Association<TRt>(bool canBeNull, Expression<Func<T, TRt>> thisKey, params Expression<Func<T, TRt>>[] thisKeys)
		{
			return this.Association(this._prop, canBeNull, thisKey, thisKeys);
		}

		/// <summary>
		/// AssociationAttribute
		/// </summary>
		/// <typeparam name="TRt"></typeparam>
		/// <param name="thisKey"></param>
		/// <param name="thisKeys"></param>
		/// <returns></returns>
		public AssociationMap<TRt> Association<TRt>(Expression<Func<T, TRt>> thisKey, params Expression<Func<T, TRt>>[] thisKeys)
		{
			return this.Association(this._prop, thisKey, thisKeys);
		}

		private MapFieldMap<T, TR> Association<TRt, TRf, TRo>(bool canBeNull
			, IEnumerable<Expression<Func<T, TRt>>> thisKeys, IEnumerable<Expression<Func<TRf, TRo>>> otherKeys)
		{
			return this.Association(this._prop, canBeNull, thisKeys, otherKeys);
		}

		/// <summary>
		/// RelationAttribute
		/// </summary>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Relation(string slaveIndex = null, string masterIndex = null)
		{
			return this.Relation(this._prop, slaveIndex, masterIndex);
		}

		/// <summary>
		/// RelationAttribute
		/// </summary>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Relation(string[] slaveIndex, string[] masterIndex)
		{
			return this.Relation(this._prop, slaveIndex, masterIndex);
		}
	}
}