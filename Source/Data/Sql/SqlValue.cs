using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlValue : ISqlExpression, IValueContainer
	{
		public SqlValue(Type systemType, object value)
		{
			_systemType = systemType;
			_value      = value;
		}

		public SqlValue(object value)
		{
			_value = value;

			if (value != null)
				_systemType = value.GetType();
		}

		readonly object _value;      public object  Value      { get { return _value;      } }
		readonly Type   _systemType; public Type    SystemType { get { return _systemType; } }

		#region Overrides

		public override string ToString()
		{
			if (_value == null)
				return "NULL";

			if (_value is string)
				return "'" + _value.ToString().Replace("\'", "''");

			return _value.ToString();
		}

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
		{
			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			SqlValue value = other as SqlValue;
			return
				value       != null              &&
				_systemType == value._systemType &&
				(_value == null && value._value == null || _value != null && _value.Equals(value._value));
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return Value == null;
		}

		#endregion

		#region ISqlExpression Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlValue(_systemType, _value));

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlValue; } }

		#endregion
	}
}
