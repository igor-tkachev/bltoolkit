using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql
{
	public abstract class SqlValueBase: IValueContainer
	{
		private object _convertedValue;

		[CLSCompliant(false)]
		protected      object _value;
		public virtual object  Value
		{
			get
			{
				if (_convertedValue != null)
					return _convertedValue;

				var valueConverter = ValueConverter;

				if (valueConverter == null)
					return _convertedValue = _value;

				#region enumeration vas done to issue 344, moved to MappingSchema

//				if (_value == null)
//					return _convertedValue = valueConverter(_value);

//				if (/*!(_value is string)*/ _isEnumConverterSet
//					&& _value is IEnumerable)
//				{
//#if SILVERLIGHT
//					var res = new List<Object>(10);
//#else
//					var res = new ArrayList(10);
//#endif
//					var e = ((IEnumerable)_value);
//					var ut = null as Type;

//					foreach (var val in e)
//					{
//						var element = valueConverter(val);
//						res.Add(element);

//						if (ut == null)
//							ut = element.GetType();
//						else if (ut != element.GetType())
//							ut = typeof(object);
//					}

//					if (ut == null)
//						ut = typeof(object);

//					if (ut != typeof(object))
//					{
//						var array = Array.CreateInstance(ut, res.Count);
//#if !SILVERLIGHT
//						res.CopyTo(array);
//#else 
//						for(var i = 0; i < res.Count; i++)
//							array.SetValue(res[i], i);
//#endif
//						_convertedValue = array;
//					}
//					else
//						_convertedValue = res.ToArray();

//				}
//				else

				#endregion
				_convertedValue = valueConverter(_value);

				return _convertedValue;
			}

			set
			{
				_value = value;
				_convertedValue = null;
			}
		}

		#region Value Converter

		internal List<Type> EnumTypes;
		internal List<int>  TakeValues;
		internal string     LikeStart, LikeEnd;

		private Converter<object,object> _valueConverter;
		public  Converter<object,object>  ValueConverter
		{
			get
			{
				if (_valueConverter == null)
				{
					if (EnumTypes != null)
						foreach (var type in EnumTypes.ToArray())
							SetEnumConverter(type, Map.DefaultSchema);
					else if (TakeValues != null)
						foreach (var take in TakeValues.ToArray())
							SetTakeConverter(take);
					else if (LikeStart != null)
						SetLikeConverter(LikeStart, LikeEnd);
				}

				return _valueConverter;
			}

			set
			{
				_valueConverter = value;
				_convertedValue = null;
			}
		}

		bool _isEnumConverterSet;

		internal void SetEnumConverter(Type type, MappingSchema ms)
		{
			if (!_isEnumConverterSet)
			{
				_isEnumConverterSet = true;

				if (EnumTypes == null)
					EnumTypes = new List<Type>();

				EnumTypes.Add(type);

				SetEnumConverterInternal(type, ms);
			}
		}

		internal void SetEnumConverter(MemberAccessor ma, MappingSchema ms)
		{
			if (!_isEnumConverterSet)
			{
				_isEnumConverterSet = true;

				if (EnumTypes == null)
					EnumTypes = new List<Type>();

				EnumTypes.Add(ma.Type);

				SetEnumConverterInternal(ma, ms);
			}
		}

		void SetEnumConverterInternal(MemberAccessor ma, MappingSchema ms)
		{
			if (_valueConverter == null)
			{
				_valueConverter = o => ms.MapEnumToValue(o, ma, true);
			}
			else
			{
				var converter = _valueConverter;
				_valueConverter = o => ms.MapEnumToValue(converter(o), ma, true);
			}
			// update system type in SqlValue :-/
			var tmp = Value;
		}

		void SetEnumConverterInternal(Type type, MappingSchema ms)
		{
			if (_valueConverter == null)
			{
				_valueConverter = o => ms.MapEnumToValue(o, type, true);
			}
			else
			{
				var converter = _valueConverter;
				_valueConverter = o => ms.MapEnumToValue(converter(o), type, true);
			}
			// update system type in SqlValue :-/
			var tmp = Value;
		}

		internal void SetTakeConverter(int take)
		{
			if (TakeValues == null)
				TakeValues = new List<int>();

			TakeValues.Add(take);

			SetTakeConverterInternal(take);
		}

		void SetTakeConverterInternal(int take)
		{
			var conv = _valueConverter;

			if (conv == null)
				_valueConverter = v => v == null ? null : (object)((int)v + take);
			else
				_valueConverter = v => v == null ? null : (object)((int)conv(v) + take);
		}

		internal void SetLikeConverter(string start, string end)
		{
			LikeStart = start;
			LikeEnd = end;
			_valueConverter = GetLikeEscaper(start, end);
		}

		static Converter<object, object> GetLikeEscaper(string start, string end)
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
	}
}
