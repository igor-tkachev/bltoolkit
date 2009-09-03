using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlParameter : ISqlExpression
	{
		public SqlParameter(string name, object value)
		{
			_name  = name;
			_value = value;
		}

		public SqlParameter(string name, object value, Converter<object,object> valueConverter)
			: this(name, value)
		{
			_valueConverter = valueConverter;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		private object _value;
		public  object  Value
		{
			get { return _valueConverter == null? _value: _valueConverter(_value); }
			set { _value = value; }
		}

		//public object RawValue
		//{
		//	get { return _value; }
		//}

		private Converter<object, object> _valueConverter;
		public  Converter<object, object>  ValueConverter
		{
			get { return _valueConverter;  }
			set { _valueConverter = value; }
		}

		#region Overrides

		public override string ToString()
		{
			return "@" + (Name ?? "parameter") + "[" + (Value ?? "NULL") + "]";
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

			SqlParameter p = other as SqlParameter;
			return _name != null && p._name != null && _name == p._name;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return true;
		}

		public object Clone(Dictionary<object,object> objectTree)
		{
			object clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlParameter(_name, _value, _valueConverter));

			return clone;
		}

		#endregion
	}
}
