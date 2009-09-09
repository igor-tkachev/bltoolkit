using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlDataType : ISqlExpression
	{
		public SqlDataType([JetBrains.Annotations.NotNull]Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			_type = type;
		}

		public SqlDataType([JetBrains.Annotations.NotNull] Type type, int length)
		{
			if (type   == null) throw new ArgumentNullException      ("type");
			if (length <= 0)    throw new ArgumentOutOfRangeException("length");

			_type   = type;
			_length = length;
		}

		public SqlDataType([JetBrains.Annotations.NotNull] Type type, int size, int scale)
		{
			if (type  == null) throw new ArgumentNullException      ("type");
			if (size  <= 0)    throw new ArgumentOutOfRangeException("size");
			if (scale <= 0)    throw new ArgumentOutOfRangeException("scale");

			_type  = type;
			_size  = size;
			_scale = scale;
		}

		readonly Type _type;   public Type Type   { get { return _type;   } }
		readonly int  _length; public int  Length { get { return _length; } }
		readonly int  _size;   public int  Size   { get { return _size;   } }
		readonly int  _scale;  public int  Scale  { get { return _scale;  } }

		#region Overrides

		public override string ToString()
		{
			return _type.Name + (_length != 0 ? "(" + _length + ")" : "(" + _size + "," + _scale + ")");
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

			SqlDataType value = other as SqlDataType;
			return _type == value._type && _length == value._length && _size == value._size && _scale == value._scale;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return false;
		}

		public SqlDataType(Type type, int length, int size, int scale)
		{
			_type   = type;
			_length = length;
			_size   = size;
			_scale  = scale;
		}

		public object Clone(Dictionary<object,object> objectTree)
		{
			object clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlDataType(_type, _length, _size, _scale));

			return clone;
		}

		#endregion
	}
}
