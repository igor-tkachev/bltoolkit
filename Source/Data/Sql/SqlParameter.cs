using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLToolkit.Data.Sql
{
	using Mapping;

    public class SqlParameter : SqlValueBase, ISqlExpression
	{
		public SqlParameter(Type systemType, string name, object value, MappingSchema mappingSchema)
		{
			IsQueryParameter = true;
			Name             = name;
			SystemType       = systemType;
			_value           = value;
			DbType           = DbType.Object;

			if (systemType != null && mappingSchema != null && systemType.IsEnum)
			{
			}
		}

		public SqlParameter(Type systemType, string name, object value, Converter<object,object> valueConverter)
			: this(systemType, name, value, (MappingSchema)null)
		{
			ValueConverter = valueConverter;
		}

		[Obsolete]
		public SqlParameter(string name, object value)
			: this(value == null ? null : value.GetType(), name, value, (MappingSchema)null)
		{
		}

		[Obsolete]
		public SqlParameter(string name, object value, Converter<object,object> valueConverter)
			: this(value == null ? null : value.GetType(), name, value, valueConverter)
		{
		}

		public string Name             { get; set; }
		public Type   SystemType       { get; set; }
		public bool   IsQueryParameter { get; set; }
		public DbType DbType           { get; set; }
		public int    DbSize           { get; set; }

		#region Overrides

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return ((IQueryElement)this).ToString(new StringBuilder(), new Dictionary<IQueryElement,IQueryElement>()).ToString();
		}

#endif

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, Func<ISqlExpression,ISqlExpression> func)
		{
			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			var p = other as SqlParameter;
			return (object)p != null && Name != null && p.Name != null && Name == p.Name && SystemType == p.SystemType;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			if (SystemType == null && _value == null)
				return true;

			return SqlDataType.CanBeNull(SystemType ?? _value.GetType());
		}

		public bool Equals(ISqlExpression other, Func<ISqlExpression,ISqlExpression,bool> comparer)
		{
			return ((ISqlExpression)this).Equals(other) && comparer(this, other);
		}

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement,ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
			{
				var p = new SqlParameter(SystemType, Name, _value, ValueConverter) { IsQueryParameter = IsQueryParameter, DbType = DbType, DbSize = DbSize };

				objectTree.Add(this, clone = p);
			}

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlParameter; } }

		StringBuilder IQueryElement.ToString(StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic)
		{
			return sb
				.Append('@')
				.Append(Name ?? "parameter")
				.Append('[')
				.Append(Value ?? "NULL")
				.Append(']');
		}

		#endregion
	}
}
