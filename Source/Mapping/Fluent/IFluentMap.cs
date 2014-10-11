using System;
using System.Data;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.Fluent
{
	/// <summary>
	/// Interface fluent mapping
	/// </summary>
	public interface IFluentMap
	{
		/// <summary>
		/// Get mapping
		/// </summary>
		/// <returns></returns>
		ExtensionList Map();

		/// <summary>
		/// TableNameAttribute
		/// </summary>
		/// <param name="database"></param>
		/// <param name="owner"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		void TableName(string database, string owner, string name);

		/// <summary>
		/// Map to ExtensionList
		/// </summary>
		/// <param name="extensions"></param>
		void MapTo(ExtensionList extensions);

		/// <summary>
		/// Maps the field.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="mapName">Name of the map.</param>
		/// <param name="storage">The storage.</param>
		/// <param name="isInheritanceDiscriminator">The is inheritance discriminator.</param>
		/// <returns></returns>
		void MapField(string propName, string mapName, string storage, bool? isInheritanceDiscriminator);

		/// <summary>
		/// Primaries the key.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="order">The order.</param>
		void PrimaryKey(string propName, int order);

		/// <summary>
		/// Nons the updatable.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		void NonUpdatable(string propName);

		/// <summary>
		/// Identities the specified prop name.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		void Identity(string propName);

		/// <summary>
		/// SQLs the ignore.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="ignore">if set to <c>true</c> [ignore].</param>
		void SqlIgnore(string propName, bool ignore);

		/// <summary>
		/// Maps the ignore.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="ignore">if set to <c>true</c> [ignore].</param>
		void MapIgnore(string propName, bool ignore);

		/// <summary>
		/// Trimmables the specified prop name.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		void Trimmable(string propName);

		/// <summary>
		/// Maps the value.
		/// </summary>
		/// <typeparam name="TR">The type of the R.</typeparam>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="origValue">The orig value.</param>
		/// <param name="value">The value.</param>
		/// <param name="values">The values.</param>
		void MapValue<TR, TV>(string propName, TR origValue, TV value, TV[] values);

		/// <summary>
		/// Maps the value.
		/// </summary>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="origValue">The orig value.</param>
		/// <param name="value">The value.</param>
		/// <param name="values">The values.</param>
		void MapValue<TV>(Enum origValue, TV value, TV[] values);

		/// <summary>
		/// Maps the value.
		/// </summary>
		/// <typeparam name="TV">The type of the V.</typeparam>
		/// <param name="origValue">The orig value.</param>
		/// <param name="value">The value.</param>
		/// <param name="values">The values.</param>
		void MapValue<TV>(object origValue, TV value, TV[] values);

		/// <summary>
		/// Defauls the value.
		/// </summary>
		/// <typeparam name="TR">The type of the R.</typeparam>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="value">The value.</param>
		void DefaulValue<TR>(string propName, TR value);

        /// <summary>
		/// Defauls the value.
		/// </summary>
		/// <typeparam name="TR">The type of the R.</typeparam>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="value">The value.</param>
        void DbType<TR>(string propName, DbType value);

        /// <summary>
        /// MemberMapper
		/// </summary>
		/// <typeparam name="TR">The type of the R.</typeparam>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="value">The value.</param>
        void MemberMapper<TR>(string propName, Type memberType, Type memberMapperType);
        
		/// <summary>
		/// Nullables the specified prop name.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
		void Nullable(string propName, bool isNullable);

        void LazyInstance(string propName, bool isLazy);

		/// <summary>
		/// Nulls the value.
		/// </summary>
		/// <typeparam name="TR">The type of the R.</typeparam>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="value">The value.</param>
		void NullValue<TR>(string propName, TR value);

		/// <summary>
		/// Associations the specified prop name.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="canBeNull">if set to <c>true</c> [can be null].</param>
		/// <param name="thisKeys">The this keys.</param>
		/// <param name="otherKeys">The other keys.</param>
		void Association(string propName, bool canBeNull, string thisKeys, string otherKeys);

		/// <summary>
		/// Relations the specified prop name.
		/// </summary>
		/// <param name="propName">Name of the prop.</param>
		/// <param name="destinationType">Type of the destination.</param>
		/// <param name="slaveIndex">Index of the slave.</param>
		/// <param name="masterIndex">Index of the master.</param>
		void Relation(string propName, Type destinationType, string[] slaveIndex, string[] masterIndex);

		/// <summary>
		/// Inheritances the mapping.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="code">The code.</param>
		/// <param name="isDefault">The is default.</param>
		void InheritanceMapping(Type type, object code, bool? isDefault);
	}
}