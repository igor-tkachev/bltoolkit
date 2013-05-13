using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.Fluent
{
	/// <summary>
	/// FluentSettings
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public partial class FluentMap<T>
	{
		private readonly TypeExtension _typeExtension;
		private List<IFluentMap> _childs;
		private const string MemberNameSeparator = ".";

		/// <summary>
		/// ctor
		/// </summary>
		public FluentMap()
			: this(new TypeExtension { Name = typeof(T).FullName }, null)
		{ }

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="typeExtension"></param>
		/// <param name="childs"></param>
		protected FluentMap(TypeExtension typeExtension, List<IFluentMap> childs)
		{
			this._typeExtension = typeExtension;
			this._childs = childs;

            if (FluentConfig.MappingConfigurator.GetTableName != null)
            {
                this.TableName(null, null, FluentConfig.MappingConfigurator.GetTableName(typeof(T)));
            }
		}

		/// <summary>
		/// TableNameAttribute
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public FluentMap<T> TableName(string name)
		{
			return this.TableName(null, null, name);
		}

		/// <summary>
		/// TableNameAttribute
		/// </summary>
		/// <param name="database"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public FluentMap<T> TableName(string database, string name)
		{
			return this.TableName(database, null, name);
		}

		/// <summary>
		/// TableNameAttribute
		/// </summary>
		/// <param name="database"></param>
		/// <param name="owner"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public FluentMap<T> TableName(string database, string owner, string name)
		{
			((IFluentMap)this).TableName(database, owner, name);
			return this;
		}

		/// <summary>
		/// MapFieldAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <param name="isInheritanceDiscriminator"></param>
		/// <returns></returns>
		public MapFieldMap<T,TR> MapField<TR>(Expression<Func<T, TR>> prop, bool isInheritanceDiscriminator)
		{
			return this.MapField(prop, null, null, isInheritanceDiscriminator);
		}

		/// <summary>
		/// MapFieldAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <param name="mapName"></param>
		/// <param name="storage"></param>
		/// <param name="isInheritanceDiscriminator"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MapField<TR>(Expression<Func<T, TR>> prop, string mapName = null, string storage = null, bool? isInheritanceDiscriminator = null)
		{
			string name = this.GetExprName(prop);

			if (mapName == null && FluentConfig.MappingConfigurator.GetColumnName != null)
			{
				mapName = FluentConfig.MappingConfigurator.GetColumnName(new MappedProperty { Name = name, Type = typeof(TR), ParentType = typeof(T) });
			}

			((IFluentMap)this).MapField(name, mapName, storage, isInheritanceDiscriminator);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		private void MapFieldOnType(string origName, string mapName)
		{
			AttributeExtensionCollection attrs;
			if (!this._typeExtension.Attributes.TryGetValue(Attributes.MapField.Name, out attrs))
			{
				attrs = new AttributeExtensionCollection();
				this._typeExtension.Attributes.Add(Attributes.MapField.Name, attrs);
			}
			var attributeExtension = new AttributeExtension();
			attributeExtension.Values.Add(Attributes.MapField.OrigName, origName);
            attributeExtension.Values.Add(Attributes.MapField.MapName, mapName);
			attrs.Add(attributeExtension);
		}

		private void MapFieldOnField(string origName, string mapName, string storage, bool? isInheritanceDiscriminator)
		{
			var member = this.GetMemberExtension(origName);
			if (!string.IsNullOrEmpty(mapName))
			{
				member.Attributes.Add(Attributes.MapField.Name, mapName);
			}
			if (null != storage)
			{
				member.Attributes.Add(Attributes.MapField.Storage, storage);
			}
			if (null != isInheritanceDiscriminator)
			{
				member.Attributes.Add(Attributes.MapField.IsInheritanceDiscriminator, this.ToString(isInheritanceDiscriminator.Value));
			}
		}

		/// <summary>
		/// PrimaryKeyAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> PrimaryKey<TR>(Expression<Func<T, TR>> prop, int order = -1)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).PrimaryKey(name, order);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// NonUpdatableAttribute
		/// </summary>
		/// <returns></returns>
		public MapFieldMap<T, TR> NonUpdatable<TR>(Expression<Func<T, TR>> prop)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).NonUpdatable(name);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// IdentityAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <returns></returns>
		public MapFieldMap<T, TR> Identity<TR>(Expression<Func<T, TR>> prop)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).Identity(name);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// SqlIgnoreAttribute
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="ignore"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> SqlIgnore<TR>(Expression<Func<T, TR>> prop, bool ignore = true)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).SqlIgnore(name, ignore);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// MapIgnoreAttribute
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="ignore"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MapIgnore<TR>(Expression<Func<T, TR>> prop, bool ignore = true)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).MapIgnore(name, ignore);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// TrimmableAttribute
		/// </summary>
		/// <returns></returns>
		public MapFieldMap<T, TR> Trimmable<TR>(Expression<Func<T, TR>> prop)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).Trimmable(name);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// MapValueAttribute
		/// </summary>
		/// <typeparam name="TV"> </typeparam>
		/// <typeparam name="TR"> </typeparam>
		/// <param name="prop"></param>
		/// <param name="origValue"></param>
		/// <param name="value"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MapValue<TV, TR>(Expression<Func<T, TR>> prop, TR origValue, TV value, params TV[] values)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).MapValue(name, origValue, value, values);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// DefaultValueAttribute
		/// </summary>
		/// <param name="prop"> </param>
		/// <param name="value"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> DefaultValue<TR>(Expression<Func<T, TR>> prop, TR value)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).DefaulValue(name, value);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// DbTypeAttribute
		/// </summary>
		/// <param name="prop"> </param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public MapFieldMap<T,TR> DbType<TR>(Expression<Func<T, TR>> prop, DbType dbType)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).DbType<TR>(name, dbType);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// MemberMapperAttribute
		/// </summary>
		/// <param name="prop"> </param>
		/// <param name="memberMapperType"></param>
		/// <returns></returns>
		public MapFieldMap<T,TR> MemberMapper<TR>(Expression<Func<T,TR>> prop, Type memberMapperType)
		{
			return this.MemberMapper(prop, null, memberMapperType);
		}

		/// <summary>
		/// MemberMapperAttribute
		/// </summary>
		/// <param name="prop"> </param>
		/// <param name="memberType"></param>
		/// <param name="memberMapperType"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> MemberMapper<TR>(Expression<Func<T, TR>> prop, Type memberType, Type memberMapperType)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).MemberMapper<TR>(name, memberType, memberMapperType);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// NullableAttribute
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="isNullable"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Nullable<TR>(Expression<Func<T, TR>> prop, bool isNullable = true)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).Nullable(name, isNullable);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// LazyInstanceAttribute
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="isLazy"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> LazyInstance<TR>(Expression<Func<T, TR>> prop, bool isLazy = true)
		{
			string name = this.GetExprName(prop);
			if (!GetIsVirtual(prop))
				throw new Exception("Property wich uses LazyInstance needs to be virtual!");
			((IFluentMap)this).LazyInstance(name, isLazy);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// NullValueAttribute
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> NullValue<TR>(Expression<Func<T, TR>> prop, TR value)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).NullValue(name, value);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// AssociationAttribute
		/// </summary>
		/// <typeparam name="TRt"></typeparam>
		/// <typeparam name="TR"> </typeparam>
		/// <param name="prop"> </param>
		/// <param name="canBeNull"></param>
		/// <param name="thisKey"></param>
		/// <param name="thisKeys"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR>.AssociationMap<TRt> Association<TRt, TR>(Expression<Func<T, TR>> prop, bool canBeNull, Expression<Func<T, TRt>> thisKey, params Expression<Func<T, TRt>>[] thisKeys)
		{
			var keys = new List<Expression<Func<T, TRt>>>(thisKeys);
			keys.Insert(0, thisKey);
			return new MapFieldMap<T, TR>.AssociationMap<TRt>(new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop), canBeNull, keys);
		}

		/// <summary>
		/// AssociationAttribute
		/// </summary>
		/// <typeparam name="TRt"></typeparam>
		/// <typeparam name="TR"> </typeparam>
		/// <param name="prop"> </param>
		/// <param name="thisKey"></param>
		/// <param name="thisKeys"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR>.AssociationMap<TRt> Association<TRt, TR>(Expression<Func<T, TR>> prop, Expression<Func<T, TRt>> thisKey, params Expression<Func<T, TRt>>[] thisKeys)
		{
			return this.Association(prop, true, thisKey, thisKeys);
		}

		protected MapFieldMap<T, TR> Association<TRt, TR, TRf, TRo>(Expression<Func<T, TR>> prop, bool canBeNull
			, IEnumerable<Expression<Func<T, TRt>>> thisKeys, IEnumerable<Expression<Func<TRf, TRo>>> otherKeys)
		{
			string name = this.GetExprName(prop);
			((IFluentMap)this).Association(name, canBeNull, this.KeysToString(thisKeys.ToArray()), this.KeysToString(otherKeys.ToArray()));
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		/// <summary>
		/// Reverse on BLToolkit.Mapping.Association.ParseKeys()
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="keys"></param>
		/// <returns></returns>
		private string KeysToString<T1, T2>(IEnumerable<Expression<Func<T1, T2>>> keys)
		{
			return keys.Select(this.GetExprName).Aggregate((s1, s2) => s1 + ", " + s2);
		}

		/// <summary>
		/// RelationAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Relation<TR>(Expression<Func<T, TR>> prop, string slaveIndex = null, string masterIndex = null)
		{
			return this.Relation(prop, new[] { slaveIndex }, new[] { masterIndex });
		}

		/// <summary>
		/// RelationAttribute
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <returns></returns>
		public MapFieldMap<T, TR> Relation<TR>(Expression<Func<T, TR>> prop, string[] slaveIndex, string[] masterIndex)
		{
			string name = this.GetExprName(prop);

			slaveIndex = (slaveIndex ?? new string[0]).Where(i => !string.IsNullOrEmpty(i)).ToArray();
			masterIndex = (masterIndex ?? new string[0]).Where(i => !string.IsNullOrEmpty(i)).ToArray();

			Type destinationType = typeof(TR);

			((IFluentMap)this).Relation(name, destinationType, slaveIndex, masterIndex);
			return new MapFieldMap<T, TR>(this._typeExtension, this.Childs, prop);
		}

		static void FillRelationIndex(string[] index, AttributeExtension attributeExtension, string indexName)
		{
			if (index.Any())
			{
				var collection = new AttributeExtensionCollection();
				foreach (var s in index)
				{
					var ae = new AttributeExtension();
					ae.Values.Add(TypeExtension.AttrName.Name, s);
					collection.Add(ae);
				}
				attributeExtension.Attributes.Add(indexName, collection);
			}
		}

		/// <summary>
		/// MapValueAttribute
		/// </summary>
		/// <typeparam name="TV"></typeparam>
		/// <param name="origValue"></param>
		/// <param name="value"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public FluentMap<T> MapValue<TV>(Enum origValue, TV value, params TV[] values)
		{
			((IFluentMap)this).MapValue(origValue, value, values);
			return this;
		}

		/// <summary>
		/// MapValueAttribute
		/// </summary>
		/// <typeparam name="TV"></typeparam>
		/// <param name="origValue"></param>
		/// <param name="value"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public FluentMap<T> MapValue<TV>(object origValue, TV value, params TV[] values)
		{
			((IFluentMap)this).MapValue(origValue, value, values);
			return this;
		}

		/// <summary>
		/// MapFieldAttribute(isInheritanceDescriminator = true)
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="prop"></param>
		/// <returns></returns>
		public FluentMap<T> InheritanceField<TR>(Expression<Func<T, TR>> prop)
		{
			return this.MapField(prop, true);
		}

		/// <summary>
		/// InheritanceMappingAttribute
		/// </summary>
		/// <typeparam name="TC"></typeparam>
		/// <param name="code"></param>
		/// <returns></returns>
		public FluentMap<T> InheritanceMapping<TC>(object code)
		{
			return this.InheritanceMapping<TC>(code, null);
		}

		/// <summary>
		/// InheritanceMappingAttribute
		/// </summary>
		/// <typeparam name="TC"></typeparam>
		/// <param name="isDefault"></param>
		/// <returns></returns>
		public FluentMap<T> InheritanceMapping<TC>(bool isDefault)
		{
			return this.InheritanceMapping<TC>(null, isDefault);
		}

		/// <summary>
		/// InheritanceMappingAttribute
		/// </summary>
		/// <typeparam name="TC"></typeparam>
		/// <param name="code"></param>
		/// <param name="isDefault"></param>
		/// <returns></returns>
		public FluentMap<T> InheritanceMapping<TC>(object code, bool? isDefault)
		{
			((IFluentMap)this).InheritanceMapping(typeof(TC), code, isDefault);
			return this;
		}

		protected void FillMapValueExtension<TR, TV>(AttributeNameCollection attributeCollection, TR origValue, TV value, TV[] values)
		{
			AttributeExtensionCollection list;
			if (!attributeCollection.TryGetValue(Attributes.MapValue.Name, out list))
			{
				list = new AttributeExtensionCollection();
				attributeCollection.Add(Attributes.MapValue.Name, list);
			}

			var allValues = new List<TV>(values);
			allValues.Insert(0, value);
			var tvFullName = typeof(TV).FullName;

			foreach (var val in allValues)
			{
				var attributeExtension = new AttributeExtension();
				attributeExtension.Values.Add(Attributes.MapValue.OrigValue, origValue);
				attributeExtension.Values.Add(TypeExtension.ValueName.Value, Convert.ToString(val));
				attributeExtension.Values.Add(TypeExtension.ValueName.Value + TypeExtension.ValueName.TypePostfix, tvFullName);
				list.Add(attributeExtension);
			}
		}

		protected void FillMemberMapperExtension(AttributeNameCollection attributeCollection, Type memberType, Type memberMapperType)
		{
			AttributeExtensionCollection attrs;
			if (!attributeCollection.TryGetValue(Attributes.MemberMapper.Name, out attrs))
			{
				attrs = new AttributeExtensionCollection();
				attributeCollection.Add(Attributes.MemberMapper.Name, attrs);
			}
			var attributeExtension = new AttributeExtension();
			attributeExtension.Values.Add(Attributes.MemberMapper.MemberType, memberType);
			attributeExtension.Values.Add(Attributes.MemberMapper.MemberMapperType, memberMapperType);
			attrs.Add(attributeExtension);	       
		}

		/// <summary>
		/// Fluent settings result
		/// </summary>
		/// <returns></returns>
		public ExtensionList Map()
		{
			var result = new ExtensionList();
			this.MapTo(result);
			return result;
		}

		/// <summary>
		/// Apply fluent settings to DbManager
		/// </summary>
		/// <param name="dbManager"></param>
		public void MapTo(DbManager dbManager)
		{
			var ms = dbManager.MappingSchema ?? (dbManager.MappingSchema = Mapping.Map.DefaultSchema);
			this.MapTo(ms);
		}

		/// <summary>
		/// Apply fluent settings to DataProviderBase
		/// </summary>
		/// <param name="dataProvider"></param>
		public void MapTo(DataProviderBase dataProvider)
		{
			var ms = dataProvider.MappingSchema ?? (dataProvider.MappingSchema = Mapping.Map.DefaultSchema);
			this.MapTo(ms);
		}

		/// <summary>
		/// Apply fluent settings to MappingSchema
		/// </summary>
		/// <param name="mappingSchema"></param>
		public void MapTo(MappingSchema mappingSchema)
		{
			var extensions = mappingSchema.Extensions ?? (mappingSchema.Extensions = new ExtensionList());
			this.MapTo(extensions);
		}

		/// <summary>
		/// Apply fluent settings to ExtensionList
		/// </summary>
		/// <param name="extensions"></param>
		public void MapTo(ExtensionList extensions)
		{
			var ext = this._typeExtension;
			TypeExtension oldExt;
			if (extensions.TryGetValue(ext.Name, out oldExt))
			{
				FluentMapHelper.MergeExtensions(ext, ref oldExt);
			}
			else
			{
				extensions.Add(ext);
			}
			this.EachChilds(m => m.MapTo(extensions));
		}

		protected MemberExtension GetMemberExtension<TR>(Expression<Func<T, TR>> prop)
		{
			string name = this.GetExprName(prop);
			return this.GetMemberExtension(name);
		}

		protected MemberExtension GetMemberExtension(string name)
		{
			MemberExtension member;
			if (!this._typeExtension.Members.TryGetValue(name, out member))
			{
				member = new MemberExtension { Name = name };
				this._typeExtension.Members.Add(member);
			}
			return member;
		}

		private string GetExprName<TT, TR>(Expression<Func<TT, TR>> prop)
		{
			string result = null;
			var memberExpression = prop.Body as MemberExpression;
			while (null != memberExpression)
			{
				result = null == result ? "" : MemberNameSeparator + result;
				result = memberExpression.Member.Name + result;
				memberExpression = memberExpression.Expression as MemberExpression;
			}
			if (null == result)
			{
				throw new ArgumentException("Fail member access expression.");
			}
			return result;
		}

		static bool GetIsVirtual<TT, TR>(Expression<Func<TT, TR>> prop)
		{
			var memberExpression = prop.Body as MemberExpression;
			if (memberExpression != null)
			{
				var prpInfo = memberExpression.Member as PropertyInfo;
				if (prpInfo != null && !prpInfo.GetGetMethod().IsVirtual)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Invert for BLToolkit.Reflection.Extension.TypeExtension.ToBoolean()
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected string ToString(bool value)
		{
			return Convert.ToString(value);
		}

		private void EachChilds(Action<IFluentMap> action)
		{
			foreach (var childMap in this.Childs)
			{
				action(childMap);
			}
		}

		private List<IFluentMap> Childs
		{
			get
			{
				if (null == this._childs)
				{
					this._childs = new List<IFluentMap>();
					var thisType = typeof(T);
					var fmType = typeof(FluentMap<>);
					// Find child only first generation ... other generation find recursive
					foreach (var childType in thisType.Assembly.GetTypes().Where(t => t.BaseType == thisType))
					{
						this._childs.Add((IFluentMap)Activator.CreateInstance(fmType.MakeGenericType(childType)));
					}
				}
				return this._childs;
			}
		}
	}
}
