using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql
{
	public class SqlParameter : ISqlExpression, IValueContainer
	{
		public SqlParameter(Type systemType, string name, object value)
		{
			_name       = name;
			_systemType = systemType;
			_value      = value;
		}

		public SqlParameter(Type systemType, string name, object value, Converter<object,object> valueConverter)
			: this(systemType, name, value)
		{
			_valueConverter = valueConverter;
		}

		[Obsolete]
		public SqlParameter(string name, object value)
			: this(value == null ? null : value.GetType(), name, value)
		{
		}

		[Obsolete]
		public SqlParameter(string name, object value, Converter<object,object> valueConverter)
			: this(value == null ? null : value.GetType(), name, value, valueConverter)
		{
		}

		private string _name;       public string Name       { get { return _name;       } set { _name = value;       } }
		private Type   _systemType; public Type   SystemType { get { return _systemType; } set { _systemType = value; } }

		private object _value;
		public  object  Value
		{
			get { return _valueConverter == null? _value: _valueConverter(_value); }
			set { _value = value; }
		}

		private Converter<object,object> _valueConverter;
		public  Converter<object,object>  ValueConverter
		{
			get { return _valueConverter;  }
			set { _valueConverter = value; }
		}

		private bool _isQueryParameter = true;
		public  bool  IsQueryParameter
		{
			get { return _isQueryParameter;  }
			set { _isQueryParameter = value; }
		}

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
			return (object)p != null && _name != null && p._name != null && _name == p._name && _systemType == p._systemType;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			if (_systemType == null && _value == null)
				return true;

			return SqlDataType.CanBeNull(_systemType ?? _value.GetType());
		}

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
			{
				var p = new SqlParameter(_systemType, _name, _value, _valueConverter) { _isQueryParameter = _isQueryParameter };

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
