using System;
using System.Collections.Generic;
using System.Text;
using BLToolkit.Mapping;

namespace BLToolkit.Data.Sql
{
	[Serializable]
	public class SqlParameter : ISqlExpression, IValueContainer
	{
		public SqlParameter(Type systemType, string name, object value)
		{
			IsQueryParameter = true;
			Name       = name;
			SystemType = systemType;
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

		public string Name             { get; set; }
		public Type   SystemType       { get; set; }
		public bool   IsQueryParameter { get; set; }

		private object _value;
		public  object  Value
		{
			get
			{
				var valueConverter = ValueConverter;
				return valueConverter == null? _value: valueConverter(_value);
			}

			set { _value = value; }
		}

		#region Value Converter

		List<Type> _enumTypes;
		List<int>  _takeValues;
		string     _likeStart, _likeEnd;

		[NonSerialized]
		private Converter<object,object> _valueConverter;
		public  Converter<object,object>  ValueConverter
		{
			get
			{
				if (_valueConverter == null)
				{
					if (_enumTypes != null)
						foreach (var type in _enumTypes)
							SetEnumConverter(type, Map.DefaultSchema);
					else if (_takeValues != null)
						foreach (var take in _takeValues)
							SetTakeConverter(take);
					else if (_likeStart != null)
						SetLikeConverter(_likeStart, _likeEnd);
				}

				return _valueConverter;
			}

			set { _valueConverter = value; }
		}

		internal void SetEnumConverter(Type type, MappingSchema ms)
		{
			if (_enumTypes == null)
				_enumTypes = new List<Type>();

			_enumTypes.Add(type);

			SetEnumConverterInternal(type, ms);
		}

		void SetEnumConverterInternal(Type type, MappingSchema ms)
		{
			if (_valueConverter == null)
			{
				_valueConverter = o => ms.MapEnumToValue(o, type);
			}
			else
			{
				var converter = _valueConverter;
				_valueConverter = o => ms.MapEnumToValue(converter(o), type);
			}
		}

		internal void SetTakeConverter(int take)
		{
			if (_takeValues == null)
				_takeValues = new List<int>();

			_takeValues.Add(take);

			SetTakeConverterInternal(take);
		}

		void SetTakeConverterInternal(int take)
		{
			var conv = _valueConverter;

			if (conv == null)
				_valueConverter = v => v == null ? null : (object) ((int) v + take);
			else
				_valueConverter = v => v == null ? null : (object) ((int) conv(v) + take);
		}

		internal void SetLikeConverter(string start, string end)
		{
			_likeStart = start;
			_likeEnd   = end;
			_valueConverter = GetLikeEscaper(start, end);
		}

		static Converter<object,object> GetLikeEscaper(string start, string end)
		{
			return value =>
			{
				if (value == null)
#if DEBUG
					value = "";
#else
					throw new SqlException("NULL cannot be used as a LIKE predicate parameter.");
#endif

				var text = value.ToString();

				if (text.IndexOfAny(new[] { '%', '_', '[' }) < 0)
					return start + text + end;

				var sb = new StringBuilder(start, text.Length + start.Length + end.Length);

				foreach (var c in text)
				{
					if (c == '%' || c == '_' || c == '[')
					{
						sb.Append('[');
						sb.Append(c);
						sb.Append(']');
					}
					else
						sb.Append(c);
				}

				return sb.ToString();
			};
		}

		#endregion

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

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
			{
				var p = new SqlParameter(SystemType, Name, _value, _valueConverter) { IsQueryParameter = IsQueryParameter };

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
