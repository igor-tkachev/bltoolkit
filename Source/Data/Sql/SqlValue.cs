using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlValue : ISqlExpression, IValueContainer
	{
		public SqlValue(object value)
		{
			_value = value;
		}

		readonly object _value;
		public   object  Value { get { return _value; } }

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
			return _value == null && value._value == null || _value != null && _value.Equals(value._value);
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return Value == null;
		}

		public object Clone(Dictionary<object,object> objectTree)
		{
			object clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlValue(_value));

			return clone;
		}

		#endregion
	}
}
